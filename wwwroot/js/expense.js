"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/expenseHub").build();


connection.on("ReceiveMessage", function () {
    const d = new Date();
    var encodedMsg = "New expense added: " + d.getDate() + "-" + d.getMonth() + "-" + d.getFullYear() + "  Time: " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
            console.log("Connected");
        }).catch(function (err) {
            console.error(err.toString());
        });

