$(function () {
    LSL_BtnAccredit.getJstreeData();
    LSL_BtnAccredit.loadEvent();
});
LSL_BtnAccredit = {
    MenuId:"",
    getJstreeData: function () {
        var id = LSLCommon.getQueryString("RoleId");//获取RoleId
        LSLCommon.jQajax("post", "/RolesPermission/GetMenuJstree", { roleid: id }, function (data) { LSL_BtnAccredit.callBack(data); }, true);
    },
    callBack: function (data) {
        $("#Roletree").jstree({
            "plugins": ["wholerow"],
            "core": {
                "data": data
            }
        }).bind("loaded.jstree", function (e, datas) {
            datas.instance.open_node($("li[aria-level='1']"));
        })
    },
    loadEvent: function () {
        var id = LSLCommon.getQueryString("RoleId");//获取RoleId
        $("#Roletree").on("changed.jstree", function (e, data) {
            var i, j;
            for (var i in data.selected) {
                var node = data.instance.get_node(data.selected[i]);
                if (node != false) {
                    LSL_BtnAccredit.MenuId = node.id;
                    $("#cb").remove();
                    LSLCommon.jQajax("post", "/RolesPermission/GetMenuBtn", { mid: node.id }, function (datas) { LSL_BtnAccredit.AddBtn(datas); }, false);
                    LSLCommon.jQajax("post", "/RolesPermission/GetRoleMenuBtn", { roleid:id,mid: node.id }, function (datass) { LSL_BtnAccredit.setBtn(datass); }, false);
                }
            }
        })
    },
    AddBtn: function (data) {
        var html = "";
        html += "<div id=\"cb\">"
        $.each(data, function (i) {
            html += "<div class=\"checkbox-custom checkbox-inline\"><input type=\"checkbox\" id=\"" + data[i].ButtonId + "\" name=\"btnbox\" value=\"" + data[i].ButtonId + "\"><label for=\"inline-checkbox1\">" + data[i].FullName + "</label></div>"
        });
        html += "</div>"
        $("#forma").append(html);
    },
    btnClose: function () {
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index);
    },
    btnSave: function () {
        var id = LSLCommon.getQueryString("RoleId");//获取RoleId
        var chk_value = [];
        $('input[name="btnbox"]:checked').each(function () {
            chk_value.push($(this).val());
        });
        LSLCommon.jQajax("post", "/RolesPermission/SaveRoleMenuBtn", { roleid: id, mid: this.MenuId, btnid: chk_value }, function (data) { LSL_BtnAccredit.saveBack(data); }, true, true);
    },
    saveBack: function (data) {
        layer.msg(data.msg);
    },
    setBtn: function (data) {
        for (var i in data) {
            $("#" + data[i].ButtonId + "").prop('checked', true);
        }
    }
}