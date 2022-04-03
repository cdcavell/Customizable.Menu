let Configure = {

    // BEGIN - Index Function
    Index: function () {

        const Urls = {
            GetMenuList: "/Configure/GetMenuList",
            ItemDelete: "/Configure/ItemDelete",
            ItemUpdate: "/Configure/ItemUpdate",
            ItemUp: "/Config/ItemUp",
            ItemDown: "/Config/ItemDown"
        };

        $(document).ready(function () {
            GetMenuList();
        });

        function GetMenuList() {

            ajaxGet(Urls.GetMenuList)
                .then(function (data) {

                    console.debug('-- Loading EnityTypes');
                    $.each(data.EntityTypeList, function (key, value) {
                        EntityTypes[key] = value.Value;
                    });
                    console.debug(EntityTypes);

                    console.debug('-- Loading EnvironmentTypes');
                    $.each(data.EnvironmentTypeList, function (key, value) {
                        EnvironmentTypes[key] = value.Value;
                    });
                    console.debug(EnvironmentTypes);

                    BuildSliderContainer(data.Menus);
                    noWait();

                })
                .catch((error) => {
                    ajaxError(error)
                    noWait();
                });

        }

        function BuildSliderContainer(sliderItems) {

            console.debug('-- BuildSliderContainer');
            $('#sliderContainer').empty();

            $.each(sliderItems, function (menuKey, menuValue) {
                console.debug('-- Card: ' + menuKey + ' Data:');
                console.debug(menuValue);

                let markup = '<div class="card">';

                // Card header
                markup += '<div class="item-slide card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '" data-slide="#collapse-' + menuKey + '">';
                markup += '<input class="form-control input-sm col-9 float-left" type="text" id="textbox-' + menuKey + '" name="textbox-' + menuKey + '" value="' + menuValue.Description.trim() + '">';
                markup += '<div class="float-right m-0 mt-2 p-0">';

                markup += '<i class="item-delete text-dark fas fa-trash mx-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="Menu"></i>';
                markup += '<i class="item-update text-dark fas fa-pen mx-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="Menu" data-textbox="#textbox-' + menuKey + '"></i>';
                markup += '<i class="item-up text-dark fas fa-arrow-up mx-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="Menu"></i>';
                markup += '<i class="item-down text-dark fas fa-arrow-down mx-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="Menu"></i>';

                markup += '</div > ';
                markup += '</div>';

                // Card body
                markup += '<div id="collapse-' + menuKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + menuKey + '">';
                markup += '<div class="card-body text-left">';

                $.each(menuValue.Sites, function (siteKey, siteValue) {

                    markup += '<div class="card">';
                    markup += '<div class="item-slide card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '-' + siteKey + '" data-slide="#collapse-' + menuKey + '-' + siteKey + '">';
                    markup += '<input class="form-control input-sm col-9 float-left" type="text" id="textbox-' + menuKey + '-' + siteKey + '" name="textbox-' + menuKey + '-' + siteKey + '" value="' + siteValue.Description.trim() + '">';
                    markup += '<div class="float-right m-0 p-0">';

                    markup += '<i class="item-delete text-dark fas fa-trash mx-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="Site"></i>';
                    markup += '<i class="item-update text-dark fas fa-pen mx-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="Site" data-textbox="#textbox-' + menuKey + '-' + siteKey + '"></i>';
                    markup += '<i class="item-up text-dark fas fa-arrow-up mx-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="Site"></i>';
                    markup += '<i class="item-down text-dark fas fa-arrow-down mx-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="Site"></i>';

                    markup += '</div > ';
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

            $('.input-sm').click(function () {
                return false;
            });

            // Slide Item
            $('.item-slide').click(function () {
                $($(this).data('slide')).slideToggle(1000);
                return false;
            });

            // Delete Item
            $('.item-delete').click(function () {
                let confirmMessage = 'Are your sure you want to continue? All defined links under this item will be deleted as well.';

                if (confirm(confirmMessage)) {
                    wait();

                    let Model = {
                        Guid: $(this).data('guid'),
                        EntityType: EntityTypes.ByValue($(this).data('entitytype'))
                    };

                    ajaxPost(Urls.ItemDelete, VerificationToken, Model)
                        .then(function (data) {
                            GetMenuList();
                        })
                        .catch((error) => {
                            ajaxError(error);
                            GetMenuList();
                        });
                }

                return false;
            });

            // Update Item
            $('.item-update').click(function () {
                wait();

                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: EntityTypes.ByValue($(this).data('entitytype')),
                    Description: $($(this).data('textbox')).val()
                };
                
                ajaxPost(Urls.ItemUpdate, VerificationToken, Model)
                    .then(function (data) {
                        GetMenuList();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        GetMenuList();
                    });

                return false;
            });

            // Move Item Up
            $('.item-up').click(function () {
                wait();

                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: EntityTypes.ByValue($(this).data('entitytype')),
                };

                ajaxPost(Urls.ItemUp, VerificationToken, Model)
                    .then(function (data) {
                        GetMenuList();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        GetMenuList();
                    });

                return false;
            });

            // Move Item Down
            $('.item-down').click(function () {
                wait();

                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: EntityTypes.ByValue($(this).data('entitytype')),
                };

                ajaxPost(Urls.ItemDown, VerificationToken, Model)
                    .then(function (data) {
                        GetMenuList();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        GetMenuList();
                    });

                return false;
            });

        }

    }
    // END - Index Function

};
