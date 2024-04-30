// Scrolls to an element with a matching ID.
// <!-- Author: Jimmie -->
// <!-- Co Authors: -->
function scrollToElement(elementId) {

    var element = document.getElementById(elementId);

    if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
    }
}