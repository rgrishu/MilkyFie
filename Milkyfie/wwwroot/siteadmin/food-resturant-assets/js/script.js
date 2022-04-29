
var swiper = new Swiper('.food-menu-slider', {
    slidesPerView: 3,
    spaceBetween: 30,
    loop: true,
    breakpoints: {
        1050: {
            slidesPerView: 2,
        },
        550: {
            slidesPerView: 1,
            spaceBetween: 10,
        }
    }
});


