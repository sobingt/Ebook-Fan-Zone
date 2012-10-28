$(document).ready(function () {
    $("#signupform").validate({
        rules: {
            Username: {
                minlength: 6,
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
                minlength: 6,
            }
        },
        highlight: function (label) {
            $(label).closest('.control-group').addClass('error');
        },
        success: function (label) {
            $(label).closest('.control-group').addClass('success');
        }
    });

    $("#loginForm").validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true
            }
        },
        highlight: function (label) {
            $(label).closest('.control-group').addClass('error');
        },
        success: function (label) {
            $(label).closest('.control-group').addClass('success');
        }
    });
});