"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/expenseHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function () {
    const d = new Date();
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "New expense added: " + DateTime.now() ;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
            console.log("Connected");
        }).catch(function (err) {
            console.error(err.toString());
        });
//EVENT => invokes SendMessage from expenseHub
//document.getElementById("sendButton").addEventListener("click", function (event) {
//    connection.invoke("SendMessage").catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});
