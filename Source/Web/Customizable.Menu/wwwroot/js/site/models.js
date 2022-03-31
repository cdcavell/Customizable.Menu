
let VerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

let Guid = {
    Empty: "00000000-0000-0000-0000-000000000000"
};

let EmptyMenu = JSON.parse('[{"Menu": {"Guid":"' + Guid.Empty + '","Ordinal": 1,"Description": "Empty","Sites": [{"Site": {"Guid": "' + Guid.Empty + '","Ordinal": 1,"Description": "Empty","Urls": [{"Url": {"Guid": "' + Guid.Empty + '", "Environment": 1, "Link": "Empty"}}]}}]}}]');

    
