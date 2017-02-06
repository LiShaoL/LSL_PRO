var LSLCommon = window.LSLCommon = LSLCommon || {}
$.extend(LSLCommon, {
    //jQuery ajax 
    jQajax: function (type,url,data,callback,bool,booll) {
        $.ajax({
            type: type,
            url: url,
            data: data,
            async: bool,
            traditional: booll,
            success: function (data) {
                callback(data);
            }
        });
    },
    successCallBack: function (data) {
        if (data.code == "-1") {
            layer.alert(data.msg);
            return false;
        } else {
            layer.alert(data.msg);
            return true;
        }       
    },
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

});
Array.prototype.removeByValue = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            this.splice(i, 1);
            break;
        }
    }
}