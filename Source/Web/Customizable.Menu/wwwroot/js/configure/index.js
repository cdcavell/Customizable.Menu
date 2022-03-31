$(document).ready(function () {

    let Configure = function () {

        Urls: {
            Index: {
                GetMenuList: "/Configure/GetMenuList"
            }
        }

        function Index()  {

            alert(Urls.Index.GetMenuList);
        }

    }

});
