let VerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

let Menu = {
    Guid: "",
    Ordinal: 0,
    Description: "",
    Sites: [Site]
};

let Site = {
    Guid: "",
    Ordinal: 0,
    Description: "",
    Urls: [Url]
};

let Url = {
    Guid: "",
    Environment: 0,
    Link: ""
};
