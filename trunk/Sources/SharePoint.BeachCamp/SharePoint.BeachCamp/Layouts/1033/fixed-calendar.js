
	//open popup modal for Calendar Overlays
	// load our function to the delayed load list
	_spBodyOnLoadFunctionNames.push('calendarEventLinkIntercept');

	// hook into the existing SharePoint calendar load function.
	function calendarEventLinkIntercept()
	{
		if (SP.UI.ApplicationPages.CalendarNotify.$4a)
	  		{
	    		var OldCalendarNotify = SP.UI.ApplicationPages.CalendarNotify.$4a;
	    		SP.UI.ApplicationPages.CalendarNotify.$4a = function () 
	      		{
	        		OldCalendarNotify();
	        		bindEventClickHandler();
	      		}
	  		}
	  	if (SP.UI.ApplicationPages.CalendarNotify.$4b)
	  		{
	    		var OldCalendarNotify = SP.UI.ApplicationPages.CalendarNotify.$4b;
	    		SP.UI.ApplicationPages.CalendarNotify.$4b =  function () 
	      		{
	        		OldCalendarNotify();
	        		bindEventClickHandler();

	      		}  
	  	}
	  // future service pack change may go here!
	  // if (SP.UI.ApplicationPages.CalendarNotify.???)
	}

	function bindEventClickHandler() {

	    $(".ms-acal-sdiv").each(function () {
	        var time = $(this).children(".ms-acal-time").text();
	        $(this).children(".ms-acal-time").hide();
	        var titleText = $(this).parent(".ms-acal-item").attr("title");
	        var titleValue = titleText.replace(time, "");
	        $(this).parent(".ms-acal-item").attr("title", titleValue);
	        $(this).find("a").html(titleValue);
	    });
        
	    //$(".ms-acal-rootdiv .ms-acal-title a[href*='/Lists/'], .ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").click(function () { EditLink2(this, 'WPQ1'); return false; });
	    //$(".ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").click(function () { EditLink2(this, 'WPQ2'); return false; });
	    $(".ms-acal-rootdiv .ms-acal-mdiv a[href*='/Lists/']").each(function () {
	        $(this).attr("onclick", "EditLink2(this, 'WPQ1'); return false;");
	    });
	}