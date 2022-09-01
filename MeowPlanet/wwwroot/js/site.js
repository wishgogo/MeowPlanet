$('.nav-item a').click(function () {
    $('.nav-item a').removeClass('nav-item-active');
    $(this).addClass('nav-item-active');
})

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