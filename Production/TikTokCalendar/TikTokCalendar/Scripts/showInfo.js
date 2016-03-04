function showInfo() {
	var id = $(this).data('assigned-id');

	vex.dialog.open({
		message: id,
		showCloseButton: true,
		buttons: []
	});
}