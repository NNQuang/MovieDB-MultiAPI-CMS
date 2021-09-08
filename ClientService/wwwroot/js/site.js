
$(document).ready(function () {

    //ilk açılışta fade animasyonu
    $('.aos-animate').fadeIn(2500);

    //infinite scrolling ajax call
    var PageNumber = 0;
    var SayfaPost = true;
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            if (SayfaPost) {
                $.ajax({
                    url: '/Movie/',
                    data: { pageNumber: PageNumber },
                    success: function (data) {
                        if ($.trim(data) == '') {
                            SayfaPost = false;
                        } else {
                            $("#lightgallery").append(data);
                            PageNumber++;
                        }
                        $('.aos-animate').fadeIn(3500);
                    }
                });
            }
        }
    });
});

