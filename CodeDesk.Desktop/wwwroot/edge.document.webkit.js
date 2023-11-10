window.__receiveMessageCallbacks = [];
window.__dispatchMessageCallback = function (message) {
    window.__receiveMessageCallbacks.forEach(function (callback) { callback(message); });
};
window.external = {
    sendMessage: function (message) {
        window.webkit.messageHandlers.CodeDeskInterop.postMessage(message);
    },
    receiveMessage: function (callback) {
        window.__receiveMessageCallbacks.push(callback);
    },
    focus: element => {
        element.focus();
    },
    shadow: id => {
        document.getElementById(id).attachShadow({ mode: 'open' });
    },
    systemCommand: (command, data) => {
        var value = {
            SystemCommand: command,
            Data: JSON.stringify(data)
        };
        window.external.sendMessage(JSON.stringify(value));
    },
    drag: selector => {
        $(selector).mousedown(function (win) {
            $(document).mousemove(function (e) {
                console.log((e.clientX - win.pageX) + ":" + (e.clientY - win.pageY));
                var data = {
                    SystemCommand: "Drag",
                    Data: (e.clientX - win.pageX) + ":" + (e.clientY - win.pageY)
                };
                window.external.sendMessage(JSON.stringify(data));

            })
            $(document).mouseup(function () {
                $(document).off('mousemove')
            })
        })
    },
    blazorMessage: (blazorObject, callback) => {
        window.external.receiveMessage(async (data) => {
            try {
                await blazorObject.invokeMethodAsync(callback, JSON.parse(data));
            }
            catch (error) {
                console.log(data + error);
            }
        });
    }
};