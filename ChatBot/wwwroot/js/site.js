// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $('#send').click(
        function () {
            var message = $('message').val();

            var url = "api/chat";

            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: message,
                success: function (data) {
                    $('#display-message').append(`Você: ${message} \n`);
                    $('#display-message').append(`${data.response} \n`);


                    $('#nessage').val("");
                }
            });
        })
});