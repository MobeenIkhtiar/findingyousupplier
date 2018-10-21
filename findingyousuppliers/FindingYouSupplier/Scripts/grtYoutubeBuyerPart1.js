$(document).ready(function () {
    var getvideoid = "0sMGPm2qrfc";

    // Default options
    var settings = $.extend({
        videoID: getvideoid,
        autoPlay: true,
        theme: "dark"
    });

    // Convert some values
    settings.autoPlay = 1;
    settings.theme = "grtyoutube-light-theme";

    // Initialize on click
    if (getvideoid) {
        //$(this).on("click", function () {
        $("body").append('<div class="grtyoutube-popup ' + settings.theme + '">' +
            '<div class="grtyoutube-popup-content">' +
            '<span class="grtyoutube-popup-close"></span>' +
            '<iframe class="grtyoutube-iframe"  src="https://www.youtube.com/embed/' + settings.videoID + '?rel=0&amp;showinfo=0&wmode=transparent&autoplay=' + settings.autoPlay + '&iv_load_policy=3" allowfullscreen frameborder="0"></iframe>' +
             //<iframe class="grtyoutube-iframe" src= "https://www.youtube-nocookie.com/embed/0sMGPm2qrfc?rel=0&amp;showinfo=0" + 'frameborder= "0"'+ ' allow= "autoplay; encrypted-media" allowfullscreen'></iframe >
            
            '</div>' +
            '</div>');
        //});
    }

    // Close the box on click or escape
    $(this).on('click', function (event) {
       // event.preventDefault();
        $(".grtyoutube-popup-close, .grtyoutube-popup").click(function () {
            $(".grtyoutube-popup").remove();
        });
    });

    $(document).keyup(function (event) {
        if (event.keyCode == 27) {
            $(".grtyoutube-popup").remove();
        }
    });
});
