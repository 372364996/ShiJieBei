function isPc() {
	if(/Android|webOS|SymbianOS|iPhone|Windows Phone|iPod|BlackBerry/i.test(navigator.userAgent)) {
		return false;
	} else {
		return true;
	}
}
$(function() {
	if(isPc()) {
        $('body').attr("style", "max-width: 50%;");   
	}
})
