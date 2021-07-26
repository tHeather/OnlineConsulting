"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessageAsync", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = message;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var origin = window.location.href;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageAsync", origin, message, null).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});