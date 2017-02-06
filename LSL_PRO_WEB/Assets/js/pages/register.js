/*lsl 2016.12.02*/
$(function () {
    $("#regi").prop("disabled", true);
    $("#AgreeTerms").change(function () {
        if ($(this).prop("checked")) {
            $("#regi").prop("disabled", false);
        } else { $("#regi").prop("disabled", true); }
    });
    $("#name").blur(function () {
        var name = $(this).val();
        var email = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
        var phone = /^1(3|4|5|7|8)\d{9}$/;
        if (name == "") {
            $("#un").html("用户名不能为空").attr("style", "color:red");
            //layer.tips('用户名不能为空', '#name', { tips:[2,'red'] });
            return;
        }
        else if (!email.test(name) && !phone.test(name)) {
            $("#un").html("请输入正确的邮箱或电话号码").attr("style", "color:red");
            layer.tips('用户名不能为空', '#name');
            return;
        } else {
            LSLCommon.jQajax("post", "/Register/RegisterPU", { name: name }, function (data) { LSL_register.isRegister(data); }, true);

        }
    });
    $("#pwd").keyup(function () {
        var strongRegex = new RegExp("^(?=.{8,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
        var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
        var enoughRegex = new RegExp("(?=.{6,}).*", "g");

        if (false == enoughRegex.test($(this).val())) {
            $('#qd').removeClass('pw-weak');
            $('#qd').removeClass('pw-medium');
            $('#qd').removeClass('pw-strong');
            $('#qd').addClass(' pw-defule');
            //密码小于六位的时候，密码强度图片都为灰色 
        }
        else if (strongRegex.test($(this).val())) {
            $('#qd').removeClass('pw-weak');
            $('#qd').removeClass('pw-medium');
            $('#qd').removeClass('pw-strong');
            $('#qd').addClass(' pw-strong');
            //密码为八位及以上并且字母数字特殊字符三项都包括,强度最强 
        }
        else if (mediumRegex.test($(this).val())) {
            $('#qd').removeClass('pw-weak');
            $('#qd').removeClass('pw-medium');
            $('#qd').removeClass('pw-strong');
            $('#qd').addClass(' pw-medium');
            //密码为七位及以上并且字母、数字、特殊字符三项中有两项，强度是中等 
        }
        else {
            $('#qd').removeClass('pw-weak');
            $('#qd').removeClass('pw-medium');
            $('#qd').removeClass('pw-strong');
            $('#qd').addClass('pw-weak');
            //如果密码为6为及以下，就算字母、数字、特殊字符三项都包括，强度也是弱的 
        }
        return true;
    }).blur(function () {
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
            $("#mm").html("密码强度").attr("style", "color:green");
        }
    });
    $("#pwd_confirm").blur(function () {
        var pwd_confirm = $(this).val();
        if (pwd_confirm == "") {
            $("#qrmm").html("再次输入密码不能为空").attr("style", "color:red");
            return;
        }
        else if (pwd_confirm != $("#pwd").val()) {
            $("#qrmm").html("两次输入密码不一致").attr("style", "color:red");
            return;
        } else {
            $("#qrmm").html("密码正确").attr("style", "color:green");
        }
    });
});
var LSL_register = {
    callBack: function (data) {
        if (LSLCommon.successCallBack(data)) {
            window.open("/Login/Login", "_self");
        } else {
            return;
        }
    },
    Register: function () {
        LSLCommon.jQajax("post", "/Register/RegisterP", $("#form_register").serialize(), function (data) { LSL_register.callBack(data); }, false);
    },
    Notice: function () {
        layer.open({
            type: 1,
            title: false, //不显示标题栏
            closeBtn: false,
            area: ['400px', '400px'],
            shade: 0.8,
            id: 'LAY_layuipro', //设定一个id，防止重复弹出,
            resize: false,
            btn: ['好的呀'],
            btnAlign: 'c',
            moveType: 1, //拖拽模式，0或者1, 
            content: '<div style="padding: 150px; line-height: 25px; background-color: #393D49; color: #fff; font-weight: 300;">1.作者LSL<br>2.QQ:2628730957</div>',
        });
    },
    isRegister: function (data) {
        if (data.code == "1") {
            $("#un").html(data.msg).attr("style", "color:green");
        } else {
            $("#un").html(data.msg).attr("style", "color:red");
        }

    }

}
