function showInfo(item) {
	vex.dialog.open({
		message: "sfsef",
		content: "f4f4",
		showCloseButton: true,
		buttons: []
	});
}

$(function loadActivators() {
    $('.activator.timeleft').BAToolTip({
        tipOpacity: 0.9,
        tipOffset: 20
    });
    $('.activator.usersgoing').BAToolTip({
        tipOpacity: 0.9,
        tipOffset: 20
    });
})