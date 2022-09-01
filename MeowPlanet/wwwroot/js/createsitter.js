$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});

// 畫面切換
$('#f1-b1').click(function () {
    $('#p1').css('display', 'none');
    $('#p2').css('display', 'block');
    $('#f1').css('display', 'none');
    $('#f2').css('display', 'block');
})

$('#name-in').keyup(function () {
    $('#name-err').text('');
})
$('#address').keyup(function () {
    $('#address-err').text('');
})
$('#intro-in').keyup(function () {
    $('#intro-err').text('');
})

$('#f2-b4').click(function () {
    if ($('#name-in').val() == "" || $('#address').val() == "" || $('#intro-in').val() == "") {
        if ($('#name-in').val() == "") {
            $('#name-err').text('請填寫服務名稱');
        }
        if ($('#address').val() == "") {
            $('#address-err').text('請填寫照顧地點');
        }
        if ($('#intro-in').val() == "") {
            $('#intro-err').text('請填寫保姆自介');
        }
        return;
    }

    $('#p2').css('display', 'none');
    $('#p3').css('display', 'block');
    $('#f2').css('display', 'none');
    $('#f3').css('display', 'block');

})

$('#f2-b3').click(function () {
    $('#p1').css('display', 'block');
    $('#p2').css('display', 'none');
    $('#f1').css('display', 'block');
    $('#f2').css('display', 'none');
})

$('#f3-b1').click(function () {

    $('#p3').css('display', 'none');
    $('#p2').css('display', 'block');
    $('#f3').css('display', 'none');
    $('#f2').css('display', 'block');

})

$('#pay-in').keyup(function () {
    $('#pay-err').text('');
})

$('#f3-b2').click(function () {

    if ($('#pay-in').val() == '') {
        $('#pay-err').text('請填寫服務價格');
        return;
    }
    $('#p3').css('display', 'none');
    $('#p4').css('display', 'block');
    $('#f3').css('display', 'none');
    $('#f4').css('display', 'block');
})

$('#f4-b1').click(function () {
    $('#p4').css('display', 'none');
    $('#p3').css('display', 'block');
    $('#f4').css('display', 'none');
    $('#f3').css('display', 'block');
})

$('.con2-b').click(function () {
    $('#meal-err').text('');
})
$('.con3-b').click(function () {
    $('#num-err').text('');
})

$('#f4-b2').click(function () {
    if ($('#meal-in').val() == '' || $('#num-in').val() == '') {
        if ($('#meal-in').val() == "") {
            $('#meal-err').text('請選擇是否供餐');
        }
        if ($('#num-in').val() == "") {
            $('#num-err').text('請選擇照顧數量');
        }
        return;
    }
    $('#p4').css('display', 'none');
    $('#p5').css('display', 'block');
    $('#f4').css('display', 'none');
    $('#f5').css('display', 'block');
})

$('#f5-b1').click(function () {
    $('#p5').css('display', 'none');
    $('#p4').css('display', 'block');
    $('#f5').css('display', 'none');
    $('#f4').css('display', 'block');
})

$('#f5-b2').click(function () {
    $('#p5').css('display', 'none');
    $('#p6').css('display', 'block');
    $('#f5').css('display', 'none');
    $('#f6').css('display', 'block');
})

$('#f6-b1').click(function () {
    $('#p6').css('display', 'none');
    $('#p5').css('display', 'block');
    $('#f6').css('display', 'none');
    $('#f5').css('display', 'block');
})

$('#f6-b2').click(function (e) {
    e.preventDefault();
    var form = $(this).parents('form');

    Swal.fire({
        heightAuto: false,
        position: 'center',
        icon: 'success',
        title: '服務建立完成',
        showConfirmButton: false,
        timer: 2500
    }).then(function () {
        form.submit();
    })
})


let inputList = [] // 放input dom的list
let imgList = [] //放img dom的list

for (var i = 0; i < $(":file").length; i++) {
    inputList.push($(":file")[i])  //把所有的input dom加進去
}

for (var i = 0; i < $(".dropImg").length; i++) {
    imgList.push($(".dropImg")[i]) //把所有的img dom加進去
}

let remainNum = 5;

$(".dropZone, .dropZone1").on({

    "dragover": function (event) {
        event.preventDefault();
    },
    "drop": function (event) {

        event.preventDefault();
        event.stopPropagation();

        let fileList = event.originalEvent.dataTransfer.files;  //把滑鼠抓住的若干檔案assign進去

        for (let i = 0; i < fileList.length; i++) {

            let file = fileList[i];          //把fileList拆分成單獨file跑迴圈

            if (file.type.indexOf('image') == -1) {

                Swal.fire({
                    heightAuto: false,
                    position: 'center',
                    title: '請上傳正確的圖片格式',
                    icon: 'warning',
                    showConfirmButton: false,
                    timer: 2000
                })

                return
            }

            //預覽功能
            let reader = new FileReader();
            reader.readAsDataURL(file);
            reader.addEventListener("load", function (event) {
                for (let i = 0; i < imgList.length; i++) {

                    //如果該img為空則把該圖片的url assign進去
                    if (imgList[i].src == '') {
                        imgList[i].src = event.target.result;
                        $(`#plus${i + 1}`).css('display', 'none')
                        $(`#dropZone${i + 1}`).css('border', '2px rgb(115, 244, 222) solid')
                        break;  //跳脫出for迴圈
                    }
                }
            })

            //檔案上傳
            for (let i = 0; i < inputList.length; i++) {

                //如果該input裡的file為空則把file assign進去
                if (inputList[i].files.length == 0) {

                    let dt = new DataTransfer();
                    dt.items.add(file);
                    inputList[i].files = dt.files;
                    break;
                }
            }
        }

    }
})


$(function () {
    $('#theFile1').change(function () {
        fileChange(1);
    })
    $('#theFile2').change(function () {
        fileChange(2);
    })
    $('#theFile3').change(function () {
        fileChange(3);
    })
    $('#theFile4').change(function () {
        fileChange(4);
    })
    $('#theFile5').change(function () {
        fileChange(5);
    })


})

function fileChange(num) {
    let file = $(`#theFile${num}`)[0].files[0]

    if (file.type.indexOf('image') == -1) {

        $(`#theFile${num}`).val('');

        Swal.fire({
            heightAuto: false,
            position: 'center',
            title: '請上傳正確的圖片格式',
            icon: 'warning',
            showConfirmButton: false,
            timer: 2000
        })

        return
    }

    let readFile = new FileReader()
    readFile.readAsDataURL(file)
    readFile.addEventListener('load', function () {
        $(`#dropImg${num}`)[0].src = readFile.result
    })
    $(`#plus${num}`).css('display', 'none')
    $(`#dropZone${num}`).css('border', '2px rgb(115, 244, 222) solid')
}

// 地址轉經緯度
var geocoder;

function initMap() {
    geocoder = new google.maps.Geocoder();
}

function getlatlng() {
    var address = $('#address').val();
    $('#area_1,#area_2,#area_3').attr('value', '');
    geocoder.geocode({
        'address': address
    }, function (result) {
        $('#lat').attr('value', result[0].geometry.location.lat());
        $('#lng').attr('value', result[0].geometry.location.lng());

        $('#formatted_address').attr('value', result[0].formatted_address);
        for (const component of result[0].address_components) {
            if (component.types[0] == 'administrative_area_level_1') {
                $('#area_1').attr('value', component.long_name);
            }
            if (component.types[0] == 'administrative_area_level_2') {
                $('#area_2').attr('value', component.long_name);
            }
            if (component.types[0] == 'administrative_area_level_3') {
                $('#area_3').attr('value', component.long_name);
            }
        }
    })
}

// 照顧條件選擇
$('.con-b').click(function () {
    $(this).toggleClass('active').blur();
})

$('#licence-btn').click(function () {
    if ($(this).val() == "無證照") {
        $('#licence-in').attr('value', "有證照");
        $(this).attr('value', "有證照");
    } else {
        $('#licence-in').attr('value', "無證照");
        $(this).attr('value', "無證照");
    }
})

$('#cage-btn').click(function () {
    if ($(this).val() == "不關籠子") {
        $('#cage-in').attr('value', "需關籠子");
        $(this).attr('value', "需關籠子");
    } else {
        $('#cage-in').attr('value', "不關籠子");
        $(this).attr('value', "不關籠子");
    }
})


$('#monitor-btn').click(function () {
    if ($(this).val() == "無監視器") {
        $('#monitor-in').attr('value', "有監視器");
        $(this).attr('value', "有監視器");
    } else {
        $('#monitor-in').attr('value', "無監視器");
        $(this).attr('value', "無監視器");
    }
})

$('.con2-b').click(function () {

    $('.con2-b').removeClass('b-press');
    $(this).addClass('b-press');

    $('#meal-in').attr('value', this.value);
})

$('.con3-b').click(function () {

    $('.con3-b').removeClass('b-press');
    $(this).addClass('b-press');

    $('#num-in').attr('value', this.value);
})

var feature = [];

Array.prototype.remove = function (value) {
    this.splice(this.indexOf(value), 1);
}

$('.con4-b').click(function () {
    $(this).toggleClass('active').blur();

    if ($(this).hasClass('active')) {
        s = 1;
    } else {
        s = 0;
    }

    if (s == 1) {
        feature.push($(this).val());
    } else {
        feature.remove($(this).val());
    }

    $('#feature').attr('value', feature);

})

