//lsl 2016.12.08
$(function () {
    LSL_index.GetMenu();
    $.sidebarMenu($('.sidebar-menu'));
    $("#tabs").addtabs({
        contextmenu: true,
        iframeHeight: "825px"
    });

});
var LSL_index = {
    json:"",
    GetMenu: function () {
        LSLCommon.jQajax("post", "/Index/IndexP", {}, function (data) { LSL_index.callBack(data); }, true);
    },
    callBack: function (data) {
        //var json = this.json = eval('(' + data + ')');
        var json = this.json = data;
        var html = "";
        $.each(json, function (i) {
            if (json[i].ParentId == '#') {
                html += "<li class=\"treeview\"><a href=\"#\"><i class=\"fa " + json[i].Img + "\" aria-hidden=\"true\"></i><span>" + json[i].FullName + "</span></a>";
                html +=LSL_index.GetSubmenu(json[i].MenuId);
            }          
        });
        html += "</li>"
        $(".sidebar-menu").append(html);      
    },
    GetSubmenu: function (menuid) {
        var html = "<ul class=\"treeview-menu\">";
        var submenu = this.json;
        $.each(submenu, function (i) {
            if (submenu[i].ParentId == menuid) {
                html += "<li><a href=\"#\" data-addtab=\"userMenu" + i + "\" url=\"" + submenu[i].NavigateUrl + "?MenuId=" + submenu[i].MenuId + "\"><i class=\"fa " + submenu[i].Img + "\"></i><span class=\"text\"> " + submenu[i].FullName + "</span></a></li>";
            }               
        });
        html += "</ul>";
        return html;
    }

}