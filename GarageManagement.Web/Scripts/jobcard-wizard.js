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
            // After restoring checked state, re-snapshot service data so hdnCheckedServiceData
            // reflects the restored selection (the user may have navigated back to this step).
            captureServiceData();
        }).fail(function (xhr) {
            var message = (xhr.responseJSON && xhr.responseJSON.detail) ||
                'Unable to load services.';
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

    // ---- Vehicle step (Flow C) ----
    $(document).on('change', '#ddlCustomer', function () {
        populateVehicles($(this).val(), null);
    });

    // ---- Services step (Flow D) ----
    $(document).on('change', '#ddlCategory', function () {
        // When the user manually changes the category, clear the service list
        // (no restore needed - they're making a fresh selection).
        populateServices($(this).val(), []);
    });

    $(document).on('change', '.msc-service-checkbox', function () {
        recalculateTotal();
        captureServiceData();   // keep hdnCheckedServiceData current as the user ticks boxes
    });

    // ---- Final step: POST the whole job card to the API instead of a postback ----
    // Register this BEFORE the state-management block. A failure wiring the MutationObserver
    // must never prevent the Create button from working.
    $(document).on('click', '#btnCreateJobCard', function (e) {
        e.preventDefault();

        // -----------------------------------------------------------------------
        // FIX: asp:Wizard only renders the ACTIVE step's content.  On the Review
        // step, #ddlVehicle, .msc-service-checkbox, #txtScheduledDate, and
        // #ddlTechnician are all in earlier steps and are NOT in the DOM.
        // Reading them directly always yields undefined/empty, so we read from
        // the hidden fields that captureWizardState() and captureServiceData()
        // keep up-to-date on every navigation click and checkbox change.
        // -----------------------------------------------------------------------

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
            // xhr.responseJSON is a ProblemDetails object per 03-error-handling.md
            var message = (xhr.responseJSON && xhr.responseJSON.detail) || 'Unable to create job card.';
            showWizardError(message);
        }).always(function () {
            $('#btnCreateJobCard').prop('disabled', false).text('Create Job Card');
        });
    });

    // ---- Persist client-only state across the Wizard's partial postbacks ----
    // Do not depend on Sys.WebForms/PageRequestManager here.  Some ScriptManager
    // load orders do not expose it when this page-level script executes.  Saving
    // state on the native navigation click works for both async and full postbacks.

    // captureServiceData() is called independently whenever checkboxes change so
    // hdnCheckedServiceData stays current without waiting for a navigation click.
    function captureServiceData() {
        // Guard: only overwrite when the service list is actually in the DOM (i.e. the
        // user is on step 2).  Without this, navigating away from any other step would
        // zero-out the previously captured service data.
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
        // Full {serviceID, qty, price} objects so btnCreateJobCard can POST without
        // re-querying checkboxes that are off-screen on the Review step.
        $('#hdnCheckedServiceData').val(JSON.stringify(serviceData));
    }

    function captureWizardState() {
        // Step 1 — vehicle
        $('#hdnSelectedVehicleId').val($('#ddlVehicle').val() || '');

        // Step 2 — services (guarded; captureServiceData is also called on checkbox change)
        captureServiceData();

        // Step 3 — schedule / technician.
        // Guard: only write when in scope so navigating from step 1→2 doesn't blank
        // a date that was already saved when the user was on step 3.
        if ($('#txtScheduledDate').length) {
            $('#hdnScheduledDate').val($('#txtScheduledDate').val() || '');
        }
        if ($('#ddlTechnician').length) {
            $('#hdnTechnicianId').val($('#ddlTechnician').val() || '');
        }
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

    // UpdatePanel replaces its descendants rather than reloading the document.
    // Observing that replacement provides a dependable end-request signal without
    // depending on the optional Sys.WebForms global.
    var restoreTimer;
    var updatePanel = document.getElementById('wizardUpdatePanel');
    if (updatePanel && window.MutationObserver) {
        new MutationObserver(function () {
            window.clearTimeout(restoreTimer);
            restoreTimer = window.setTimeout(restoreWizardState, 0);
        }).observe(updatePanel, { childList: true, subtree: true });
    }
});