﻿
function showAjaxError(jsonError) {
	let err = jsonError.responseJSON;
	myAlert("Attenzione", err.userMessage);
}

function myAlert(title, message) {
    $.get({
        url: "/Home/_AlertModale/" + '?title=' + title + '&message=' + message,
        cache: false
    }).done(function (data) {
        $("#divMyModal").html(data);
        var myModal = new bootstrap.Modal(document.getElementById('myModal'));
        myModal.show();
    }).fail(function (data) {
        console.log(data);
    });
}