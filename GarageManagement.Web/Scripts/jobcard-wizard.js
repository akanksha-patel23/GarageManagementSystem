// jobcard-wizard.js
// Depends on jQuery (loaded globally by Site.Master).
// apiBaseUrl is read from a hidden field the page renders server-side from
// Web.config's ApiBaseUrl appSetting — per 08-environment.md, never hardcode
// the API URL into this file directly.
//
// IMPORTANT: the Wizard is wrapped in an UpdatePanel, so Next/Previous cause
// PARTIAL postbacks that replace the DOM nodes inside it. All event bindings
// below use delegation ($(document).on(event, selector, handler)) instead of
// direct binding ($(selector).on(...)) — direct bindings would attach to the
// OLD nodes and silently stop working the moment a partial postback swaps
// them out for new ones with the same id.

$(function () {
    var apiBaseUrl = $('#hdnApiBaseUrl').val();

    function populateVehicles(customerId, restoreVehicleId) {
        var $vehicleDropdown = $('#ddlVehicle');
        $vehicleDropdown.empty();

        if (!customerId) {
            $vehicleDropdown.append('<option value="">-- Select Customer First --</option>');
            return;
        }

        $vehicleDropdown.append('<option value="">Loading...</option>');

        $.ajax({
            url: apiBaseUrl + '/api/vehicles/' + customerId,
            method: 'GET',
            dataType: 'json'
        }).done(function (vehicles) {
            $vehicleDropdown.empty();
            if (vehicles.length === 0) {
                $vehicleDropdown.append('<option value="">No vehicles for this customer</option>');
                return;
            }
            $vehicleDropdown.append('<option value="">-- Select Vehicle --</option>');
            $.each(vehicles, function (i, v) {
                $vehicleDropdown.append(
                    $('<option>').val(v.vehicleID).text(v.regNumber + ' \u2014 ' + v.make + ' ' + v.model)
                );
            });
            if (restoreVehicleId) {
                $vehicleDropdown.val(restoreVehicleId);
            }
        }).fail(function () {
            $vehicleDropdown.empty().append('<option value="">Unable to load vehicles</option>');
        });
    }

    function populateServices(categoryId, restoreServiceIds) {
        var $servicesList = $('#chkServices');
        $servicesList.empty();

        if (!categoryId) return;

        $.ajax({
            url: apiBaseUrl + '/api/servicecatalog?categoryId=' + categoryId,
            method: 'GET',
            dataType: 'json'
        }).done(function (services) {
            if (services.length === 0) {
                $servicesList.append('<p class="msc-hint">No services in this category.</p>');
                return;
            }
            $.each(services, function (i, s) {
                var shouldCheck = restoreServiceIds && restoreServiceIds.indexOf(String(s.serviceID)) > -1;
                var $checkbox = $('<input type="checkbox" class="msc-service-checkbox">')
                    .attr('data-service-id', s.serviceID)
                    .attr('data-price', s.basePrice);
                if (shouldCheck) {
                    $checkbox.prop('checked', true);
                }

                var $item = $('<label class="msc-checkbox-item"></label>')
                    .append($checkbox)
                    .append(' ' + s.serviceName + ' ($' + s.basePrice.toFixed(2) + ')');
                $servicesList.append($item);
            });
            recalculateTotal();
        }).fail(function () {
            $servicesList.append('<p class="msc-message msc-message-error">Unable to load services.</p>');
        });
    }

    function recalculateTotal() {
        var total = 0;
        $('.msc-service-checkbox:checked').each(function () {
            total += parseFloat($(this).attr('data-price'));
        });
        $('#lblLiveTotal').text('$' + total.toFixed(2));
    }

    function showWizardError(message) {
        $('#lblWizardError').text(message).show();
    }

    // ---- Vehicle step (Flow C) ----
    $(document).on('change', '#ddlCustomer', function () {
        populateVehicles($(this).val(), null);
    });

    // ---- Services step (Flow D) ----
    $(document).on('change', '#ddlCategory', function () {
        populateServices($(this).val(), []);
    });

    $(document).on('change', '.msc-service-checkbox', recalculateTotal);

    // ---- Persist client-only state across the Wizard's partial postbacks ----
    // Before each partial postback (clicking Next/Previous/the Calendar), stash
    // the current Vehicle selection and checked Service IDs into hidden fields.
    // After the postback completes, ddlCustomer/ddlCategory's OWN selected
    // values already survived automatically (they're real server controls,
    // persisted via ViewState) — but their AJAX-populated child options did
    // not, so re-fetch and then restore from the hidden fields.
    if (window.Sys && window.Sys.WebForms) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function () {
            $('#hdnSelectedVehicleId').val($('#ddlVehicle').val() || '');

            var checkedIds = $('.msc-service-checkbox:checked').map(function () {
                return $(this).attr('data-service-id');
            }).get();
            $('#hdnCheckedServiceIds').val(checkedIds.join(','));
        });

        prm.add_endRequest(function () {
            var customerId = $('#ddlCustomer').val();
            if (customerId) {
                populateVehicles(customerId, $('#hdnSelectedVehicleId').val());
            }

            var categoryId = $('#ddlCategory').val();
            var checkedIdsRaw = $('#hdnCheckedServiceIds').val();
            var checkedIds = checkedIdsRaw ? checkedIdsRaw.split(',') : [];
            if (categoryId) {
                populateServices(categoryId, checkedIds);
            }
        });
    }

    // ---- Final step: POST the whole job card to the API instead of a postback ----
    $(document).on('click', '#btnCreateJobCard', function (e) {
        e.preventDefault();

        var vehicleId = parseInt($('#ddlVehicle').val(), 10);
        if (!vehicleId) {
            showWizardError('Please select a vehicle before continuing.');
            return;
        }

        var services = [];
        $('.msc-service-checkbox:checked').each(function () {
            services.push({
                serviceID: parseInt($(this).attr('data-service-id'), 10),
                qty: 1,
                price: parseFloat($(this).attr('data-price'))
            });
        });

        if (services.length === 0) {
            showWizardError('Select at least one service.');
            return;
        }

        var payload = {
            vehicleID: vehicleId,
            scheduledDate: $('#txtScheduledDate').val(),
            technicianID: $('#ddlTechnician').val() ? parseInt($('#ddlTechnician').val(), 10) : null,
            services: services,
            parts: [] // populated the same way from a Parts CheckBoxList once that step is built
        };

        $('#btnCreateJobCard').prop('disabled', true).text('Creating...');

        $.ajax({
            url: apiBaseUrl + '/api/jobcards',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload)
        }).done(function (result) {
            window.location.href = 'JobCardList.aspx?created=' + result.jobCardID;
        }).fail(function (xhr) {
            // xhr.responseJSON is a ProblemDetails object per 03-error-handling.md
            var message = (xhr.responseJSON && xhr.responseJSON.detail) || 'Unable to create job card.';
            showWizardError(message);
        }).always(function () {
            $('#btnCreateJobCard').prop('disabled', false).text('Create Job Card');
        });
    });
});