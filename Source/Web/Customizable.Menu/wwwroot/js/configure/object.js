let Configure = {

    // BEGIN - Index Function
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

                    BuildSliderContainer(data.Menus);

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

                $.each(value.Sites, function (subKey, subValue) {

                    markup += '<div class="card">';
                    markup += '<div class="sub card-header px-2 py-1" type="button" role="tab" id="heading-' + key + '-' + subKey + '">';
                    markup += '<h7 class="text-left text-dark mb-0">' + subValue.Description.trim() + '</h7>';
                    markup += '</div>';

                    // Card body
                    markup += '<div id="collapse-' + key + '-' + subKey + '" class="collapse" role="tabpanel" aria-labelledby="heading-' + key + '-' + subKey + '">';
                    markup += '<div class="card-body text-left">';

                    $.each(subValue.Urls, function (urlKey, urlValue) {

                        markup += '<div class="card">';
                        markup += '<div class="url card-header px-2 py-1" role="tab" id="heading-' + key + '-' + subKey + '-' + urlKey + '">';
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

            $(".main").click(function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
            });

            $(".sub").click(function () {
                let result = $(this).attr("id").indexOf('-');
                result = $(this).attr("id").substring(result);
                $('#collapse' + result).slideToggle(1000);
            });
        }

    }
    // END - Index Function

};
