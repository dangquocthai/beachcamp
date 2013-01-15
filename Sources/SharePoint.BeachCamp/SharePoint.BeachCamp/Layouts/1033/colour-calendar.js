
_spBodyOnLoadFunctionNames.push('WaitForCalendarToLoad');
var SEPARATOR = "|||"; 
function WaitForCalendarToLoad() {        
	if(typeof SP.UI.ApplicationPages.CalendarNotify.$4a == 'undefined') { 
		// post SP1
		var pwold$4b = SP.UI.ApplicationPages.CalendarNotify.$4b;
		SP.UI.ApplicationPages.CalendarNotify.$4b = function () {
			pwold$4b();
			ColourCalendar();
		}            
		SP.UI.ApplicationPages.SummaryCalendarView.prototype.renderGrids = function($p0) {
			var $v_0 = new Sys.StringBuilder();
			var $v_1 = $p0.length;
			for (var $v_2 = 0; $v_2 < $v_1; $v_2++) {
				this.$7t_2($v_2, $p0[$v_2]);                  
			}                  
			for (var $v_3 = 0; $v_3 < $v_1; $v_3++) {
				$v_0.append('<div>');
				this.$I_2.$7o($v_0, $p0[$v_3], $v_3);
				$v_0.append(this.emptY_DIV);
				$v_0.append('</div>');
			}
			this.setInnerHtml($v_0.toString());
			ColourCalendar();
		}        
	}       
	else
	{ 
		// pre SP1
		var pwold$4a = SP.UI.ApplicationPages.CalendarNotify.$4a;
		SP.UI.ApplicationPages.CalendarNotify.$4a = function () {
			pwold$4a();
			ColourCalendar();
		}             
		SP.UI.ApplicationPages.SummaryCalendarView.prototype.renderGrids = function($p0) {
			var $v_0 = new Sys.StringBuilder();
			var $v_1 = $p0.length;
			for (var $v_2 = 0; $v_2 < $v_1; $v_2++) {
				this.$7r_2($v_2, $p0[$v_2]);                  
			}                  
			for (var $v_3 = 0; $v_3 < $v_1; $v_3++) {
				$v_0.append('<div>');                       
				this.$I_2.$7m($v_0, $p0[$v_3], $v_3);
				$v_0.append(this.emptY_DIV);
				$v_0.append('</div>');
			}
			this.setInnerHtml($v_0.toString());
			ColourCalendar();
		}
	}  
} 

function ColourCalendar() {
        if(jQuery('a:contains(' + SEPARATOR + ')') != null)
        {             
		jQuery('a:contains(' + SEPARATOR + ')').each(function (i) {
			$box = jQuery(this).parents('div[title]');
			var colour = GetColourCodeFromCategory(GetCategory(this.innerHTML));
			this.innerHTML = GetActualText(this.innerHTML);
			jQuery($box).attr("title", GetActualText(jQuery($box).attr("title")));
			$box.css('background-color', colour);
			});        
	}   
}   

function GetActualText(originalText) {     
	var parts = originalText.split(SEPARATOR);
	return parts[0] + parts[2];   
}

function GetCategory(originalText) {
	var parts = originalText.split(SEPARATOR);
	return parts[1];   
}

function GetColourCodeFromCategory(category) {
	var colour = null;     
	switch (category.trim().toLowerCase()) {
		case 'draft':         
			colour = "#F08616";
			break;
        case 'pending':
            colour = "#FFFF00";//"#E0F558";  
            break;
        case 'approved':
            colour = "#00FF00"; //'#4FDB51';
            break;
        case 'rejected':
            colour = "#FF0000"; //"#6E80FA";         
            break;			
		case 'work hours':         
			colour = '#4FB8DB';
			break;       
		case 'holiday':         
			colour = "#F55875";         
			break;       		       
		case 'gifts':         
			colour = "#F558D5";         
			break;       
		case 'anniversary':         
			colour = "#FF4040";         
			break;     
	}     
	return colour;   
}
		
		