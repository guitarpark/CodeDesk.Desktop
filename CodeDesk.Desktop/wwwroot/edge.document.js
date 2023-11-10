window.external = {
    sendMessage: message => {
        window.chrome.webview.postMessage(message);
    },
    receiveMessage: callback => {
        window.chrome.webview.addEventListener('message', e => callback(e.data));
    },
    focus: element => {
        element.focus();
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


