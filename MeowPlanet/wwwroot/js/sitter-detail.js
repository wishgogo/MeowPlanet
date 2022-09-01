
//* 點擊日曆
$(document).ready(function () {
    const calendarControl = new CalendarControl()
});
$(".reserve-btn")[0].addEventListener("click", function (e) {
    $(".calender-rec").slideDown();
}, false);
$(".calender-rec").click(function (e) {
    e.stopPropagation();
})

//點擊選擇寵物
$(".reserve-number-btn")[0].addEventListener("click", function (e) {
    $(".pet-rec").slideToggle();
    $(".icon").toggleClass("rotate");
}, false);

//選擇貓咪在viewbag 裡的順位
let index;
$(".cat-card").on("click", function (e) {
    index = e.target.dataset.index;
    $(".number").html(viewBag_catList[index].name);
    $(".number").attr("data-catid", `${viewBag_catList[index].catId}`)
})




// 日曆
// 儲存開始結束時間
let startend = [];
let night;
function CalendarControl() {
    const calendar = new Date();
    let localDate = new Date();
    localDate.setHours(0, 0, 0, 0);
    const calendarControl = {
        prevMonthLastDate: null,
        calWeekDays: ["日", "一", "二", "三", "四", "五", "六"],
        calMonthName: [
            "1月",
            "2月",
            "3月",
            "4月",
            "5月",
            "6月",
            "7月",
            "8月",
            "9月",
            "10月",
            "11月",
            "12月"
        ],
        //format Date
        formatDate: function (date) {
            let format_date = `${date.getFullYear()}年${date.getMonth() + 1}月${date.getDate()}日`;
            return format_date;
        },
        formatShortDate: function (date) {
            let format_date = `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
            return format_date;
        },
        // 這個月的日數(number)
        daysInMonth: function (month, year) {
            return new Date(year, month, 0).getDate();
        },
        // 本月第一天的日期物件(date)
        firstDay: function () {
            return new Date(calendar.getFullYear(), calendar.getMonth(), 1);
        },
        lastDay: function () {
            return new Date(calendar.getFullYear(), calendar.getMonth() + 1, 0);
        },
        // 這個月第一天的星期數(number)
        firstDayNumber: function () {
            return calendarControl.firstDay().getDay() + 1;
        },
        lastDayNumber: function () {
            return calendarControl.lastDay().getDay() + 1;
        },

        getPreviousMonthLastDate: function () {
            let lastDate = new Date(
                calendar.getFullYear(),
                calendar.getMonth(),
                0
            ).getDate();
            return lastDate;
        },
        navigateToPreviousMonth: function (e) {
            calendar.setMonth(calendar.getMonth() - 1);
            calendarControl.attachEventsOnNextPrev();
            calendarControl.checkRenderOrNot();
            e.stopPropagation();

        },
        navigateToNextMonth: function (e) {
            calendar.setMonth(calendar.getMonth() + 1);
            calendarControl.attachEventsOnNextPrev();
            calendarControl.checkRenderOrNot();
            e.stopPropagation();
        },
        checkRenderOrNot: function () {
            let thisYear = calendar.getFullYear();
            let thisMonth = calendar.getMonth();
            let dateSelect = document.querySelectorAll(".number-item"); // div tag
            let dateNumber = document.querySelectorAll(".calendar .dateNumber");  //a tag

            //本日前日期槓掉
            dateNumber.forEach( aTag => {
                let aTagDate = new Date(aTag.dataset.dateid);
                if (aTagDate.getTime() < localDate.getTime()) {
                    aTag.classList.add('dateover');
                }
            });
            
            if (startend.length === 0) {

            } else if (startend.length === 1) {
                if (startend[0].getFullYear() === thisYear && startend[0].getMonth() === thisMonth) {
                    dateNumber[startend[0].getDate() - 1].classList.add("calendar-today");
                }
            } else if (startend.length === 2) {
                if (startend[0].getTime() < calendarControl.firstDay().getTime()) {

                    if (startend[1].getTime() > calendarControl.firstDay().getTime() && startend[1].getTime() < calendarControl.lastDay().getTime()) {
                        //  S | E |
                        for (let i = 0; i < startend[1].getDate(); i++) {
                            if (i == startend[1].getDate() - 1) {
                                dateSelect[i].classList.add("calendar-during-end");
                                dateNumber[i].classList.add("calendar-today");
                            } else {
                                dateSelect[i].classList.add("calendar-during");
                            }
                        }

                    } else if (startend[1].getTime() > calendarControl.lastDay().getTime()) {
                        // S | | E
                        dateSelect.forEach(element => {
                            element.classList.add("calendar-during");
                        });
                    } else if (startend[1].getTime() < calendarControl.firstDay().getTime()) {
                        console.log(" S E | |")
                        // S E | |
                    }

                } else if (startend[0].getTime() > calendarControl.firstDay().getTime() && startend[0].getTime() < calendarControl.lastDay().getTime()) {
                    if (startend[1].getTime() < calendarControl.lastDay().getTime()) {
                        //  |S E|
                        let night = (startend[1] - startend[0]) / (1000 * 3600 * 24);
                        for (let i = 0; i < night + 1; i++) {
                            if (i == 0) {
                                dateSelect[startend[0].getDate() - 1 + i].classList.add("calendar-during-start");
                                dateNumber[startend[0].getDate() - 1 + i].classList.add("calendar-today");
                            } else if (i == night) {
                                dateSelect[startend[0].getDate() - 1 + i].classList.add("calendar-during-end");
                                dateNumber[startend[0].getDate() - 1 + i].classList.add("calendar-today");
                            } else {
                                dateSelect[startend[0].getDate() - 1 + i].classList.add("calendar-during");
                            }
                        }
                    } else if (startend[1].getTime() > calendarControl.lastDay().getTime()) {
                        // | S | E
                        let night = calendarControl.lastDay().getDate() - startend[0].getDate()
                        for (let i = 0; i < night; i++) {
                            if (i == 0) {
                                dateSelect[startend[0].getDate() - 1 + i].classList.add("calendar-during-start");
                                dateNumber[startend[0].getDate() - 1 + i].classList.add("calendar-today");
                            } else {
                                dateSelect[startend[0].getDate() - 1 + i].classList.add("calendar-during");
                            }
                        }
                    }
                } else if (startend[0].getTime() > calendarControl.lastDay().getTime() &&
                    startend[1].getTime() > calendarControl.lastDay().getTime()) {
                    //  | | S E 
                }
            }
        },

        displayYear: function () {
            let yearLabel = document.querySelector(".calendar .calendar-year-label");
            yearLabel.innerHTML = calendar.getFullYear();
        },

        displayMonth: function () {
            let monthLabel = document.querySelector(
                ".calendar .calendar-month-label"
            );
            monthLabel.innerHTML = calendarControl.calMonthName[calendar.getMonth()];
        },

        // 月份選擇
        plotSelectors: function () {
            document.querySelector(".calendar").innerHTML +=
            `<div class="calendar-inner">
                <div class="calendar-controls">
                    <div class="calendar-prev">
                        <a >
                            <svg xmlns="http://www.w3.org/2000/svg" width="128" height="128" viewBox="0 0 128 128">
                                <path fill="#666" d="M88.2 3.8L35.8 56.23 28 64l7.8 7.78 52.4 52.4 9.78-7.76L45.58 64l52.4-52.4z" />
                            </svg>
                        </a>
                    </div>
                    <div class="calendar-year-month">
                        <div class="calendar-year-label"></div>
                        <div>-</div>
                        <div class="calendar-month-label"></div>
                    </div>
                    <div class="calendar-next">
                        <a >
                            <svg xmlns="http://www.w3.org/2000/svg" width="128" height="128" viewBox="0 0 128 128">
                                <path fill="#666"  d="M38.8 124.2l52.4-52.42L99 64l-7.77-7.78-52.4-52.4-9.8 7.77L81.44 64 29 116.42z" />
                            </svg>
                        </a>
                    </div>
                </div>
                <div class="calendar-body"></div>
            </div>`;
        },

        // 輸出星期名字
        plotDayNames: function () {
            for (let i = 0; i < calendarControl.calWeekDays.length; i++) {
                document.querySelector(
                    ".calendar .calendar-body"
                ).innerHTML += `<div>${calendarControl.calWeekDays[i]}</div>`;
            }
        },

        //輸出日期
        plotDates: function () {
            document.querySelector(".calendar .calendar-body").innerHTML = "";
            calendarControl.plotDayNames();
            calendarControl.displayMonth();
            calendarControl.displayYear();
            let count = 1;
            let prevDateCount = 0;

            calendarControl.prevMonthLastDate = calendarControl.getPreviousMonthLastDate();
            let prevMonthDatesArray = [];
            let calendarDays = calendarControl.daysInMonth(
                calendar.getMonth() + 1,
                calendar.getFullYear()
            );
            // dates of current month
            for (let i = 1; i < calendarDays; i++) {
                if (i < calendarControl.firstDayNumber()) {
                    prevDateCount += 1;
                    document.querySelector(
                        ".calendar .calendar-body"
                    ).innerHTML += `<div class="prev-dates"></div>`;
                } else {
                    document.querySelector(
                        ".calendar .calendar-body"
                    ).innerHTML +=
                        `<div class="number-item">
                                    <a class="dateNumber" data-dateid="${calendar.getFullYear()}/${calendar.getMonth() + 1}/${count}">${count++}</a>
                                </div>`;
                }
            }
            //remaining dates after month dates
            for (let j = 0; j < prevDateCount + 1; j++) {
                document.querySelector(
                    ".calendar .calendar-body"
                ).innerHTML +=
                    `<div class="number-item" >
                        <a class="dateNumber" data-dateid="${calendar.getFullYear()}/${calendar.getMonth() + 1}/${count}">${count++}
                        </a>
                     </div>`;
            }
        },

        attachEvents: function () {
            let prevBtn = document.querySelector(".calendar .calendar-prev a");
            let nextBtn = document.querySelector(".calendar .calendar-next a");
            let todayDate = document.querySelector(".calendar .calendar-today-date");
            let dateNumber = document.querySelectorAll(".calendar .dateNumber");
            let clearSelect = document.querySelector(".clear-select");
            let closeCalendar = document.querySelector(".close-calendar");

            prevBtn.addEventListener("click", calendarControl.navigateToPreviousMonth);
            nextBtn.addEventListener("click", calendarControl.navigateToNextMonth);
            for (var i = 0; i < dateNumber.length; i++) {
                dateNumber[i].addEventListener(
                    "click",
                    calendarControl.selectDate,
                    false
                );
            }
            clearSelect.addEventListener("click", calendarControl.clearSelect);
            closeCalendar.addEventListener("click", calendarControl.closeCalendar);
        },

        closeCalendar: function (e) {
            if (startend[0] !== undefined) {
                $("#check-in").html(calendarControl.formatShortDate(startend[0]));
            } else {
                $("#check-in").html("輸入日期");
            }
            if (startend[1] !== undefined) {
                $("#check-out").html(calendarControl.formatShortDate(startend[1]));
            } else {
                $("#check-out").html("輸入日期");

            }
            $(".calender-rec").slideUp();
        },

        clearSelect: function (e) {
            let dateSelect = document.querySelectorAll(".number-item"); // div tag
            let dateNumber = document.querySelectorAll(".calendar .dateNumber");  //a tag
            dateSelect.forEach(element => { element.classList.remove("calendar-during") });
            dateSelect.forEach(element => { element.classList.remove("calendar-during-end") });
            dateSelect.forEach(element => { element.classList.remove("calendar-during-start") });
            dateNumber.forEach(element => { element.classList.remove("calendar-today") });
            document.querySelector(".night").innerHTML = "選擇開始日期";
            document.querySelector(".calender-header span:nth-child(1)").innerHTML = "";
            document.querySelector(".calender-header span:nth-child(2)").innerHTML = "";
            startend = [];
            e.stopPropagation();
        },

        selectDate: function (e) {
            let selectDate = new Date(e.target.dataset.dateid);
            //Mon Aug 08 2022 00:00:00 GMT+0800 (GMT+08:00)
            // 本日後才能點
            if (localDate.getTime() <= selectDate.getTime()) {
                if (startend.length === 0) {
                    startend.push(selectDate);
                    e.target.classList.add("calendar-today");
                    document.querySelector(".calender-header span:nth-child(1)").innerHTML = calendarControl.formatDate(selectDate);
                    document.querySelector(".night").innerHTML = "選擇結束日期";
                    //! disable prev date
                } else if (startend.length === 1) {
                    if (startend[0] < selectDate) {
                        startend.push(selectDate);
                        e.target.classList.add("calendar-today");
                        document.querySelector(".calender-header span:nth-child(2)").innerHTML = calendarControl.formatDate(selectDate);
                        console.log(startend[1], startend[0])
                        night = (startend[1] - startend[0]) / (1000 * 3600 * 24);
                        document.querySelector(".night").innerHTML = `${night}晚`;
                        if (startend[0].getTime() > calendarControl.firstDay().getTime()) {
                            for (let i = 0; i < night + 1; i++) {
                                let dateSelect = document.querySelectorAll(".number-item")[startend[0].getDate() - 1 + i];
                                if (i == 0) {
                                    dateSelect.classList.add("calendar-during-start");
                                } else if (i == night) {
                                    dateSelect.classList.add("calendar-during-end");
                                } else {
                                    dateSelect.classList.add("calendar-during");
                                }
                            }
                        } else {
                            for (let i = 0; i < startend[1].getDate(); i++) {
                                let dateSelect = document.querySelectorAll(".number-item")[0 + i];
                                if (i == startend[1].getDate() - 1) {
                                    dateSelect.classList.add("calendar-during-end");
                                } else {
                                    dateSelect.classList.add("calendar-during");
                                }
                            }
                        }

                    }
                } else if (startend.length === 2) {
                    // document.querySelector(".calender-header span:nth-child(1)").innerHTML = "";
                    // document.querySelector(".calender-header span:nth-child(2)").innerHTML = "";
                    // document.querySelector(".night").innerHTML = "選擇開始日期";
                    // startend = [];
                    // calendarControl.selectDate(e);
                }
            }
            e.stopPropagation();
        },

        attachEventsOnNextPrev: function () {
            calendarControl.plotDates();
            calendarControl.attachEvents();
        },
        init: function () {
            calendarControl.plotSelectors();
            calendarControl.plotDates();
            calendarControl.attachEvents();
            calendarControl.checkRenderOrNot();
        }
    };
    calendarControl.init();
}

//* 彈出評論



//* 地圖
let map;
let marker;
let catList = [];
function initMap() {
    //偏移保姆位置
    let seed = model.sitter.serviceId % 4;
    let part = model.sitter.serviceId % 10 + 1;
    let latlng;
    switch (seed) {
        case 0:
            latlng = new google.maps.LatLng(model.sitter.posLat + 0.002 * (part / 10), model.sitter.posLng + 0.002 * (part / 10));
            break;
        case 1:
            latlng = new google.maps.LatLng(model.sitter.posLat + 0.002 * (part / 10), model.sitter.posLng - 0.002 * (part / 10));
            break;
        case 2:
            latlng = new google.maps.LatLng(model.sitter.posLat - 0.002 * (part / 10), model.sitter.posLng - 0.002 * (part / 10));
            break;
        case 3:
            latlng = new google.maps.LatLng(model.sitter.posLat - 0.002 * (part / 10), model.sitter.posLng + 0.002 * (part / 10));
            break;
    }

    //初始化地圖
    map = new google.maps.Map($('#map')[0], {
        center: latlng,
        zoom: 16,
        // minZoom: 12,
        // maxZoom: 17,
        disableDefaultUI: true,
        mapId: 'a5f4cec6781c8dda',
        gestureHandling: 'cooperative'
    });

    //保姆位置marker
    marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: {
            url: "../../images/sitter/house_marker.png",
            scaledSize: new google.maps.Size(50, 50),
            anchor: new google.maps.Point(25, 25) // anchor
        },
    });

    //模糊圓圈
    const blurCircle = new google.maps.Circle({
        strokeColor: "#B99668",
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: "#B99668",
        fillOpacity: 0.35,
        map,
        center: latlng,
        radius: 300,
    });

    //定位按鈕
    const locationButton = document.createElement("button");
    locationButton.innerHTML = '<i class="fa-solid fa-crosshairs fa-lg"></i>';
    $(locationButton).addClass('btn btn-dark btn-location');
    $(locationButton).css('border-radius', '10px');
    $(locationButton).css('border-color', 'white');
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(locationButton);
    $(locationButton).on('click', function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    let pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };
                    map.setCenter(pos);
                }
            )
        };
    })
}

window.initMap = initMap;




//照片輪播
function moveToSelected(element) {

    if (element == "next") {
        var selected = $(".selected").next();
    } else if (element == "prev") {
        var selected = $(".selected").prev();
    } else {
        var selected = element;
    }

    var next = $(selected).next();
    var prev = $(selected).prev();
    var prevSecond = $(prev).prev();
    var nextSecond = $(next).next();

    $(selected).removeClass().addClass("selected");

    $(prev).removeClass().addClass("prev");
    $(next).removeClass().addClass("next");

    $(nextSecond).removeClass().addClass("nextRightSecond");
    $(prevSecond).removeClass().addClass("prevLeftSecond");

    $(nextSecond).nextAll().removeClass().addClass('hideRight');
    $(prevSecond).prevAll().removeClass().addClass('hideLeft');

}

// Eventos teclado
$(document).keydown(function (e) {
    switch (e.which) {
        case 37: // left
            moveToSelected('prev');
            break;

        case 39: // right
            moveToSelected('next');
            break;

        default: return;
    }
    e.preventDefault();
});

$('#carousel div').click(function () {
    moveToSelected($(this));
});

$('#prev').click(function () {
    moveToSelected('prev');
});

$('#next').click(function () {
    moveToSelected('next');
});



//跳轉到ComfirmPay
$('#comfirmPay').click(
    () => {
        checkLogin(() => {
            if (startend.length === 2 && index !== null) {
                $('#confirmForm input[name="startDate"]').val(startend[0].toJSON());
                //2022-08-01T16:00:00.000Z
                $('#confirmForm input[name="endDate"]').val(startend[1].toJSON());
                $('#confirmForm input[name="night"]').val(night);
                $('#confirmForm input[name="catId"]').val(viewBag_catList[index].catId);
                $('#confirmForm input[name="serviceId"]').val(model.sitter.serviceId);
                 /*alert($('#confirmForm input[name="sitter"]').val());*/
                $('#confirmForm').submit();
            } else {
                alert("請先選擇入住退房日期及需保姆寵物");
            }
            
        });
    }
)

//私訊保姆
$('.black-overlay').click(function () {
    let s = $(this).data('sid');
    messagebox.messageTo(s, memberId);
});

