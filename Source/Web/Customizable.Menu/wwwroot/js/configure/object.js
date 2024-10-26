﻿let Configure = {

    // BEGIN - Index Function
    Index: function () {

        const Urls = {
            GetMenuList: "/Configure/GetMenuList",
            ItemAdd: "/Configure/ItemAdd",
            ItemAddSite: "/Configure/GetSite",
            ItemDelete: "/Configure/ItemDelete",
            ItemUpdate: "/Configure/ItemUpdate",
            ItemUp: "/Configure/ItemUp",
            ItemDown: "/Configure/ItemDown"
        };

        let OpenCard = {};

        let MaxMenuOrdinal = 0;

        $(function() {
            wait();

            let obj = JSON.parse(sessionStorage.getItem('ConfigureOpenCard'));
            if (obj != null) {
                console.debug('-- Loading OpenCard:');
                $.each(obj, function (key, value) {
                    $(OpenCard).prop(key, value);
                    console.debug('key: ' + key + ' value: ' + value);
                });
            }

            GetMenuList();
        });

        function ReloadDocument() {
            window.document.location.reload();
        };

        function ShowCards() {
            $.each(OpenCard, function (key, value) {
                $(value).slideToggle();
            });
        };

        function GetMenuList() {

            ajaxGet(Urls.GetMenuList)
                .then(function (data) {
                    console.debug('-- Loading MaxMenuOrdinal');
                    MaxMenuOrdinal = data.MaxMenuOrdinal;
                    console.debug('   MaxMenuOrdinal: ' + MaxMenuOrdinal);

                    console.debug('-- Loading EnityTypes');
                    $.each(data.EntityTypeList, function (key, value) {
                        EntityTypes[key] = value.Value;
                    });
                    console.debug('   Property Count: ' + EntityTypes.PropertyCount());
                    console.debug(EntityTypes);

                    console.debug('-- Loading EnvironmentTypes');
                    $.each(data.EnvironmentTypeList, function (key, value) {
                        EnvironmentTypes[key] = value.Value;
                    });
                    console.debug('   Property Count: ' + EnvironmentTypes.PropertyCount());
                    console.debug(EnvironmentTypes);

                    BuildSliderContainer(data.Menus);
                    ShowCards();

                })
                .catch((error) => {
                    ajaxError(error)
                });

            noWait();
        }

        function BuildSliderContainer(sliderItems) {
            console.debug('-- BuildSliderContainer');
            $('#sliderContainer').empty();

            let markup = '';

            if (sliderItems.length < MaxMenuOrdinal) {
                markup = '<div class="clearfix m-0 p-0">';
                markup += '<button type="button" class="item-add btn btn-secondary btn-sm border border-secondary mb-1 mr-1 px-1 py-0 float-right" data-entitytype="' + EntityTypes.ByValue('Menu') + '" data-guid="' + Guid.Empty + '"><i class="fas fa-plus"></i></button>';
                markup += '</div>';
                $(markup).appendTo('#sliderContainer');
            }

            $.each(sliderItems, function (menuKey, menuValue) {
                console.debug('-- Card: ' + menuKey + ' Data:');
                console.debug(menuValue);

                markup = '<div class="card">';

                // Card header
                markup += '<div class="item-slide card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '" data-slide="#collapse-' + menuKey + '">';
                markup += '<input class="form-control input-sm col-9 float-left" type="text" id="textbox-' + menuKey + '" name="textbox-' + menuKey + '" value="' + menuValue.Description.trim() + '">';
                markup += '<div class="float-right m-0 mt-2 p-0">';

                markup += '<i class="item-delete text-dark fas fa-trash mx-1 p-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Menu') + '"></i>';
                markup += '<i class="item-update text-dark fas fa-pen mx-1 p-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Menu') + '" data-textbox="#textbox-' + menuKey + '"></i>';
                markup += '<i class="item-up text-dark fas fa-arrow-up mx-1 p-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Menu') + '"></i>';
                markup += '<i class="item-down text-dark fas fa-arrow-down mx-1 p-1" type="button" data-guid="' + menuValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Menu') + '"></i>';

                markup += '</div > ';
                markup += '</div>';

                // Card body
                markup += '<div id="collapse-' + menuKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + menuKey + '">';
                markup += '<div class="card-body text-left">';

                markup += '<div class="clearfix m-0 p-0">';
                markup += '<button type="button" class="item-add btn btn-secondary btn-sm mb-1 mr-1 px-1 py-0 float-right" data-entitytype="' + EntityTypes.ByValue('Site') + '" data-guid="' + menuValue.Guid + '"><i class="fas fa-plus"></i></button>';
                markup += '</div>';

                $.each(menuValue.Sites, function (siteKey, siteValue) {

                    markup += '<div class="card">';
                    markup += '<div class="item-slide card-header px-2 py-1" type="button" role="tab" id="heading-' + menuKey + '-' + siteKey + '" data-slide="#collapse-' + menuKey + '-' + siteKey + '">';
                    markup += '<input class="form-control input-sm col-9 float-left" type="text" id="textbox-' + menuKey + '-' + siteKey + '" name="textbox-' + menuKey + '-' + siteKey + '" value="' + siteValue.Description.trim() + '">';
                    markup += '<div class="float-right m-0 mt-2 p-0">';

                    markup += '<i class="item-delete text-dark fas fa-trash mx-1 p-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Site') + '"></i>';
                    markup += '<i class="item-update text-dark fas fa-pen mx-1 p-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Site') + '" data-textbox="#textbox-' + menuKey + '-' + siteKey + '"></i>';
                    markup += '<i class="item-up text-dark fas fa-arrow-up mx-1 p-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Site') + '"></i>';
                    markup += '<i class="item-down text-dark fas fa-arrow-down mx-1 p-1" type="button" data-guid="' + siteValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Site') + '"></i>';

                    markup += '</div > ';
                    markup += '</div>';

                    // Card body
                    markup += '<div id="collapse-' + menuKey + '-' + siteKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + menuKey + '-' + siteKey + '">';
                    markup += '<div class="card-body text-left">';

                    if (siteValue.Urls.length < EnvironmentTypes.PropertyCount()) {
                        markup += '<div class="clearfix m-0 p-0">';
                        markup += '<button type="button" class="item-add btn btn-secondary btn-sm mb-1 mr-1 px-1 py-0 float-right" data-entitytype="' + EntityTypes.ByValue('Url') + '" data-guid="' + siteValue.Guid + '"><i class="fas fa-plus"></i></button>';
                        markup += '</div>';
                    }

                    $.each(siteValue.Urls, function (urlKey, urlValue) {

                        markup += '<div class="card">';
                        markup += '<div class="url card-header px-2 py-1" role="tab" id="heading-' + menuKey + '-' + siteKey + '-' + urlKey + '">';
                        markup += '<span class="col-1 col-sm-3 font-weight-bold text-right text-dark float-left mb-0 mt-2 mr-2">' + EnvironmentTypes[urlValue.Environment] + '</span>';
                        markup += '<input class="form-control input-sm col-11 col-sm-7 float-left" type="text" id="textbox-' + menuKey + '-' + siteKey + '-' + urlKey + '" name="textbox-' + menuKey + '-' + siteKey + '-' + urlKey + '" value="' + urlValue.Link.trim() + '">';

                        markup += '<div class="float-right m-0 mt-2 p-0">';
                        markup += '<i class="item-delete text-dark fas fa-trash mx-1 p-1" type="button" data-guid="' + urlValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Url') + '"></i>';
                        markup += '<i class="item-update text-dark fas fa-pen mx-1 p-1" type="button" data-guid="' + urlValue.Guid + '" data-entitytype="' + EntityTypes.ByValue('Url') + '" data-textbox="#textbox-' + menuKey + '-' + siteKey + '-' + urlKey + '" data-environmenttype="' + urlValue.Environment + '"></i>';
                        markup += '</div>';

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

            $('.input-sm').on('click', function () {
                return false;
            });

            // Slide Item
            $('.item-slide').on('click', function () {
                let value = $(this).data('slide').trim();
                let key = value.substring(1, value.length);

                if (OpenCard[key] == null) {
                    $(OpenCard).prop(key, value);
                    console.debug('-- OpenCard Added Key: ' + key + ' Value: ' + value);
                } else {
                    $(OpenCard).removeProp(key);
                    console.debug('-- OpenCard Removed Key: ' + key);
                }

                sessionStorage.removeItem('ConfigureOpenCard');
                sessionStorage.setItem('ConfigureOpenCard', JSON.stringify(OpenCard));
                $(value).slideToggle(1000);

                return false;
            });

            // Add Item
            $('.item-add').on('click', function (e) {
                e.preventDefault();

                $('#Guid').val($(this).data('guid'));
                $('#EntityType').val($(this).data('entitytype'));
                $('#addModalLabel').text('New ' + EntityTypes[$(this).data('entitytype')] + ' Item');
                $('#addDescriptionText').val('');

                if (EntityTypes[$(this).data('entitytype')] === 'Url') {
                    $('#addDescriptionLabel').text('Url Link');
                    $('#addEnvironmentSelect').empty();

                    let Model = {
                        Guid: $('#Guid').val(),
                        Site: {
                            Guid: Guid.Empty,
                            Ordinal: 0,
                            Description: 'X',
                            Menu: {
                                Guid: Guid.Empty,
                                Ordinal: 0,
                                Description: 'X',
                            }
                        }
                    };

                    ajaxPost(Urls.ItemAddSite, VerificationToken, Model)
                        .then(function (data) {
                            $.each(data.Site.AvailableEnvironments, function (key, value) {
                                let markup = '<option value="' + value.Key + '">' + value.Value + '</option>';
                                $(markup).appendTo('#addEnvironmentSelect');
                            });
                        })
                        .catch((error) => {
                            ajaxError(error);
                            $('#addModal').modal('hide');
                        });

                    $('#addEnvironmentDiv').show();
                } else {
                    $('#addDescriptionLabel').text('Description');
                    $('#addEnvironmentSelect').empty();
                    $('#addEnvironmentDiv').hide();
                }

                $('#addModal').modal('show');
            });

            $('.add-close').on('click', function () {
                $('#addModal').modal('hide');
                $('#hiddenGuid').val(null);
                $('#hiddenEntityType').val(null);
                $('#addDescriptionText').val(null);

                return false;
            });

            $('#addModal').on('shown.bs.modal', function () {
                $('#addDescriptionText').trigger('focus');
            });

            $('#add-save').on('click', function () {
                $('#addModal').modal('hide');
                wait('fast');

                let Model = {
                    Guid: $('#Guid').val(),
                    EntityType: $('#EntityType').val(),
                    Description: $('#addDescriptionText').val(),
                    EnvironmentType: $('#addEnvironmentSelect').find(":selected").val()
                };

                $('#Guid').val(null);
                $('#EntityType').val(null);
                $('#addDescriptionText').val(null);

                ajaxPost(Urls.ItemAdd, VerificationToken, Model)
                    .then(function (data) {
                        ReloadDocument();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        ReloadDocument();
                    });

                return false;
            });

            // Delete Item
            $('.item-delete').on('click', function () {
                wait('fast');
                let confirmMessage = 'Are your sure you want to continue? All defined links under this item will be deleted as well.';

                if (confirm(confirmMessage)) {
                    let Model = {
                        Guid: $(this).data('guid'),
                        EntityType: $(this).data('entitytype')
                    };

                    ajaxPost(Urls.ItemDelete, VerificationToken, Model)
                        .then(function (data) {
                            ReloadDocument();
                        })
                        .catch((error) => {
                            ajaxError(error);
                            ReloadDocument();
                        });
                } else {
                    noWait();
                }

                return false;
            });

            // Update Item
            $('.item-update').on('click', function () {
                wait('fast');
                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: $(this).data('entitytype'),
                    Description: $($(this).data('textbox')).val(),
                    EnvironmentType: $(this).data('environmenttype')
                };

                ajaxPost(Urls.ItemUpdate, VerificationToken, Model)
                    .then(function (data) {
                        ReloadDocument();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        ReloadDocument();
                    });

                return false;
            });

            // Move Item Up
            $('.item-up').on('click', function () {
                wait('fast');
                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: $(this).data('entitytype')
                };

                ajaxPost(Urls.ItemUp, VerificationToken, Model)
                    .then(function (data) {
                        ReloadDocument();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        ReloadDocument();
                    });

                return false;
            });

            // Move Item Down
            $('.item-down').on('click', function () {
                wait('fast');
                let Model = {
                    Guid: $(this).data('guid'),
                    EntityType: $(this).data('entitytype')
                };

                ajaxPost(Urls.ItemDown, VerificationToken, Model)
                    .then(function (data) {
                        ReloadDocument();
                    })
                    .catch((error) => {
                        ajaxError(error);
                        ReloadDocument();
                    });

                return false;
            });

        }

    }
    // END - Index Function

};
