var documentID = $('#documentID').text();
$('#documentID').hide();

var editor = (function () {
	var aceEditor = ace.edit("editor");
	// default theme
	aceEditor.setTheme("ace/theme/monokai");
	// default mode
	var language = $('#language').text();
	$('#language').hide();
	aceEditor.getSession().setMode("ace/mode/" + language);
	aceEditor.setShowPrintMargin(false);
	return aceEditor;
})();

$('#theme').change(function () {
	var e = document.getElementById("theme");
	var strTheme = e.options[e.selectedIndex].value;
	editor.setTheme("ace/theme/" + strTheme);
});

$('#mode').change(function () {
	var e = document.getElementById("mode");
	var strMode = e.options[e.selectedIndex].value;
	editor.getSession().setMode("ace/mode/" + strMode);
});

var textarea = $('textarea[class="editor"]').hide()
editor.getSession().setValue(textarea.val());
editor.getSession().on('change', function () {
	textarea.val(editor.getSession().getValue());
});

var codeHub = $.connection.codeHub;
var silent = false;

codeHub.client.onChange = function (changeData) {
	console.log(changeData);
	silent = true;
	editor.getSession().getDocument().applyDelta(changeData);

	
	silent = false;
};

$.connection.hub.start().done(function () {
	codeHub.server.joinDocument(documentID);
	editor.on("change", function (obj) {
		if (silent) {
			return;
		}
		console.log(obj);
		codeHub.server.onChange(obj, documentID);
		
	});
});

$("#theme").submit(function (e) {
	e.preventDefault();
	var formData = $(this).serialize();
	var pUrl = "leaguestatus.php";

	submitFormSave(formData, pUrl);
});

// here the chat option comes in
    
$(function () {
	// Reference the auto-generated proxy for the hub.
	var chat = $.connection.chatHub;
	// Create a function that the hub can call back to display messages.
	chat.client.addNewMessageToPage = function (name, message) {
		// Add the message to the page.
		$('#discussion').append('<li> <strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
	};
	// Get the user name and store it to prepend to messages.
	// $('#displayname').val(prompt('Enter your name:', ''));
	// Set initial focus to message input box.
	$('#message').focus();
	// Start the connection.
	$.connection.hub.start().done(function () {
		$('#sendmessage').click(function () {
			// Call the Send method on the hub.
			chat.server.send($('#displayname').val(), $('#message').val());
			// Clear text box and reset focus for next comment.
			$('#message').val('').focus();
		});
	});
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).text();
	return encodedValue;
}