$(function () {
    var $page_div = $('#CusPageGridDiv');
    var $page_div_table = $page_div.find('table');

    $.getJSON("/Custom/returnJS", function (data) {
        if (typeof (data) !== 'undefined') {
            if ($page_div !== null && $page_div_table !== null) {
                showPage().init(data);
            }
        }
    });

    function showPage() {
        var total_count; //總筆數
        var total_page_num; //總頁數
        var perNum; //每頁有幾筆
        var now_page_num; //現在是第幾頁
        var controller_name; // Controller/Action，之後點頁碼會用到

        var create_page_btn = function () {
            //pre btn
            var pre_btn = '<li class="prebtn"><a><span class="glyphicon glyphicon-chevron-left"></span></a></li>';
            $(".pagination").append(pre_btn);

            //頁碼按鈕
            total_page_num = Math.ceil(total_count / perNum);
            for (var i = 1; i <= total_page_num; i++) {
                var a_page_btn = '<li class="otherbtn otherbtn' + i + '"><a>' + i + '</a></li>';
                var btn = $(".pagination").append(a_page_btn);
            }

            //next btn
            var next_btn = '<li class="nextbtn"><a><span class="glyphicon glyphicon-chevron-right"></span></a></li>';
            $(".pagination").append(next_btn);
        };

        var refresh_page_btn = function () {
            for (var i = 1; i <= total_page_num; i++) {
                $('.otherbtn' + i).find('a')
                    .css('background-color', '')
                    .attr('href', controller_name + '?page_num=' + i);

                //設定那些頁數按鈕要開或關
                if (i >= (now_page_num - 3) && i <= (now_page_num + 3)) {
                    $('.otherbtn' + i).show();
                } else {
                    $('.otherbtn' + i).hide();
                }
            }

            if (now_page_num <= 1) {
                $('.prebtn').addClass("disabled");
            }
            else {
                $('.prebtn').removeClass("disabled");
            }

            if (now_page_num >= total_page_num) {
                $('.nextbtn').addClass("disabled");
            }
            else {
                $('.nextbtn').removeClass("disabled");
            }

            $('.otherbtn' + now_page_num).find('a').css('background-color', 'silver');
        };

        var create_page_btn_event = function () {
            $('.pagination li a').hover(function () {
                $(this).css('cursor', 'pointer');
            });

            //頁碼 btn
            $('.otherbtn').on('click', function (key, el) {
                refresh_page_btn();
            });

            //pre btn
            $('.prebtn').on('click', function (key, el) {
                if (now_page_num !== 1) {
                    now_page_num -= 1;
                    window.location.href = window.location.href + '?page_num=' + now_page_num;
                }
            });

            //next btn
            $('.nextbtn').on('click', function (key, el) {
                if (now_page_num !== total_page_num) {
                    now_page_num += 1;
                    window.location.href = window.location.href + '?page_num=' + now_page_num;
                }
            });
        };

        return {
            init: function (data) {
                // Controller/Action 字串
                controller_name = data[0].ControllerName;

                //目前是第幾頁
                now_page_num = data[0].now_page;

                //產生頁碼區塊
                var $_div = $('#mytable').parent();
                var $_pagination = '<div><ul class="pagination"></ul></div>';
                $_div.append($_pagination);
                $('.pagination').parent().css('text-align', 'center');

                //總筆數
                total_count = data[0].totalcount;

                //每頁幾筆
                perNum = data[0].per_num;

                if (total_count > 1) {
                    //建立分頁碼的按鈕
                    create_page_btn();
                    create_page_btn_event();

                    $('.otherbtn1').click();
                }
            }
        }
    };
});