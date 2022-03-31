   
let Configure = {

    Index: function () {

        const Urls = {
            GetMenuList: "/Configure/GetMenuList"
        }

        $(document).ready(async function () {

            let Model = {
                Menus: JSON.stringify(EmptyMenu)
            }

            await ajaxPost(Urls.GetMenuList, VerificationToken, Model)
                .then(function (data) {
                    console.debug(data);
                })
                .catch((error) => {
                    ajaxError(error)
                });

        });

    }

};