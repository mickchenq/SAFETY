let _GetQueryUrl = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return decodeURI(results[1]) || 0;
    }
}
/**
 * 時間轉字串
 * @param {any} date
 */
let _TrsDateToStr = function (date) {
    console.log(date)
    let month = (date.getMonth() + 1).toString()
    let day = date.getDate().toString()
    if ((date.getMonth() + 1) < 10) {
        month = '0' + month
    }
    if (date.getDate() < 10) {
        day = '0' + day
    }
    return date.getFullYear() + '-' + month + '-' + day;
}
let _CheckOverZero = function (val) {
    return (!isNaN(val) && parseFloat(val) > 0)
}

let _AlertMsg = function (isSuccess, msg, callback) {
    Swal.fire({
        position: 'top-center',
        //title: isSuccess ? '成功' : '注意',
        title: isSuccess ? localizer_sucessed : localizer_warning,
        html: msg,
        icon: isSuccess ? 'success' : 'warning',
        timer: 2000,
        showConfirmButton: false,
        showCloseButton: true,
        willClose: callback
    })
}

let _ConfirmAlert = function (type = 'i', title = '', content = '', showCancel = false, successCallBack, cancelCallBack) {
    var myicon = 'info'
    switch (type) {
        case 's':
            myicon = 'success';
            break;
        case 'e':
            myicon = 'error';
            break;
        case 'w':
            myicon = 'warning';
            break;
        case 'i':
            myicon = 'info';
            break;
        case 'q':
            myicon = 'question';
            break;
        default:
    }
    console.log(myicon)
    Swal.fire({
        title: title,
        html: content,
        icon: myicon,
        showCancelButton: showCancel,
        //confirmButtonText: '確定',
        //cancelButtonText: '取消'
        confirmButtonText: localizer_confirm,
        cancelButtonText: localizer_cancel
    }).then(
        function (result) {
            console.log(result)
            if (result.isConfirmed) {
                //successCallBack();
                //使用者按下「確定」要做的事
                if (typeof successCallBack === 'function') {
                    // do something
                    successCallBack();
                }
            } else if (result.dismiss === "cancel") {
                //使用者按下「取消」要做的事
                if (typeof cancelCallBack === 'function') {
                    // do something
                    cancelCallBack();
                }
            }
        }.bind(this));
}