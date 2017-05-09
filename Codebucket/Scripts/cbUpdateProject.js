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
	codeHub.server.joinDocument(documentID); //this is most likely not right
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