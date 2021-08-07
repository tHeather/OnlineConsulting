"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessageAsync", ({ createDate, content }) => {
    
    const li = document.createElement("li");
    const span = document.createElement('span');
    const messageList = document.getElementById("messagesList");

    span.dataset.utcDate = createDate;
    li.textContent = content;
    li.appendChild(span);

    if (messageList.lastChild) {
        messageList.lastChild.after(li);
    } else {
        messageList.appendChild(li);
    }

    convertUtcToLocalDate();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageAsync", message, null).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
