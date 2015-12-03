$.noConflict();
jQuery(document).ready(function ($) {
    var startDateTextBox = $('#StartDate');
    var endDateTextBox = $('#EndDate');

    $.timepicker.datetimeRange(
        startDateTextBox,
        endDateTextBox,
        {
            timeFormat: 'hh:mm:ss TT',
            start: {
                currentText: 'Current'
            },
            end: {
                currentText: 'Current'
            }
        }
    );
});