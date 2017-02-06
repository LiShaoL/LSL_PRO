$(function () {
    var id = LSLCommon.getQueryString("RoleId");
    if (id != null) {
        LSLCommon.jQajax("post", "/Role/GetRoleInfo", { id: id }, function (data) { LSL_roleAdd.UpdatecallBack(data); }, false);
    }
});
var LSL_roleAdd = {
    btnBack: function () {
        history.back();
    },
    btnSave: function () {
        LSLCommon.jQajax("post", "/Role/SaveInfo", $(".form-horizontal").serialize(), function (data) { LSL_roleAdd.AddcallBack(data); }, true);
    },
    AddcallBack: function (data) {
        layer.msg(data.msg);
    },
    UpdatecallBack: function (data) {
        if (data.code == "-1") {
            layer.msg(data.msg);
            return;
        } else {
            $("#RoleId").val(data.result.RoleId);
            $("#RoleName").val(data.result.FullName);
            $("#lx").get(0).value = data.result.Category;
        }
    }

}