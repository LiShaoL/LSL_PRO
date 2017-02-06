var LSL_accreditB = [];
$(function () {

    LSL_accredit.getJstreeData();
    LSL_accredit.loadEvent();
});
LSL_accredit = {
    getJstreeData: function () {
        LSLCommon.jQajax("post", "/RolesPermission/GetJstree", {}, function (data) { LSL_accredit.callBack(data); }, true);
    },
    callBack: function (data) {
        $("#Roletree").jstree({
            "plugins": ["wholerow", "checkbox"],
            "core": {
                "data": data
            }
        }).bind("loaded.jstree", function (e, datas) {
            datas.instance.open_node($("li[aria-level='1']"));
            var id = LSLCommon.getQueryString("RoleId");//获取RoleId
            LSLCommon.jQajax("post", "/RolesPermission/GetRoleMenu", { roleid: id }, function (data) { LSL_accredit.RoleMenucallBack(data, e, datas); }, true);
        })
    },
    btnSave: function () {
        if (LSL_accreditB.length != 0) {
            var id = LSLCommon.getQueryString("RoleId");//获取RoleId
            LSLCommon.jQajax("post", "/RolesPermission/AddAccredit", { roleid: id, mid: LSL_accreditB }, function (data) { LSL_accredit.AddcallBack(data); }, true, true);
            LSL_accreditB = [];
        }
    },
    btnClose: function () {
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index);
    },
    AddcallBack: function (data) {
        layer.msg(data.msg);
    },
    //搞来搞去弄不好啊MDZZ！！
    loadEvent: function () {
        $("#Roletree").on("changed.jstree", function (e, data) {
            var i, j;
            for (i = 0, j = data.selected.length; i < j; i++) {
                var node = data.instance.get_node(data.selected[i]);
                if (data.instance.is_leaf(node)) {
                    if ($.inArray(node.parent, LSL_accreditB) == -1) {
                        LSL_accreditB.push(node.parent);
                    }
                    if ($.inArray(node.id, LSL_accreditB) == -1) {
                        LSL_accreditB.push(node.id);
                    }

                }
            }
        })
        $("#Roletree").on("deselect_node.jstree", function (e, data) {
            var i, j;
            LSL_accreditB = [];
            for (i = 0, j = data.selected.length; i < j; i++) {
                var node = data.instance.get_node(data.selected[i]);
                if (data.instance.is_leaf(node)) {
                    if ($.inArray(node.parent, LSL_accreditB) == -1) {
                        LSL_accreditB.push(node.parent);
                    }
                    if ($.inArray(node.id, LSL_accreditB) == -1) {
                        LSL_accreditB.push(node.id);
                    }

                }
            }

        })
    },

    RoleMenucallBack: function (data, e, datas) {
        if (data != null) {
            var inst = datas.instance;
            for (var i in data) {
                if (data[i].MenuId != '3885ba7f-c246-493f-9053-7aa70a642662') {
                    var obj = inst.get_node(data[i].MenuId);
                    inst.select_node(obj);
                }              
            }
        }
    }

}
