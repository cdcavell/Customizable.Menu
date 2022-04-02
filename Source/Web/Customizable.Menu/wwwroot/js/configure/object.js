let Configure = {

    // BEGIN - Index Function
    Index: function () {

        const Urls = {
            GetMenuList: "/Configure/GetMenuList",
            DeleteMenuItem: "/Configure/DeleteMenuItem"
        }

        $(document).ready(async function () {

            let Model = {
                Menus: JSON.stringify(EmptyMenu)
            }

            await ajaxPost(Urls.GetMenuList, VerificationToken, Model)
                .then(function (data) {

                    BuildSliderContainer(data.Menus);
                    noWait();

                })
                .catch((error) => {
                    ajaxError(error)
                    noWait();
                });

        });

        function BuildSliderContainer(sliderItems) {

            console.debug('-- BuildSliderContainer');

            $.each(sliderItems, function (menuKey, menuValue) {
                console.debug('-- Card: ' + menuKey + ' Data:');
                console.debug(menuValue);

                let markup = '<div class="card">';

                // Card header
                markup += '<div class="menu-div card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '">';
                markup += '<h5 class="text-left text-dark mb-0">' + menuValue.Description.trim();
                markup += '<div class="float-right m-0 p-0">';

                markup += '<i class="menu-delete fas fa-trash mx-1" type="button" data-guid="' + menuValue.Guid + '"></i>';
                markup += '<i class="menu-edit fas fa-pen mx-1" type="button" data-guid="' + menuValue.Guid + '"></i>';
                markup += '<i class="menu-up fas fa-arrow-up mx-1" type="button" data-guid="' + menuValue.Guid + '"></i>';
                markup += '<i class="menu-down fas fa-arrow-down mx-1" type="button" data-guid="' + menuValue.Guid + '"></i>';

                markup += '</div > ';
                markup += '</h5>';
                markup += '</div>';

                // Card body
                markup += '<div id="collapse-' + menuKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + menuKey + '">';
                markup += '<div class="card-body text-left">';

                $.each(menuValue.Sites, function (siteKey, siteValue) {

                    markup += '<div class="card">';
                    markup += '<div class="site-div card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '-' + siteKey + '">';
                    markup += '<h7 class="text-left text-dark mb-0">' + siteValue.Description.trim() + '</h7>';
                    markup += '</div>';

                    // Card body
                    markup += '<div id="collapse-' + menuKey + '-' + siteKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + menuKey + '-' + siteKey + '">';
                    markup += '<div class="card-body text-left">';

                    $.each(siteValue.Urls, function (urlKey, urlValue) {

                        markup += '<div class="card">';
                        markup += '<div class="url card-header px-2 py-1" role="tab" id="heading-' + menuKey + '-' + siteKey + '-' + urlKey + '">';
                        markup += '<h7 class="text-left text-dark mb-0">' + urlValue.Link.trim() + '</h7>';
                        markup += '</div>';
                        markup += '</div>';

                    });

                    markup += '</div>';
                    markup += '</div>';

                });

                markup += '</div>';
                markup += '</div>';

                markup += '</div>';

                $(markup).appendTo('#sliderContainer');
            });

            // Show Sites
            $(".menu-div").click(function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
                return false;
            });

            // Delete Menu Item
            $(".menu-delete").click(function () {
                let confirmMessage = "Are your sure you want to continue? All defined links under menu item will be deleted as well.";

                if (confirm(confirmMessage)) {
                    wait();

                    let Model = {
                        MenuGuid: $(this).data("guid")
                    }

                    ajaxPost(Urls.DeleteMenuItem, VerificationToken, Model)
                        .then(function (data) {
                        })
                        .catch((error) => {
                            ajaxError(error)
                        });

                    location.reload();
                }

                return false;
            });

            // Edit Menu Item
            $(".menu-edit").click(function () {
                let result = $(this).data("guid");
                alert(result);
                return false;
            });

            $(".menu-up").click(function () {
                let result = $(this).data("guid");
                alert(result);
                return false;
            });

            $(".menu-down").click(function () {
                let result = $(this).data("guid");
                alert(result);
                return false;
            });

            $(".site-div").click(function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
                return false;
            });
        }

    }
    // END - Index Function

};
