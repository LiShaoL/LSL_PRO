//lsl 2016.1.6
$(function () {  
    LSL_menubtn.GetButton();
    var id = LSLCommon.getQueryString("MenuId");
    setTimeout(LSL_menubtn.setCheckbox(id),2000);
});
var LSL_menubtn = {
    GetButton: function () {
        LSLCommon.jQajax("post", "/Button/GetBtn", {}, function (data) { LSL_menubtn.callBack(data); }, false);
    },
    callBack: function (data) {
        var html = "";
        $.each(data, function (i) {
            html += "<div class=\"col-md-3 col-sm-3\"><input type=\"checkbox\" id=\"" + data[i].ButtonId + "\" name=\"btnbox\" value=\"" + data[i].ButtonId + "\" title=\"" + data[i].FullName + "\"></div>";
        });
        $(".layui-form-item").append(html);
       
    },
    btnSave: function () {
        var id = LSLCommon.getQueryString("MenuId");
        var chk_value = "";    
        $('input[name="btnbox"]:checked').each(function () {
            chk_value += $(this).val()+",";
        });
        LSLCommon.jQajax("post", "/Menu/AddMenuBtn", { id: id, btnid: chk_value }, function (data) { LSL_menubtn.saveBack(data); }, true);
    },
    saveBack: function (data) {
        layer.msg(data.msg);
    },
    btnClose: function () {
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index);
    },
    setCheckbox: function (id) {
        LSLCommon.jQajax("post", "/Menu/GetMenuBtn", { id: id }, function (data) { LSL_menubtn.setBack(data); }, false);
    },
    setBack: function (data) {
        for (var i in data) {
            $("#" + data[i].ButtonId + "").prop('checked', true);
        }
    }
}