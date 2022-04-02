let Home = {

    // BEGIN - Index Function
    Index: function () {

        const Urls = {
            GetSiteList: "/Home/GetSiteList"
        }

        const EnvironmentTypes = {};

        $(document).ready(async function () {

            let Model = {
                MenuGuid: $('#MenuGuid').val()
            }

            await ajaxPost(Urls.GetSiteList, VerificationToken, Model)
                .then(function (data) {

                    console.debug('-- Loading EnvironmentTypes');
                    $.each(data.EnvironmentTypes, function (key, value) {
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

        });

        function BuildSliderContainer(sliderItems) {

            console.debug('-- BuildSliderContainer');

            $.each(sliderItems, function (key, value) {
                console.debug('-- Card: ' + key + ' Data:');
                console.debug(value);

                let markup = '<div class="card">';

                // Card header
                markup += '<div class="main card-header px-2 py-1" type="button" role="tab" id="heading-' + key + '">';
                markup += '<h5 class="text-left text-dark mb-0">' + value.Description.trim() + '</h5>';
                markup += '</div>';

                // Card body
                markup += '<div id="collapse-' + key + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + key + '">';
                markup += '<div class="card-body text-left">';

                $.each(value.Urls, function (subKey, subValue) {

                    markup += '<div class="card">';
                    markup += '<div class="sub card-header text-center text-dark px-2 py-1" type="button" role="tab" id="heading-' + key + '-' + subKey + '"  data-href="' + subValue.Link + '">';
                    markup += '<h7 class="mb-0">' + EnvironmentTypes[subValue.Environment] + '</h7>';
                    markup += '</div>';
                    markup += '</div>';

                });

                markup += '</div>';
                markup += '</div>';

                markup += '</div>';

                $(markup).appendTo('#sliderContainer');
            });

            $(".main").click(function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
            });

            $(".sub").click(function () {
                let result = $(this).data("href");
                window.location = result;
            });
        }

    }
    // END - Index Function

};
