
let VerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

let Guid = {
    Empty: "00000000-0000-0000-0000-000000000000"
};

//let Url = {
//    Guid: Guid.Empty,
//    Environment: 1,
//    Link: "Empty"
//};

//let Site = {
//    Guid: Guid.Empty,
//    Ordinal: 1,
//    Description: "Empty",
//    Urls: [Url]
//};

//let Menu = {
//    Guid: Guid.Empty,
//    Ordinal: 1,
//    Description: "Empty",
//    Sites: [Site]
//};

//let EmptyMenu = Menu;
//EmptyMenu.Sites = push(Site);
//EmptyMenu.Sites[0].Urls = push(Url);

let EmptyMenu = JSON.parse('[{"Menu": {"Guid":"' + Guid.Empty + '","Ordinal": 1,"Description": "Empty","Sites": [{"Site": {"Guid": "' + Guid.Empty + '","Ordinal": 1,"Description": "Empty","Urls": [{"Url": {"Guid": "' + Guid.Empty + '", "Environment": 1, "Link": "Empty"}}]}}]}}]');

    
