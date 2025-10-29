window.chatHelpers = {
    scrollToBottom: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    },

    scrollToBottomSmooth: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.scrollTo({
                top: element.scrollHeight,
                behavior: 'smooth'
            });
        }
    },

    focusElement: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.focus();
        }
    }
};