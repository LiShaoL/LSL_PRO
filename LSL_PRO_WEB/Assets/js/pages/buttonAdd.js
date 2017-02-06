$(function () {
    var id = LSLCommon.getQueryString("ButtonId");
    if (id != null) {
        LSLCommon.jQajax("post", "/Button/GetButtonInfo", { id: id }, function (data) { LSL_btnAdd.UpdatecallBack(data); }, true);
    } 
});
var LSL_btnAdd = {
    btnBack: function () {
        history.back();
    },
    btnSave: function () {
        LSLCommon.jQajax("post", "/Button/SaveInfo", $(".form-horizontal").serialize(), function (data) { LSL_btnAdd.AddcallBack(data); }, true);
    },
    AddcallBack: function (data) {
        layer.msg(data.msg);
    },
    UpdatecallBack: function (data) {
        if (data.code == "-1") {
            layer.msg(data.msg);
            return;
        } else {
            $("#BtnID").val(data.result.ButtonId);
            $("#BtnName").val(data.result.FullName);
            $("#ImgName").val(data.result.Img);
            $("#Event").val(data.result.Event);
            $("#CSS").val(data.result.Description);
        }
    }
}