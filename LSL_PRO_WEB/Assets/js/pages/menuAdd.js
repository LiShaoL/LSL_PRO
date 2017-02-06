$(function () {
    LSL_menuAdd.getMenulidt();
    var id = LSLCommon.getQueryString("MenuId");
    if (id != null) {
        LSLCommon.jQajax("post", "/Menu/GetMenuInfo", { id: id }, function (data) { LSL_menuAdd.UpdatecallBack(data); }, false);
    }
    LSL_menuAdd.ShowMenu();

});
var LSL_menuAdd = {
    btnBack: function () {
        history.back();
    },
    btnSave: function () {
        LSLCommon.jQajax("post", "/Menu/SaveInfo", $(".form-horizontal").serialize(), function (data) { LSL_menuAdd.AddcallBack(data); }, true);
    },
    AddcallBack: function (data) {
        layer.msg(data.msg);
    },
    UpdatecallBack: function (data) {
        if (data.code == "-1") {
            layer.msg(data.msg);
            return;
        } else {            
            $("#menuID").val(data.result.MenuId);
            $("#menuName").val(data.result.FullName);
            $("#ImgName").val(data.result.Img);
            $("#category").get(0).value = data.result.Category;
            $("#url").val(data.result.NavigateUrl);
            $("#topmenu").get(0).value = data.result.ParentId;
        }
    },
    ShowMenu: function () {
        var v = $("#category option:selected").val();
        if (v == "") {
            $("#tm").css("display", "");
            $("#lj").css("display", "");
        }
        if (v == "模块") {
            $("#tm").css("display", "none");
            $("#lj").css("display", "none");
        }
        if(v=="菜单"){
            $("#tm").css("display", "");
            $("#lj").css("display", "");
        }
    },
    getMenulidt: function () {
        LSLCommon.jQajax("post", "/Menu/GetTopMenu", { }, function (data) { LSL_menuAdd.topcallBack(data); }, false);
    },
    topcallBack: function (data) {
        for (var key in data) {
            $("#topmenu").append("<option value='" + data[key]["MenuId"] + "'>" + data[key]["FullName"] + "</option>");
        }
    }

}