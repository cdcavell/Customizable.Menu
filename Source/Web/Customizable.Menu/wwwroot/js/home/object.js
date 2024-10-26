let Home = {

    // BEGIN - Index Function
    Index: function () {

        const Urls = {
            GetSiteList: "/Home/GetSiteList"
        }

        $(function() {
            GetSiteList();
        });

        function GetSiteList() {
            let Model = {
                Guid: $('#MenuGuid').val()
            }

            ajaxPost(Urls.GetSiteList, VerificationToken, Model)
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

                    if (data.Menus.length > 0) {
                        BuildSliderContainer(data.Menus[0].Sites);
                    }

                })
                .catch((error) => {
                    ajaxError(error)
                });

            noWait();
        }

        function BuildSliderContainer(sliderItems) {

            console.debug('-- BuildSliderContainer');

            $.each(sliderItems, function (siteKey, siteValue) {
                console.debug('-- Card: ' + siteKey + ' Data:');
                console.debug(siteValue);

                let markup = '<div class="card">';

                // Card header
                markup += '<div class="site-div card-header px-2 py-1" type="button" role="tab" id="heading-' + siteKey + '">';
                markup += '<h5 class="text-left text-dark mb-0">' + siteValue.Description.trim() + '</h5>';
                markup += '</div>';

                // Card body
                markup += '<div id="collapse-' + siteKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + siteKey + '">';
                markup += '<div class="card-body text-left">';

                $.each(siteValue.Urls, function (urlKey, urlValue) {

                    markup += '<div class="card">';
                    markup += '<div class="url-div card-header text-center text-dark px-2 py-1" type="button" role="tab" id="heading-' + siteKey + '-' + urlKey + '"  data-href="' + urlValue.Link + '">';
                    markup += '<h7 class="font-weight-bold mb-0">' + EnvironmentTypes[urlValue.Environment] + '</h7>';
                    markup += '</div>';
                    markup += '</div>';

                });

                markup += '</div>';
                markup += '</div>';

                markup += '</div>';

                $(markup).appendTo('#sliderContainer');
            });

            $(".site-div").on('click', function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
                return false;
            });

            $(".url-div").on('click', function () {
                let result = $(this).data("href");
                window.location = result;
            });
        }

    }
    // END - Index Function

};
