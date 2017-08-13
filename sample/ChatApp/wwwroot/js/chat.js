var webSocket;
$().ready(function () {
    webSocket = new WebSocket("ws://localhost:52295/chat/submit");
    webSocket.onopen = function () { console.log("connected"); };
    webSocket.onmessage = function (evt) { $("#chat-container").append(evt.data); };
    webSocket.onerror = function (evt) { alert(evt.message); };
    webSocket.onclose = function () { console.log("disconnected"); };

    $("body").on("click", "#message-submit", function() {
        var payload = {
            username: $("#Username").val(),
            message: $("#message-input").val()
        };

        if (payload.message === "") {
            return;
        }

        webSocket.send(JSON.stringify(payload));
    });
});