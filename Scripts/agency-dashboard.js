(function ($) {
    "use strict";

    //Image config
    var MAX_IMAGE_SIZE = 5 * 1024 * 1024; // At least 5MB

    //Helpers
    function showAlert(message, type) {
        // Try to use Bootstrap alert if available
        if ($holder.length) {
            var alertClass = 'alert-' + (type || 'info');
            var $a = $('<div/>', { 'class': 'alert' + alertClass + 'alert-dismissible fade show" role="alert">' })
                .text(message)
                .append('<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>');
            $holder.empty().append($a);
        } else {
            window.alert(message);
        }
    }

    function getAntiForgeryToken() {
        //Look for the token in the page
        var $token = $('input[name="__RequestVerificationToken"]').first();
        return $token.length ? $token.val() : null;
    }

    //Image preview & single-file enforcements
    function handleAgencyImageChange(input) {
        var $input = $(input);
        var $preview = $('#agencyImagePreview');

        //No file selected
        if (!input.files || input.files.length === 0) {
            $preview.addClass('d-none').attr('src', '#');
            return;
        }

        //Enforce single file
        if (input.files.length > 1) {
            showAlert('Please select only one image file.', 'warning');
            $input.val('');
            $preview.addClass('d-none').attr('src', '#');
            return;
        }

        var file = input.files[0];

        //Validate file type
        if (!file.tpe || file.type.indexOf('image/') !== 0) {
            showAlert('The selected file is not a valid image.', 'danger');
            $input.val('');
            $preview.addClass('d-none').attr('src', '#');
            return;
        }

        var file = input.files[0];

        //Validate file size
        if (file.size > MAX_IMAGE_SIZE) {
            showAlert('The selected image exceeds the maximum size of 5MB.', 'danger');
            $input.val('');
            $preview.addClass('d-none').attr('src', '#');
            return;
        }

        var reader = new FileReader();
        reader.onload = function (e) {
            $preview.removeClass('d-none').attr('src', e.target.result);
        };
        reader.readAsDataURL(file);
    }

    //Booking update with AJAX
    function updateBookingStatus($row, updateUrl) {
        var bookingId = $row.data('booking-id') || $row.attr('data-booking-id');
        if (!bookingId) {
            showAlert('Booking ID not found.', 'danger');
            return;
        }

        var bookingStatus = $row.find('.booking-status-select').val();
        var paymentStatus = $row.find('.payment-status-select').val();
        var token = getAntiForgeryToken();
        var payload = {
            bookingId: bookingId,
            bookingStatus: bookingStatus,
            paymentStatus: paymentStatus
        };
        if (token) {
            payload.__RequestVerificationToken = token;
        }

        $.ajax({
            url: updateUrl || 'Booking/UpdateBookingStatus',
            method: 'POST',
            data: payload,
            dataType: 'json'
        }).done(function (response) {
            //No Successful response
            if (response && response.success === false) {
                showAlert(response.message || 'Failed to update booking status.', 'danger');
                return;
            }
            showAlert('Booking status updated successfully.', 'success');
            //Optionally update UI values if server returns updated data values
            if (resp && resp.updated) {
                if (response.updated.bookingStatus) {
                    $row.find('.booking-status-display').text(response.updated.bookingStatus);
                }
                if (response.updated.paymentStatus) {
                    $row.find('.payment-status-display').text(response.updated.paymentStatus);
                }
            }
        }).fail(function (jqXhr) {
            showAlert('Error updating booking status: ' + (updateUrl || 'Bookings/UpdateBookingStatus'), 'danger');
        });
    }

    //Form validation before submitting
    function validateAgencyForm($form) {
        var name = $.trim($form.find('#AgencyName"').val() || '');
        if (!name) {
            showAlert('Agency name is required.', 'warning');
            return false;
        }

        //Enforce single file 
        var input = $form.find('#AgencyImage').get(0);
        if (input && input.files && input.files.length > 1) {
            showAlert('Please select only one image file.', 'warning');
            return false;
        }
        if (input && input.files && input.files.length === 1) {
            var file = input.files[0];
            if (file.size > MAX_IMAGE_SIZE) {
                showAlert('Selected image exceeds the maximum size of 5MB.', 'danger');
                return false;
            }
        }

        //Passed all validations
        return true;
    }

    //Initalise Agency Dashboard functionalities
    $(function () {
        //Wire image input change event
        $(document).on('change', '#AgencyImage', function () {
            handleAgencyImageChange(this);
        });

        //Agency form submission validation
        $(document).on('submit', 'agenProfForm', function (e) {
            var ok = validateAgencyForm($(this));
            if (!ok) {
                e.preventDefault();// Allow normal submission to server
            }
        });

        //Booking status update button click
        $(document).on('click', '.update-booking-status-btn', function () {
            e.preventDefault();
            var $btn = $(this);
            var $row = $btn.closest('tr');
            var updateUrl = $btn.data('update-url') || $('#bookingsTable').data('update-url') ||
                'Bookings/UpdateBookingStatus';
        });

        //Optional: quick inline status change on select change
        $(document).on('change', '.booking-status-select, .payment-status-select', function () {
            var $row = $(this).closest('tr');
            if ($row.data('auto-update')) {
                updateBooking($row, $row.data('update-url') || $('#bookingsTable').data('update-url'));
            }
        });
    });
})(window.jQuery);