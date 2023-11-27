// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function handleScroll(list, scrollLeftButton, scrollRightButton, cardWidth) {
    scrollRightButton.addEventListener('click', function () {
        list.scrollBy({
            top: 0,
            left: cardWidth,
            behavior: 'smooth'
        });
    });

    scrollLeftButton.addEventListener('click', function () {
        list.scrollBy({
            top: 0,
            left: -cardWidth,
            behavior: 'smooth'
        });
    });
}

const list = document.getElementById('list');
const scrollLeftButton = document.getElementById('scroll-left');
const scrollRightButton = document.getElementById('scroll-right');
const cardWidth = document.querySelector('.card').offsetWidth + 15; // considering the margin

handleScroll(dogList, scrollLeftButton, scrollRightButton, cardWidth);
