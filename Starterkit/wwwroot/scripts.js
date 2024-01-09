window.myCustomFunction = function () {
    // Your JavaScript code here
    alert('Hello from my custom function!');
};

window.metronicCloseModal = function (modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        $(modal).modal('hide'); // Assuming you are using jQuery in Metronic for modals
    }
};

window.metronicShowModal = function (modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        $(modal).modal('show'); // Assuming you are using jQuery in Metronic for modals
    }
};

function blazorGetCookie(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function blazorSetCookie(name, value) {
    document.cookie = `${name}=${value}; path=/;`;
}

function blazorRemoveCookie(name) {
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}