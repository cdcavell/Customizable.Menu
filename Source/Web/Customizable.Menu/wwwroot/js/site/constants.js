
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
    },

    PropertyCount: function () {
        let count = 0;
        for (var property in this) {
            if (property != 'ByValue') {
                if (property != 'PropertyCount') {
                    count = count + 1;
                }
            }
        }

        return count;
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
    },

    PropertyCount: function () {
        let count = 0;
        for (var property in this) {
            if (property != 'ByValue') {
                if (property != 'PropertyCount') {
                    count = count + 1;
                }
            } 
        }

        return count;
    }
};
