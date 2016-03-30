var emails = new Array();
var employees = new Array();

$(function () {
    //get employeeds' name and emails
    //var url = "/Offer/GetEmployeesEmails/";

    //var url = '@Url.Action("GetEmployeesEmails", "Offer")';
    //window.location.href = url.replace('__param__', encodeURIComponent(param));
    //var url = @Url.Action("GetEmployeesEmails","Offer");
    $.getJSON(getEmployeeEmailURL, function (data) {
        $.each(data, function (i, item) {
            employees.push(item.Employee_FullName);
            emails.push(item.Employee_Email);
        });
    });

    $("input#Report_ReportLine").autocomplete({
        source: employees,
        minLength: 0,

        select: function (event, ui) {
            var index = $.inArray(ui.item.value, employees);
            $("input#Report_ReportLineEmail").val(emails[index]);
        },
        change: function (event, ui) {
            if (ui.item) {

            } else {
                alert("The employee doesn't exist!");
            }
        }
    });

    $('input#Report_ReportLine').click(function () { $(this).autocomplete("search", ""); });


    $("input#Report_DepartmentMgr").autocomplete({
        source: employees,
        minLength: 0,

        select: function (event, ui) {
            var index = $.inArray(ui.item.value, employees);
            $("input#Report_DepartmentMgrEmail").val(emails[index]);
        },
        change: function (event, ui) {
            if (ui.item) {

            } else {
                alert("The employee doesn't exist!");
            }
        }
    });

    $('input#Report_DepartmentMgr').click(function () { $(this).autocomplete("search", ""); });
});
