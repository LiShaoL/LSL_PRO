//lsl 2016 12.14
$(function () {
    LSL_user.GetButton();
    LSL_user.GetJqList();
});
var LSL_user = {
    GetJqList: function () {
        $("#jqGridList").jqGrid({
            //caption: "用户管理",
            url: "/User/UserGetList",
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
            //multiselect: false,
            //multiselectWidth:35,
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
    GetButton: function () {
        var id = LSLCommon.getQueryString("MenuId");
        LSLCommon.jQajax("post", "/User/GetBtn", {mid:id}, function (data) { LSL_user.callBack(data); }, true);
    },
    callBack: function (data) {
        var json = data;
        var html = "";
        $.each(json, function (i) {
            html += " <button type=\"button\" onclick=\"" + json[i].Event + "\" class=\"bk-margin-5 btn " + json[i].Description + "\"><i class=\"fa " + json[i].Img + "\"></i>&nbsp;" + json[i].FullName + "</button>"
        });
        $("#getbtn").append(html);
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
    DelcallBack: function (data) {
        layer.msg(data.msg);
        $("#jqGridList").trigger("reloadGrid", [{ page: 1 }]);
    }

}
//增加方法
function Add() {
    window.location.href = "/User/UserAdd";
}
//修改方法
function Edit() {
    var id = $("#jqGridList").jqGrid('getGridParam', "selrow");
    if (id == null) {
        layer.alert("Sorry，没有选中任何数据！", { icon: 0 });
        return;
    } else {
        var celldata = $("#jqGridList").jqGrid('getRowData', id);
        window.location.href = "/User/UserAdd?UserId=" + celldata.UserId + "";
    }
}
//删除方法
function Delete() {
    var id = $("#jqGridList").jqGrid('getGridParam', "selrow");
    if (id == null) {
        layer.alert("Sorry，没有选中任何数据！", { icon: 0 });
        return;
    } else {
        layer.confirm('您确定要删除？',
            {
                icon: 2,
                btn: ['确定', '取消'] //按钮
            }, function () {
                var celldata = $("#jqGridList").jqGrid('getRowData', id);
                LSLCommon.jQajax("post", "/User/Delete", { id: celldata.UserId }, function (data) { LSL_user.DelcallBack(data); }, true);
            }, function () {
                return;
            });
    }
}

