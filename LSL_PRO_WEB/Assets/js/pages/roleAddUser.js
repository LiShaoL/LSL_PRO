//lsl 2017.1.3
$(function () {
    var id = LSLCommon.getQueryString("RoleId");
    if (id != null) {
        LSL_roleAdduser.GetJqList();
    }
});
var LSL_roleAdduser = {
    GetJqList: function () {
        $("#jqGridList").jqGrid({
            //caption: "用户管理",
            url: "/RolesPermission/UserGetList?RoleId=" + LSLCommon.getQueryString("RoleId"),
            mtype: "post",
            styleUI: 'Bootstrap',//设置jqgrid的全局样式为bootstrap样式
            datatype: "json",
            colNames: ['主键', '登录帐号', '姓名', '性别', '邮箱', '电话', '创建时间'],
            colModel: [
    { name: 'UserId', index: 'UserId', width: 60, hidden: true },
    { name: 'Account', index: 'Account', width: 60 },
    { name: 'RealName', index: 'RealName', width: 60 },
    {
        name: 'Gender', index: 'Gender', width: 60,
        formatter: function (cellValue) {
            return cellValue == 0 ? "男" : "女";
        }//jqgrid自定义格式化
    },
    { name: 'Email', index: 'Email', width: 60 },
    { name: 'Mobile', index: 'Mobile', width: 60 },
    { name: 'CreateDate', index: 'CreateDate', width: 60, formatter: 'date', formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } }
            ],
            sortorder: "desc",
            sortname: 'CreateDate',
            viewrecords: true,
            multiselect: true,
            multiselectWidth:35,
            rownumbers: true,
            autowidth: true,
            height: "100%",
            width: "100%",
            rowNum: 10,
            rowList: [10, 20, 30],
            rownumbers: true, // 显示行号
            rownumWidth: 35, // the width of the row numbers columns
            pager: "#jqGridPager",//分页控件的id
            subGrid: false,//是否启用子表格
            gridComplete: function () {
                var ret = $("#jqGridList").jqGrid("getRowData");
                if (ret.length == 0) {
                    layer.alert("Sorry，没有数据！", { icon: 0 });
                }
                
            }
        });
        $("#jqGridList").jqGrid('navGrid', '#jqGridPager', { edit: false, add: false, del: false, search: false });
    },
    Search: function () {
        var datas = {
            strname: $("#tiaojian option:selected").val(),
            str: $("#txtSearchKey").val()
        }
        var postData = $("#jqGridList").jqGrid("getGridParam", "postData");
        $.extend(postData, datas);
        $("#jqGridList").jqGrid("setGridParam").trigger("reloadGrid", [{ page: 1 }]);	// 重新载入Grid表格
    },
    btnSave: function () {
        var ids = "";
        var roleid = LSLCommon.getQueryString("RoleId");
        var id = $("#jqGridList").jqGrid('getGridParam', 'selarrrow');
        if (id.length==0) {
            layer.alert("Sorry，没有选中任何数据！", { icon: 0 });
            return;
        } else {
            for (var i in id) {
                var celldata = $("#jqGridList").jqGrid('getRowData', id[i]);
                ids += celldata.UserId + ',';
            }
            LSLCommon.jQajax("post", "/RolesPermission/AddRoleUser", { roleid: roleid, uid: ids }, function (data) { LSL_roleAdduser.callBack(data); }, true);
        }
    },
    btnBack:function(){
        history.back();
    },
    callBack: function (data) {
        layer.msg(data.msg);
        $("#jqGridList").trigger("reloadGrid");
    }
}