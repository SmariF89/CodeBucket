$(document).ready(function () {
    $(".movie-views .btn input:radio").change(function () {
        if ($(this).val() == 'poster') {
            $('.movies-list').fadeOut('fast', function () {
                $('.movies-poster').fadeIn('fast');
            });
        }
        else {
            $('.movies-poster').fadeOut('fast', function () {
                $('.movies-list').fadeIn('fast');
            });
        }
    });
});
