
const VerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

const Guid = {
    Empty: "00000000-0000-0000-0000-000000000000"
};

const EmptyMenu = [{
    Guid: Guid.Empty,
    Ordinal: 1,
    Description: "X",
    Sites: [{
        Guid: Guid.Empty,
        Ordinal: 1,
        Description: "X",
        Urls: [{
            Guid: Guid.Empty,
            Environment: 1,
            Link: "X",
        }]
    }]
}];