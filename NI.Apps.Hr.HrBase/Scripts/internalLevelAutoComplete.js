var levels = new Array();

$(function () {
    $.getJSON(getInternalLevelURL, function (data) {
        $.each(data, function (i, item) {
            levels.push(item.InternalLevel_Name);
        });
    });

    $("input#InternalLevel").autocomplete({
        source: levels,
        minLength: 0,

        change: function (event, ui) {
            if (ui.item) {

            } else {
                alert("The InternalLevel doesn't exist!");
            }
        }
    });

    $('input#InternalLevel').click(function () { $(this).autocomplete("search", ""); });
});