// jobcard-wizard.js
// Depends on jQuery (loaded globally by Site.Master).
// apiBaseUrl is read from a hidden field the page renders server-side from
// Web.config's ApiBaseUrl appSetting — per 08-environment.md, never hardcode
// the API URL into this file directly.

$(function () {
    var apiBaseUrl = $('#hdnApiBaseUrl').val();

    var jwtToken = $('#hdnJwtToken').val();

    // Attaches Authorization: Bearer <token> to every AJAX call this file
    // makes, since Program.cs now requires [Authorize] on every controller.
    $.ajaxSetup({
        beforeSend: function (xhr) {
            if (jwtToken) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + jwtToken);
            }
        }
    });

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
            // Re-snapshot after restoring checked state, since the checkboxes
            // just changed identity (freshly rendered elements).
            captureServiceData();
        }).fail(function (xhr) {
            var message = (xhr.responseJSON && xhr.responseJSON.detail) || 'Unable to load services.';
            $servicesList.append($('<p class="msc-message msc-message-error"></p>').text(message));
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

    function clearWizardError() {
        $('#lblWizardError').text('').hide();
    }

    // ---- Vehicle step (Flow C) ----
    $(document).on('change', '#ddlCustomer', function () {
        populateVehicles($(this).val(), null);
    });

    // ---- Services step (Flow D) ----
    $(document).on('change', '#ddlCategory', function () {
        populateServices($(this).val(), []);
    });

    $(document).on('change', '.msc-service-checkbox', function () {
        recalculateTotal();
        captureServiceData();
    });

    // ---- Final step: POST the whole job card to the API instead of a postback ----
    $(document).on('click', '#btnCreateJobCard', function (e) {
        e.preventDefault();

        // asp:Wizard only renders the ACTIVE step's markup. On the Review step,
        // #ddlVehicle/.msc-service-checkbox/#txtScheduledDate/#ddlTechnician are
        // not in the DOM at all, so read from the hidden fields that
        // captureWizardState()/captureServiceData() keep current instead.
        var vehicleId = parseInt($('#hdnSelectedVehicleId').val(), 10);
        if (!vehicleId) {
            showWizardError('Please select a vehicle before continuing.');
            return;
        }

        var servicesRaw = $('#hdnCheckedServiceData').val();
        var services = [];
        try {
            services = servicesRaw ? JSON.parse(servicesRaw) : [];
        } catch (ex) {
            services = [];
        }

        if (services.length === 0) {
            showWizardError('Select at least one service.');
            return;
        }

        var scheduledDate = $('#hdnScheduledDate').val();
        if (!scheduledDate) {
            showWizardError('Please select a scheduled date.');
            return;
        }

        var payload = {
            vehicleID: vehicleId,
            scheduledDate: scheduledDate,
            technicianID: $('#hdnTechnicianId').val() ? parseInt($('#hdnTechnicianId').val(), 10) : null,
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
            var message = (xhr.responseJSON && xhr.responseJSON.detail) || 'Unable to create job card.';
            showWizardError(message);
        }).always(function () {
            $('#btnCreateJobCard').prop('disabled', false).text('Create Job Card');
        });
    });

    // ---- Persist client-only state across the Wizard's postbacks ----

    function captureServiceData() {
        // Guard: only overwrite when the service list is actually in the DOM
        // (the user is on the Services step). Without this, navigating away
        // from any other step would zero-out previously captured service data.
        if (!$('#chkServices').length) { return; }

        var checkedIds = [];
        var serviceData = [];
        $('.msc-service-checkbox:checked').each(function () {
            var id = $(this).attr('data-service-id');
            var price = $(this).attr('data-price');
            checkedIds.push(id);
            serviceData.push({
                serviceID: parseInt(id, 10),
                qty: 1,
                price: parseFloat(price)
            });
        });
        $('#hdnCheckedServiceIds').val(checkedIds.join(','));
        $('#hdnCheckedServiceData').val(JSON.stringify(serviceData));
    }

    function captureWizardState() {
        // FIX (Bug A): guard the vehicle read the same way schedule/technician
        // already are. Without this guard, $('#ddlVehicle') matches nothing on
        // any step but Vehicle, .val() returns undefined, and
        // `undefined || ''` blanks the hidden field on every navigation away
        // from step 1 — which is exactly why the selection was being lost.
        if ($('#ddlVehicle').length) {
            $('#hdnSelectedVehicleId').val($('#ddlVehicle').val() || '');
        }

        captureServiceData();

        if ($('#txtScheduledDate').length) {
            $('#hdnScheduledDate').val($('#txtScheduledDate').val() || '');
        }
        if ($('#ddlTechnician').length) {
            $('#hdnTechnicianId').val($('#ddlTechnician').val() || '');
        }

        // Minor UX fix: an error from a previous step shouldn't linger once
        // the user has navigated away and is trying something new.
        clearWizardError();
    }

    function restoreWizardState() {
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
    }

    // Wizard buttons and sidebar links initiate the Web Forms postback. Capture
    // during the click, before ASP.NET AJAX serializes the form.
    $(document).on('click', '#jobCardWizard input[type="submit"], ' +
        '#jobCardWizard button[type="submit"], ' +
        '#jobCardWizard a[href^="javascript:__doPostBack"]', captureWizardState);
    $('#mainForm').on('submit', captureWizardState);

    // FIX (Bug B): { childList: true, subtree: true } was also catching
    // populateServices()'s own .empty()/.append() calls on #chkServices (a
    // nested descendant of #wizardUpdatePanel), re-triggering
    // restoreWizardState() -> populateServices() -> empty() -> observer fires
    // again, an infinite loop. { childList: true } (no subtree) only observes
    // the panel's own direct children, which is exactly what changes when
    // ASP.NET AJAX swaps the panel's content on a genuine step navigation —
    // and correctly ignores mutations from this script's own later calls.
    var restoreTimer;
    var updatePanel = document.getElementById('wizardUpdatePanel');
    if (updatePanel && window.MutationObserver) {
        new MutationObserver(function () {
            window.clearTimeout(restoreTimer);
            restoreTimer = window.setTimeout(restoreWizardState, 0);
        }).observe(updatePanel, { childList: true });
    }
});