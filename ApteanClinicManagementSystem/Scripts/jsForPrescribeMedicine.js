[].forEach.call(document.querySelectorAll('[type="number"]'), function (elem) {
    elem.disabled = true;
});
[].forEach.call(document.querySelectorAll('[type="checkbox"]'), function (elem) {
    elem.addEventListener('change', function () {
        this.parentNode.parentNode.querySelector('[type="number"]').disabled = !this.checked;
    });
})