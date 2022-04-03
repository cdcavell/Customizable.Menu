
const VerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

const Guid = {
    Empty: "00000000-0000-0000-0000-000000000000"
};

const EntityTypes = {
    ByValue: function (value) {
        for (var prop in this) {
            if (this.hasOwnProperty(prop)) {
                if (this[prop] === value)
                    return prop;
            }
        }
    }
};

const EnvironmentTypes = {
    ByValue: function (value) {
        for (var prop in this) {
            if (this.hasOwnProperty(prop)) {
                if (this[prop] === value)
                    return prop;
            }
        }
    }
};
