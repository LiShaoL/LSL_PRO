$(function () {
    var id = LSLCommon.getQueryString("UserId");
    if (id != null) {
        LSLCommon.jQajax("post", "/User/GetUserInfo", { id: id }, function (data) { LSL_userAdd.UpdatecallBack(data); }, true);
    } else {
    $("#LoginName").blur(function () {
        var name = $(this).val();
        var email = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
        var phone = /^1(3|4|5|7|8)\d{9}$/;
        if (name == "") {
            $("#un").html("用户名不能为空").attr("style", "color:red");
            return;
        }
        else if (!email.test(name) && !phone.test(name)) {
            $("#un").html("请输入正确的邮箱或电话号码").attr("style", "color:red");
            return;
        } else {
            LSLCommon.jQajax("post", "/Register/RegisterPU", { name: name }, function (data) { LSL_userAdd.isRegister(data); }, true);

        }
    });
    $("#Password").blur(function () {
        var pwd = $(this).val();
        var pa = /^(?![^a-zA-Z]+$)(?!\D+$).{6,16}$/
        if (pwd == "") {
            $("#mm").html("密码不能为空").attr("style", "color:red");
            return;
        }
        else if (!pa.test(pwd)) {
            $("#mm").html("请输入6-16位密码,由“数字”“字母”组成").attr("style", "color:red");
            return;
        } else {
            $("#mm").html("密码可用").attr("style", "color:green");
        }
    });
    $("#ConfirmPwd").blur(function () {
        var pwd_confirm = $(this).val();
        if (pwd_confirm == "") {
            $("#qrmm").html("再次输入密码不能为空").attr("style", "color:red");
            return;
        }
        else if (pwd_confirm != $("#Password").val()) {
            $("#qrmm").html("两次输入密码不一致").attr("style", "color:red");
            return;
        } else {
            $("#qrmm").html("密码正确").attr("style", "color:green");
        }
    });
    }
});
var LSL_userAdd = {
    btnBack: function () {
        history.back();
    },
    isRegister: function (data) {
        if (data.code == "1") {
            $("#un").html(data.msg).attr("style", "color:green");
        } else {
            $("#un").html(data.msg).attr("style", "color:red");
        }
    },
    btnSave: function () {
        LSLCommon.jQajax("post", "/User/SaveInfo", $(".form-horizontal").serialize(), function (data) { LSL_userAdd.AddcallBack(data); }, true);
    },
    AddcallBack: function (data) {
        layer.msg(data.msg);
    },
    UpdatecallBack: function (data) {
        if (data.code == "-1") {
            layer.msg(data.msg);
            return;
        } else {
            $("#LoginName").val(data.result.Account);
            $("#Password").val(data.result.Password);
            $("#ConfirmPwd").val(data.result.Password);
            $("#LoginName").attr("readonly", true);
            $("#Password").attr("readonly", true);
            $("#ConfirmPwd").attr("readonly", true);
        }
       
    }
}