var departments = new Array();

$(function () {
    //get employeeds' name and emails
    //var url = "/Offer/GetEmployeesEmails/";

    //var url = '@Url.Action("GetDepartments", "Employee")';
    //window.location.href = url.replace('__param__', encodeURIComponent(param));
    //var url = @Url.Action("GetEmployeesEmails","Offer");
    $.getJSON(getDepartmentURL, function (data) {
        $.each(data, function (i, item) {
            departments.push(item.Department_Name);
        });
    });

    $("input#Department").autocomplete({
        source: departments,
        minLength: 0,
       
        change: function (event, ui) {
            if (ui.item) {

            } else {
                alert("The Department doesn't exist!");
            }
        }
    });

    $('input#Department').click(function () { $(this).autocomplete("search", ""); });
});
