function showInfo(item) {
	vex.dialog.open({
		message: "id: " + item.ID + "noe annet: " + item.EventName,
		showCloseButton: true,
		buttons: []
	});
}