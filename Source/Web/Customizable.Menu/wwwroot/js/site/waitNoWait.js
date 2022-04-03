function wait(speed) {

    if (speed === 'fast') {
        $('.preloader').fadeIn(speed);
    } else {
        $('.preloader').delay(500).fadeIn('slow');
    }

    $('.preloader-icon').fadeIn(400);
}

function noWait() {
    $('.preloader-icon').fadeOut(400);
    $('.preloader').delay(500).fadeOut('slow');
}
