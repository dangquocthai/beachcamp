
//open popup modal for Calendar Overlays
// load our function to the delayed load list
_spBodyOnLoadFunctionNames.push('calendarEventLinkIntercept');

// hook into the existing SharePoint calendar load function.
function calendarEventLinkIntercept() {
    if (SP.UI.ApplicationPages.CalendarNotify.$4a) {
        var OldCalendarNotify = SP.UI.ApplicationPages.CalendarNotify.$4a;
        SP.UI.ApplicationPages.CalendarNotify.$4a = function () {
            OldCalendarNotify();
            bindEventClickHandler();
        }
    }
    if (SP.UI.ApplicationPages.CalendarNotify.$4b) {
        var OldCalendarNotify = SP.UI.ApplicationPages.CalendarNotify.$4b;
        SP.UI.ApplicationPages.CalendarNotify.$4b = function () {
            OldCalendarNotify();
            bindEventClickHandler();

        }
    }
    // future service pack change may go here!
    // if (SP.UI.ApplicationPages.CalendarNotify.???)
}

function bindEventClickHandler() {

    //Expand calendar item
    expandAllCalendarItem();
    /*
    $(".ms-acal-ctrlitem").each(function () {
        expandAllCalendarItem();
    });*/

    //$(".ms-acal-rootdiv .ms-acal-title a[href*='/Lists/'], .ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").click(function () { EditLink2(this, 'WPQ1'); return false; });
    //$(".ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").click(function () { EditLink2(this, 'WPQ2'); return false; });
    $(".ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").each(function () {
        $(this).attr("onclick", "EditLink2(this, 'WPQ1'); return false;");
    });

    /*
    $(".ms-acal-sdiv").each(function () {
    var objtime = $(this).children(".ms-acal-time");
    var time = objtime.text();
    objtime.hide();
    var objParent = $(this).parent(".ms-acal-item");
    objParent.height("16px");
    objParent.css('text-align', 'center');
    objParent.css('vertical-align', 'middle');
    var ogrinalTitle = objParent.attr("title");
    var titleValue = ogrinalTitle.replace(time, "");
    objParent.attr("title", titleValue);
    var startString = titleValue.indexOf("(");
    var endString = titleValue.lastIndexOf(")");
    var titleText = titleValue.substring(startString, endString + 1);
    $(this).find("a").html(titleText);
    $(this).find("a").addClass("word_wrap");
    });*/

    $(".ms-acal-sdiv").each(function () {
        var objtime = $(this).children(".ms-acal-time");
        objtime.hide();
        var objParent = $(this).parent(".ms-acal-item");
        objParent.height("16px");
        objParent.css('text-align', 'center');
        var ogrinalTitle = objParent.attr("title");
        var startString = ogrinalTitle.indexOf("(");
        var endString = ogrinalTitle.lastIndexOf(")");
        var titleText = ogrinalTitle.substring(startString, endString + 1);

        var objatag = $(this).find("a");
        objatag.html(titleText);
        objParent.attr("title", titleText);
        objatag.addClass("word_wrap");
        $(this).empty();
        $(this).append(objatag);
    });

    $(".ms-acal-mdiv").each(function () {
        var objatag = $(this).find("a");
        var titleValue = $(this).text().replace(objatag.html(), "");
        //alert(titleValue);
        objatag.html(titleValue);
        //alert(objatag.html());
        $(this).empty();
        $(this).append(objatag);
    });
}

//Expand all function
function expandAllCalendarItem() {
    var ctrl = SP.UI.ApplicationPages.CalendarInstanceRepository.firstInstance();
    if (ctrl) {
        ctrl.expandAll();
    }
}

function expandCalendar() {
    try {
        var aTags = document.getElementsByTagName('A');
        for (i = 0; i < aTags.length; i++) {

            if ((aTags[i].evtid == "expand_collapse") && (aTags[i].innerText != "collapse")) {
                (aTags[i]).click();
            }
        }
    }
    catch (err) {
        alert(err.message);
    }
}
