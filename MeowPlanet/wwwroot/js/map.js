let map;

//使用者設置的圖標
let newMarker;

//從資料庫撈到的所有貓咪物件
let catList = [];

let detalModal;

let clickedMarker;


function removeMarker() {
    if (newMarker != null) {
        newMarker.setMap(null);
    }
}

//把地圖平移到目前位置
function currentPos() {
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
}

function initMap() {

    //初始化地圖
    map = new google.maps.Map($('#map')[0], {
        center: { lat: 22.629314218928563, lng: 120.29299528465663 },
        zoom: 14,
        minZoom: 10,
        maxZoom: 18,
        disableDefaultUI: true,
        mapId: 'a5f4cec6781c8dda',
        gestureHandling: 'greedy',
        clickableIcons: false
    });

    currentPos();

    //定位按鈕
    const locationButton = document.createElement("button");
    locationButton.innerHTML = '<i class="fa-solid fa-crosshairs fa-lg"></i>';
    $(locationButton).addClass('btn btn-dark btn-location');
    $(locationButton).css('border-radius', '10px');
    $(locationButton).css('border-color', 'white');
    $(locationButton).css('padding-bottom', '10px');
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(locationButton);
    $(locationButton).on('click', function () {
        currentPos();
    })


    //刊登協尋
    $('#pre-publish').on('click', function () {

        checkLogin(() => {

            checkCats(() => {

                let windowContent =
                    '<div class="px-3 pt-2 pb-1">' +
                    '<span class="h5 my-3">您的愛貓在這裡走失的嗎?</span>' +
                    '<div class="d-flex mt-3 mb-1 justify-content-between">' +
                    '<button class="btn btn-dark rounded-pill" onclick="confirmPos()">' +
                    '<i class="fa-solid fa-check me-2"></i>' +
                    '確認' +
                    '</button>' +
                    '<button onclick="removeMarker();" class="btn btn-danger rounded-pill">' +
                    '<i class="fa-solid fa-xmark me-2"></i>' +
                    '取消' +
                    '</button>' +
                    '</div>' +
                    '</div>';

                settingMode(windowContent);
            })
        })

    })


    //搜尋框
    const input = document.getElementById("search-input");
    const searchBox = new google.maps.places.SearchBox(input);

    map.controls[google.maps.ControlPosition.TOP_CENTER].push(input);

    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });

    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }
        const bounds = new google.maps.LatLngBounds();

        places.forEach((place) => {
            if (!place.geometry || !place.geometry.location) {
                console.log("Returned place contains no geometry");
                return;
            }

            if (place.geometry.viewport) {
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

    map.addListener('zoom_changed', () => {
        let zoom = map.getZoom();
        switch (zoom) {
            case 10:
                scaledRange = 64000;
                break;
            case 11:
                scaledRange = 32000;
                break;
            case 12:
                scaledRange = 16000;
                break;
            case 13:
                scaledRange = 8000;
                break;
            case 14:
                scaledRange = 4000;
                break;
            case 15:
                scaledRange = 2000;
                break;
            case 16:
                scaledRange = 1000;
                break;
            case 17:
                scaledRange = 500;
                break;
            case 18:
                scaledRange = 250;
                break;
            default:                
                Swal.fire({
                    heightAuto: false,
                    position: 'center',
                    icon: 'warning',
                    title: '搜尋的地區超出縮放範圍',
                    text: '請重新執行搜尋',
                    showConfirmButton: false,
                    timer: 2000
                })
                break;                
        }
    })
    //綁定移動事件
    listener = map.addListener('idle', () => { searchCat(scaledRange) })
}

let scaledRange = 4000;

//刊登協尋 檢查名下是否有貓&&是否已有刊登
function checkCats(callback) {
    $.get('/Missings/CheckCats', function (data) {
        if (data == "0") {
            Swal.fire({
                title: '您尚未登記任何貓咪',
                text: '是否要前往登記頁面?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: '前往登記',
                cancelButtonText: '下次一定',
                heightAuto: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = "/Member/CreateCat";
                }
                else {
                    return false
                }
            })
        }

        else if (data == "1") {

            Swal.fire({
                heightAuto: false,
                position: 'center',
                title: '每位會員僅限刊登一筆協尋',
                icon: 'warning',
                showConfirmButton: false,
                timer: 2500
            })
        }

        else {
            callback();
        }
    })
}

//檢查登入
function checkLogin(action) {

    if (isLogin == "False") {
        Swal.fire({
            title: '請登入會員以執行此操作',
            text: '是否要前往登入頁面?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: '前往登入',
            cancelButtonText: '下次一定',
            heightAuto: false,
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = "/Login";
            }
            else {
                return false
            }
        })
    }
    else {
        action();
    }
}


//設定位置模式
function settingMode(windowContent, offset) {

    google.maps.event.removeListener(listener);

    //把頁面上圖標隱藏
    setMarkerVisibility('hide');

    map.setOptions({ draggableCursor: 'url(../images/marker-cursor.png) 15 45, auto' });

    toggleMap()

    let listenerHandle = map.addListener("click", (e) => {

        removeMarker();

        newMarker = new google.maps.Marker({
            position: e.latLng,
            map,
            Draggable: true,
            icon: {
                url: "images/marker.png",
                scaledSize: new google.maps.Size(40, 57)
            }
        });


        map.setZoom(16);
        if (offset == true) {

            map.panTo({
                lat: newMarker.getPosition().lat() + 0.005,
                lng: newMarker.getPosition().lng()
            });
        }
        else {
            map.panTo(newMarker.getPosition());
        }


        const infowindow = new google.maps.InfoWindow({
            content: windowContent
        });

        infowindow.open({
            anchor: newMarker,
            map,
            shouldFocus: false,
        });
    });

    //切換刊登&取消刊登顯示
    $('#pre-publish').toggleClass('d-none');
    $('#cancel-publish').toggleClass('d-none');

    //取消刊登按鈕 綁定單次事件
    $('#cancel-publish').one('click', function () {

        map.setOptions({ draggableCursor: '' });
        google.maps.event.removeListener(listenerHandle);

        $('#cancel-publish').toggleClass('d-none');
        $('#pre-publish').toggleClass('d-none');

        toggleMap();

        removeMarker();

        //把頁面上圖標顯示回來
        setMarkerVisibility('show');

        listener = map.addListener('idle', () => { searchCat(scaledRange) });
    })
}




let missingId;
let clueCount;
let showingWindow;
let cluesMarker = [];

//把所有走失貓咪抓進catList
$(function () {
    $.get('/Missings/GetMissing', function (data) {
        data.forEach((value) => {
            let cat = value;
            let LatLng = new google.maps.LatLng(cat.lat, cat.lng);
            let marker = new google.maps.Marker({
                position: LatLng,
                icon: {
                    url: "images/marker.png",
                    scaledSize: new google.maps.Size(33, 46)
                }
            });

            //綁定此marker點擊事件
            marker.addListener('click', function () {

                clickedMarker = marker;

                //地圖中心移動到圖標位置
                map.setZoom(16);
                map.panTo({
                    lat: this.getPosition().lat(), //+ 0.005
                    lng: this.getPosition().lng()
                })

                //發送AJAX取得資料
                $.get('/Missings/GetDetail', { 'missingId': cat.missingId }, function (data) {
                    detalModal = data;
                    $('#detailModal').html(data);
                    $('#detailModal').modal('show');
                })

                missingId = cat.missingId;
                clueCount = $(`#missing-${missingId}`).data('clueCount');
            })

            //綁定此marker hover事件
            marker.addListener('mouseover', function () {
                const id = cat.missingId;
                catList.filter(x => x.missingId == id)[0].marker.setAnimation(google.maps.Animation.BOUNCE);
                showingCatList.filter(x => x.missingId != id).forEach((cat) => {
                    cat.marker.setOptions({ 'opacity': 0.35 })
                })
            })

            marker.addListener('mouseout', function () {
                const id = cat.missingId;
                catList.filter(x => x.missingId == id)[0].marker.setAnimation(null);
                catList.forEach((cat) => {
                    cat.marker.setOptions({ 'opacity': 1 })
                })
            })

            cat.marker = marker;
            catList.push(cat);
        })
    })

        .then(function () {
            searchCat(scaledRange);
        })

})

let showingCatList = [];

//搜尋中心點附近貓貓
function searchCat(range) {

    catList.forEach((cat) => {
        let marker = cat.marker;
        let LatLng = new google.maps.LatLng(cat.lat, cat.lng);
        let center = map.getCenter();
        let distance = google.maps.geometry.spherical.computeDistanceBetween(center, LatLng);


        //距離小於1公里就加入圖標 & 顯示在左邊item列
        if (distance < range) {

            marker.setMap(map);

            if (!showingCatList.includes(cat)) {
                showingCatList.push(cat);
                $(`#missing-${cat.missingId}`).remove().appendTo('#div-items').delay(100).fadeIn(600);
            }
            $(`#missing-${cat.missingId}`).fadeIn(600);
        }
        else {

            marker.setMap(null);

            if (showingCatList.includes(cat)) {
                showingCatList.splice(showingCatList.indexOf(cat), 1);
                $(`#missing-${cat.missingId}`).animate({ marginLeft: "200px", opacity: 0 }, 500, 'swing', function () { $(this).hide().css({ 'margin-left': '0', 'opacity': '1' }) });
            }
        }
    })
}

// 顯示/隱藏圖標
function setMarkerVisibility(option) {
    for (var i = 0; i < showingCatList.length; i++) {
        if (option == 'show') {
            showingCatList[i].marker.setMap(map);
        }
        else if (option == 'hide') {
            showingCatList[i].marker.setMap(null);
        }
    }
}


//關掉前一個貓貓視窗
function removeWindow(clickedWindow) {
    if (showingWindow != null) {
        showingWindow.close();
    }
    showingWindow = clickedWindow
}


//點擊item導到該marker且打開window
function itemClicked(item) {

    let id = $(item).data('id');
    missingId = id;
    clueCount = $(item).data('clueCount');
    let clickedCat = catList.filter(x => x.missingId == id)[0];
    clickedMarker = clickedCat.marker;


    map.setZoom(16);
    map.panTo({
        lat: clickedCat.marker.getPosition().lat(),
        lng: clickedCat.marker.getPosition().lng()
    });

    //發送AJAX取得資料
    $.get('/Missings/GetDetail', { 'missingId': id }, function (data) {

        detalModal = data;
        $('#detailModal').html(data);
        $('#detailModal').modal('show')
    })
}

function itemActive(item) {
    let id = $(item).data('id');
    catList.filter(x => x.missingId == id)[0].marker.setAnimation(google.maps.Animation.BOUNCE);
    showingCatList.filter(x => x.missingId != id).forEach((cat) => {
        cat.marker.setOptions({ 'opacity': 0.35 })
    })
}

function itemInactive(item) {
    let id = $(item).data('id');
    catList.filter(x => x.missingId == id)[0].marker.setAnimation(null);
    catList.forEach((cat) => {
        cat.marker.setOptions({ 'opacity': 1 })
    })
}

//切換地圖區塊布局
function toggleMap() {
    $('#div-items').toggleClass('items-activate');
    $('#div-map').toggleClass('map-activate');
}

//確定貓咪在此遺失
function confirmPos() {

    $.get('/Missings/GetPublish', (data) => {
        $('#publishModal').html(data);
        $('#publishModal').modal('show');

        $('#lat').val(newMarker.getPosition().lat());
        $('#lng').val(newMarker.getPosition().lng());
    })
}

//preview綁定事件
$(document).on({
    'click': () => $('#imgInput').trigger("click"), //點擊照片觸發input點擊事件
    'dragover': (event) => event.preventDefault(),
    'drop': (event) => {

        event.preventDefault();

        let file = event.originalEvent.dataTransfer.files[0];

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

        let reader = new FileReader()

        reader.readAsDataURL(file);
        reader.onload = function (e) {
            $('#imgPreview').attr('src', e.target.result)
        }

        let dt = new DataTransfer();
        dt.items.add(file);
        $('#imgInput')[0].files = dt.files
    }
}, '#imgPreview')

//上傳預覽功能
$(document).on('change', '#imgInput', function () {    

    if ($(this)[0].files[0].type.indexOf('image') == -1) {

        $(this).val('');

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


    let reader = new FileReader();
    reader.onload = function (e) {
        $('#imgPreview').attr('src', e.target.result);
    }

    reader.readAsDataURL($('#imgInput')[0].files[0])
})


//提供線索
$(document).on('click', '#provideClues', () => {

    checkLogin(() => {

        if (memberId == $('#provideClues').data('memberId')) {
            Swal.fire({
                heightAuto: false,
                position: 'center',
                title: '您無法對您發布的協尋提供線索',
                icon: 'warning',
                showConfirmButton: false,
                timer: 2000
            })
        }
        else {

            date = new Date($('#provideClues').data('missingDate'))

            let windowContent =
                '<form id="clueForm" enctype="multipart/form-data" style="width: 350px; height: 450px" autocomplete="off">' +
                '<div class="h5 text-center">請輸入線索詳細資訊</div>' +
                '<div class="form-group mt-3 text-center" style="height: 40%">' +
                '<image id="imgPreview" class="img-upload" src="/images/addphoto.png" style="width: 80%;height: 100%; object-fit: cover;" />' +
                '<input id="imgInput" type="file" class="form-control d-none" name="Image" accept="image/*" />' +
                '</div>' +
                '<div class="form-group mt-3 m-auto w-85">' +
                '<label for="witness-time" class="h6">目擊時間 :</label>' +
                '<input type="text" id="witness-time" class="form-control rounded-pill" name="WitnessTime" readonly />' +
                '</div>' +
                '<div class="form-group mt-3 m-auto w-85">' +
                '<label for="other-description" class="h6">其他描述 :</label>' +
                '<input type="text" id="other-description" class="form-control rounded-pill" name="Description" />' +
                '</div>' +
                '<div class="d-flex mt-4 mb-1" style="justify-content: space-evenly">' +
                '<button type="submit" class="btn btn-dark rounded-pill"><i class="fa-solid fa-check me-1"></i> 發布</button>' +
                '<button onclick="removeMarker();" class="btn btn-danger rounded-pill"><i class="fa-solid fa-xmark me-1"></i> 取消</button>' +
                '</div>' +
                '<input name="WitnessLat" id="WitnessLat" type="hidden" />' + '<input name="WitnessLng" id="WitnessLng" type="hidden" />' +
                `<input name="MissingId" id="MissingId" type="hidden" value="${missingId}"/>` +
                '</form>';

            postClueId = missingId;
            $('#detailModal').modal('hide');

            settingMode(windowContent, true);


            $('#cancel-publish').one('click', () => {
                $('#detailModal').html(detalModal);
                $('#detailModal').modal('show');
            })

        }

    })

})

let date;

$(function () {
    $(document).on({
        'focus': function () {
             
            $(this).datepicker({ maxDate: 0, minDate: date });
        }
    }, '#witness-time'
    )
})

let postClueId;
//提供線索表單
$(document).on('submit', '#clueForm', function (e) {

    $('#WitnessLat').val(newMarker.getPosition().lat());
    $('#WitnessLng').val(newMarker.getPosition().lng());
    $('#MissingId').val(postClueId);

    let formData = new FormData($('#clueForm')[0]);
    let hasEmpty = false;

    if (formData.get('Image').size == 0) {
        hasEmpty = true;
    }

    for (let pair of formData.entries()) {
        if (pair[1] == '') {
            hasEmpty = true;
            break;
        }
    }

    if (hasEmpty) {
        Swal.fire({
            heightAuto: false,
            position: 'center',
            icon: 'warning',
            title: '您有未填的項目',
            showConfirmButton: false,
            timer: 2000
        })
    } else {
        $.ajax({

            url: '/Missings/PostClue',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {

                if (data == "OK") {

                    $('#cancel-publish').trigger('click');

                    Swal.fire({
                        heightAuto: false,
                        position: 'center',
                        icon: 'success',
                        title: '線索提交成功',
                        showConfirmButton: false,
                        timer: 2500
                    })
                }
            }
        })
    }


    e.preventDefault();

})