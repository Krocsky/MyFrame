$(function () {
    layer.config({
        path: '/Scripts/layer/',
        scrollbar: false,
        area: "auto",
        maxWidth: "1500"
    });

    $.ajaxSetup({ cache: false });
    $(document).on("click", ".deletebutton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        layer.confirm('确认要删除吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                if (data.Status == 1) {
                    $this.closest("tr").slideUp();
                    $this.closest("div.attachmentbox").slideUp();
                }
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });

    $("textarea[plugin='ueditor']").livequery(function () {
        var id = $(this).attr("id");
        var data = $.parseJSON($(this).attr("data"));
        ue = UE.getEditor(id);
        ue.ready(function () {
            ue.execCommand('serverparam', data);
        });
    });

    $(document).on("click", ".freezebutton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        layer.confirm('确认要设为黑名单吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                if (data.Status == 1) {
                    $this.closest("div.attachmentbox").slideUp();
                }
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });

    $(document).on("click", ".refreezeButton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        layer.confirm('确认要解除黑名单吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                if (data.Status == 1) {
                    $this.closest("div.attachmentbox").slideUp();
                }
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });

    $(document).on("click", ".rollBackButton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        layer.confirm('确认要恢复吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                if (data.Status == 1) {
                    $this.closest("div.attachmentbox").slideUp();
                }
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });

    $(document).on("click", "[dialog]", function (e) {
        e.preventDefault();
        var maxW = "500";
        //修复可以多次弹层
        var thisDom = $(this);
        thisDom.attr("disabled", "disabled");

        if ($(this).data("width") != undefined) {
            maxW = $(this).data("width");
        }

        var area = "auto";
        if ($(this).data("area") != undefined) {
            area = eval($(this).data("area"));
        }

        var loading = layer.load(0, { shade: false });
        var $this = $(this);
        var url = $this.attr("href");
        $.get(url, function (data) {
            layer.close(loading);
            var index = layer.open({
                type: 1,
                title: window.title,
                maxWidth: maxW,
                id: generateUUID(),
                area: area,
                content: data,
                zIndex: 2000,
                success: function (layero, index) {
                    $.validator.unobtrusive.parse(document);
                    thisDom.removeAttr("disabled");
                }
            });
            layer.title(window.title, index)
        });
    });

    $(document).on("click", "[cancel]", function (e) {
        e.preventDefault();
        layer.closeAll();
    });

    $(document).on("click", ".pagination[data-ajax] li a", function (e) {
        e.preventDefault();
        var target = $(this).closest("ul").data("target");
        var url = $(this).attr("href");
        $.get(url, function (data) {
            $(target).html(data);
        })
    });

    $(document).on("click", ".bootstrap-table table tr,.table tr", function (e) {
        if (e.target.type == "checkbox") {
            return;
        }
        var input = $(this).find("td").first().find("input[type='checkbox']");
        if (input[0] && input.attr("id") != "select-all") {
            if (input.prop("checked") == true) {
                input.prop("checked", false);

                if (input.is('.tn-giftsList-checkbox')) {
                    var giftId = input.val();
                    var buttonId = "SelectSuccessGift_" + giftId;
                    $("#" + buttonId).remove();
                }
                if (input.is('.tn-goodsList-checkbox')) {
                    var goodsId = input.val();
                    var buttonId = "SelectSuccessGoods_" + goodsId;
                    $("#" + buttonId).remove();
                }

            } else {
                input.prop("checked", true);

                if (input.is('.tn-giftsList-checkbox')) {
                    var giftId = input.val();
                    var buttonId = "SelectSuccessGift_" + giftId;
                    var giftNumber = $("#GiftNumber_" + giftId).text();
                    var number = $("#SelectNumber_" + giftId).val();

                    if (parseInt(number) <= 0 || number == "") {
                        input.prop("checked", false);
                        layer.msg("请输入赠品数量！", { icon: 0, time: 2000 });
                        return;
                    }
                    if (parseInt(number) > parseInt(giftNumber)) {
                        input.prop("checked", false);
                        layer.msg("所需赠品数量不足！", { icon: 0, time: 2000 });
                        return;
                    }
                    var name = $("#GoodsName_" + giftId).text() + "*" + number;
                    $("#" + buttonId).remove();
                    $("#SelectGiftsSuccess").append("<button name=\"giftsIds\" style=\"margin:2px;\" class=\"btn btn-success btn-group btn-removegift\" type=\"button\" Id=\"" + buttonId + "\"  data-giftid=\"" + giftId + "\" data-number=\"" + number + "\">" + name + "</button>");

                }
                if (input.is('.tn-goodsList-checkbox')) {
                    var goodsId = input.val();
                    var buttonId = "SelectSuccessGoods_" + goodsId;
                    var giftNumber = $("#GoodsNumber_" + goodsId).text();
                    var number = $("#SelectNumber_" + goodsId).val();
                    var price = $("#GoodsPrice_" + goodsId).text();
                    if (parseInt(number) <= 0 || number == "") {
                        input.prop("checked", false);
                        layer.msg("请输入商品数量！", { icon: 0, time: 2000 });
                        return;
                    }
                    if (parseInt(number) > parseInt(giftNumber)) {
                        input.prop("checked", false);
                        layer.msg("所需商品数量不足！", { icon: 0, time: 2000 });
                        return;
                    }
                    var totalPrice = Number(price) * Number(number);
                    var name = $("#GoodsName_" + goodsId).text() + "*" + number;
                    $("#" + buttonId).remove();
                    $("#SelectGoodsSuccess").append("<button name=\"goodsIds\" style=\"margin:2px;\" class=\"btn btn-success btn-group btn-removegoods\" type=\"button\" Id=\"" + buttonId + "\"  data-goodsid=\"" + goodsId + "\" data-number=\"" + number + "\" data-price=\"" + totalPrice + "\">" + name + "</button>");
                }
            }
        }
    });

    //用户选择器
    $("input.userSelector").livequery(function () {
        $(this).bsSuggest({
            url: $(this).data("url") + "?key=",
            allowNoKeyword: false,
            getDataMethod: "url",
            showBtn: false,
            idField: "Id",
            keyField: "Name",
            autoSelect: true,
            effectiveFields: ['Name', 'Phone', 'MotherPhone', 'FatherPhone'],
            effectiveFieldsAlias: { Name: "姓名", Phone: "电话", MotherPhone: "母亲电话", FatherPhone: "父亲电话" },
            processData: function (json) {
                var index, len, data = { value: [] };
                if (!json || json.length === 0) {
                    return false;
                }
                len = json.length;
                for (index = 0; index < len; index++) {
                    data.value.push(json[index]);
                }
                return data;
            }
        }).on('onSetSelectValue', function (e, keyword, data) {
            $(this).next().val(data.Id);
            //退费学生选择
            if (window.location.pathname == '/EducationAffair/ManageRefound') {
                RefoundSelect();
            }
        });
    });

    if (navigator.userAgent.toLowerCase().indexOf("chrome") >= 0) {
        $(window).load(function () {
            $('input:-webkit-autofill').each(function () {
                var name = $(this).attr('name');
                $(this).after(this.outerHTML).remove();
                $('input[name=' + name + ']').val('');
            });
        });
    }

    var contentheight = $(".top-content").height();
    var documentheight = $(document).height();
    var headerheight = $("header.header").height();
    var footerheight = $("footer#footer").height();
    var htmlheight = contentheight + headerheight + footerheight;
    if (documentheight > htmlheight) {
        $(".top-content").css("height", (documentheight - headerheight - footerheight));
    }

    $(window).resize(function () {
        headerheight = $("header.header").height();
        footerheight = $("footer#footer").height();
        if (documentheight > htmlheight) {
            $(".top-content").css("height", (documentheight - headerheight - footerheight));
        }
    });

    $("input[type='file']").on('fileuploaded', function (event, data, previewId, index) {
        window.location.reload();
    });

    $(".J_menuItem").on("click", function () {
        $(this).parent("li:not('.profile')").addClass("second-level-active");
        $(".J_menuItem").not($(this)).parent("li").removeClass("second-level-active");
    });

    $(".J_menuTabs").on("click", ".J_menuTab", menuHighLight);

    function menuHighLight() {
        var url = $(this).data("id");
        $(".J_menuItem").parent("li:not('.profile')").removeClass("second-level-active");
        $(".J_menuItem[href='" + url + "']").parent("li:not('.profile')").addClass("second-level-active");
    }

    //$(function () {
    //    $("#side-menu > li:not('.nav-header'):eq(0)").addClass("second-level-active");
    //});


    $(document).on("click", ".closebutton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        layer.confirm('确认要废弃吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });


    $(document).on("click", ".operatebutton", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        var text = $this.text().trim();
        layer.confirm('确认要' + text + '吗？', { icon: 3, title: '提示' }, function (index) {
            $.post(url, function (data) {
                layer.close(index);
                layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                    window.location.reload();
                });
            });
        });
    });

    $("li.dropdown.username").hover(function () {
        $(this).children("ul").css("display", "block");
    }, function () {
        $(this).children("ul").css("display", "none");
    });

    $(".navbar-minimalize").click(function () {
        SmoothlyNavSchool();
    });

    //选择学生-全选
    $(document).on("click", "#select-student-all", function () {
        if (this.checked) {
            $(".tn-studentcheckbox").each(function () {
                this.checked = true;
            });
        }
        else {
            $(".tn-studentcheckbox").each(function () {
                this.checked = false;
            });
        }
    });

    //公开课-查询
    $(document).on("click", "#btnSelectForOpenClass", function (e) {

        var $this = $(this);
        var url = $this.attr("href");
        var openClassTime = $("#openClassTime").val();
        $.get(url, { endDate: openClassTime }, function (data) {
            $("#OpenClassList").html(data);
        });
    });
    //公开课-全部
    $(document).on("click", "#btnSelectAllForOpenClass", function (e) {
        var $this = $(this);
        var url = $this.attr("href");
        $.get(url, function (data) {
            $("#OpenClassList").html(data);
        });
    });

    //公开课 - 选择
    $(document).on("click", ".btn-InvitationChoose", function (e) {
        var $this = $(this);
        var url = $this.attr("href");
        var userId = $("#InvitationUserId").val();
        var openClassId = $(this).attr("data-id");

        $.post(url, { openClassId: openClassId, userId: userId }, function (data) {
            layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                if (data.Status == 1) {
                    layer.close($("#InvitationIndex").val());
                }
            });
        });
    });

    //入班试听-查询
    $(document).on("click", "#btnSelectForAudition", function (e) {

        var $this = $(this);
        var url = $this.attr("href");
        var classId = $("#classId").val();
        var lessonDate = $("#lessonDate").val();
        $.get(url, { classId: classId, lessonDate: lessonDate }, function (data) {
            $("#ClassList").html(data);
        });
    });

    //入班试听 - 选择
    $(document).on("click", ".btn-ClassInvitationChoose", function (e) {
        var $this = $(this);
        var url = $this.attr("href");
        var userId = $("#InvitationUserId").val();
        var invitationId = $(this).attr("data-id");

        $.post(url, { invitationId: invitationId, userId: userId }, function (data) {
            layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
                if (data.Status == 1) {
                    layer.close($("#InvitationIndex").val());
                }
            });
        });
    });

    //邀约
    $(document).on("click", "#btn_Invitation", function (e) {
        var $this = $(this);
        var url = $this.attr("href");
        var userId = $("#UserId").val();
        var loading = layer.load(0, { shade: false });
        $.get(url, { userId: userId }, function (data) {
            layer.close(loading);
            var title = "邀约";
            var index = layer.open({
                type: 1,
                title: title,
                zIndex: 2010,
                content: data,
                area: ['600px', '60%']
            });
            $("#InvitationIndex").val(index);
        });
    });
    //邀约
    $(document).on("click", "#btn_InvitationForPerson", function (e) {
        e.preventDefault();
        var $this = $(this);
        var url = $this.attr("href");
        var userId = $("#UserId").val();
        var loadUrl = $("#LoadUrl").val();
        var loading = layer.load(0, { shade: false });
        $.get(url, { userId: userId }, function (data) {
            layer.close(loading);
            var title = "邀约";
            var index = layer.open({
                type: 1,
                title: title,
                zIndex: 2010,
                content: data,
                area: ['600px', '60%'],
                end: function () {
                    $.post(loadUrl, { userId: userId }, function (data) {
                        $("#table_invite").html(data);
                    });
                }
            });
            $("#InvitationIndex").val(index);
        });
    });

    //公开课邀约编辑
    $(document).on("click", "#btn_EditInvitation", function (e) {
        e.preventDefault();
        if ($("input[name='invitationIds']:checked").length == 0) {
            layer.msg("请选择一条公开课！", { icon: 0, time: 2000 });
            return;
        }
        else if ($("input[name='invitationIds']:checked").length > 1) {
            layer.msg("只能选择一条公开课进行修改！", { icon: 0, time: 2000 });
            return;
        }
        else {
            var checkedId = $("input[name='invitationIds']:checked").val();
            var title = "编辑公开课";
            var userId = $("#UserId").val();
            var url = $(this).attr("href");
            var loadUrl = $("#LoadUrl").val();
            var loading = layer.load(0, { shade: false });
            $.get(url, { invitationId: checkedId }, function (data) {
                layer.close(loading);
                var index = layer.open({
                    type: 1,
                    title: title,
                    content: data,
                    area: ['800px', 'auto'],
                    success: function (layero, index) {
                        $.validator.unobtrusive.parse(document);
                    },
                    end: function () {
                        $.post(loadUrl, { userId: userId }, function (data) {
                            $("#table_invite").html(data);
                        });
                    }
                });
                layer.title(window.title, index);
                $("#InvitationChooseIndex").val(index);
            });
        }
    });
    //邀约全选
    $(document).on("click", "#select-all-Invitation", function () {
        if (this.checked) {
            $(".tn-checkbox-Invitation").each(function () {
                this.checked = true;
            });
        }
        else {
            $(".tn-checkbox-Invitation").each(function () {
                this.checked = false;
            });
        }
    });


    //跟进页签--新增 和续费页面的跟进  通用
    $(document).on("click", ".follow-up", function (e) {
        e.preventDefault();
        var loading = layer.load(0, { shade: false });
        var url = $(this).attr("href");
        $.get(url, function (data) {
            layer.close(loading);
            var index = layer.open({
                type: 1,
                title: "跟进新增",
                zIndex: 2005,
                content: data,
                closeBtn: 1,
                area: ['800px', '80%'],
                success: function (layero, index) {
                    $.validator.unobtrusive.parse(document);
                }
            });
            $("#FollowupIndex").val(index);
        });
    });

    //跟进页签--新增 和续费页面的跟进  通用
    $(document).on("click", ".follow-up-person", function (e) {
        e.preventDefault();
        var loading = layer.load(0, { shade: false });
        var url = $(this).attr("href");
        var userId = $("#UserId").val();
        var loadUrl = $("#UrlFlowUpList").val();
        $.get(url, function (data) {
            layer.close(loading);
            var index = layer.open({
                type: 1,
                title: "跟进新增",
                zIndex: 2005,
                content: data,
                area: ['800px', '80%'],
                success: function (layero, index) {
                    $.validator.unobtrusive.parse(document);
                },
                end: function () {
                    $.post(loadUrl, { userId: userId }, function (data) {
                        $("#table_followUp").html(data);
                    });
                }
            });
            $("#FollowupIndex").val(index);
        });
    });

});

function SmoothlyNavSchool() {
    $("body").hasClass("mini-navbar") ? $("body").hasClass("fixed-sidebar") ? ($(".school-info-box").hide(), setTimeout(function () {
        $(".school-info-box").fadeIn(500);
    },
        300)) : $(".school-info-box").removeAttr("style") : ($(".school-info-box").hide(), setTimeout(function () {
            $(".school-info-box").fadeIn(500);
        },
            100));
}

function bootstraptableCardView(selector) {
    $("#" + selector).bootstrapTable({
        onToggle: function () {
            var $table = $("#" + selector);
            if ($table.children("thead").css("display") == "none") {
                $("#" + selector + " td").each(function () {
                    var $this = $(this);
                    var index = $this.parent("tr").data("index");
                    var $first = $this.children(":first");
                    $first.wrap("<div class=\"panel-heading\" role=\"tab\" id=\"heading" + index + "\">" +
                        "<h4 class=\"panel-title\">" +
                        "<a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + selector + "collapse" + index + "\" aria-expanded=\"truecollapsed\" aria-controls=\"" + selector + "collapse" + index + "\" class=\"collapsed\"></a>" +
                        "</h4>" +
                        "</div>");
                    $first.append("<span class=\"fa toggle\"></span>");
                    $first.find("span").remove("span.title");
                    $this.children().not(":first").wrapAll("<div id=\"" + selector + "collapse" + index + "\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"heading" + index + "\">" +
                        "<div class=\"panel-body\"></div>" +
                        "</div>");
                });

                $("#" + selector + " td a").on("click", function () {
                    var index = $(this).parents("tr").data("index");
                    $(".panel-collapse.collapse").not("#collapse" + index).collapse("hide");
                });
            }
        },
        onColumnSearch: function () {
            var $table = $("#" + selector);
            if ($table.children("thead").css("display") == "none") {
                $("#" + selector + " td").each(function () {
                    var $this = $(this);
                    var index = $this.parent("tr").data("index");
                    var $first = $this.children(":first");
                    $first.wrap("<div class=\"panel-heading\" role=\"tab\" id=\"heading" + index + "\">" +
                        "<h4 class=\"panel-title\">" +
                        "<a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + selector + "collapse" + index + "\" aria-expanded=\"truecollapsed\" aria-controls=\"" + selector + "collapse" + index + "\" class=\"collapsed\"></a>" +
                        "</h4>" +
                        "</div>");
                    $first.append("<span class=\"fa toggle\"></span>");
                    $first.find("span").remove("span.title");
                    $this.children().not(":first").wrapAll("<div id=\"" + selector + "collapse" + index + "\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"heading" + index + "\">" +
                        "<div class=\"panel-body\"></div>" +
                        "</div>");
                });

                $("#" + selector + " td a").on("click", function () {
                    var index = $(this).parents("tr").data("index");
                    $(".panel-collapse.collapse").not("#collapse" + index).collapse("hide");
                });
            }
        },
        onColumnSwitch: function () {
            var $table = $("#" + selector);
            if ($table.children("thead").css("display") == "none") {
                $("#" + selector + " td").each(function () {
                    var $this = $(this);
                    var index = $this.parent("tr").data("index");
                    var $first = $this.children(":first");
                    $first.wrap("<div class=\"panel-heading\" role=\"tab\" id=\"heading" + index + "\">" +
                        "<h4 class=\"panel-title\">" +
                        "<a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + selector + "collapse" + index + "\" aria-expanded=\"truecollapsed\" aria-controls=\"" + selector + "collapse" + index + "\" class=\"collapsed\"></a>" +
                        "</h4>" +
                        "</div>");
                    $first.append("<span class=\"fa toggle\"></span>");
                    $first.find("span").remove("span.title");
                    $this.children().not(":first").wrapAll("<div id=\"" + selector + "collapse" + index + "\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"heading" + index + "\">" +
                        "<div class=\"panel-body\"></div>" +
                        "</div>");
                });

                $("#" + selector + " td a").on("click", function () {
                    var index = $(this).parents("tr").data("index");
                    $(".panel-collapse.collapse").not("#collapse" + index).collapse("hide");
                });
            }
        },
        onPageChange: function () {
            var $table = $("#" + selector);
            if ($table.children("thead").css("display") == "none") {
                $("#" + selector + " td").each(function () {
                    var $this = $(this);
                    var index = $this.parent("tr").data("index");
                    var $first = $this.children(":first");
                    $first.wrap("<div class=\"panel-heading\" role=\"tab\" id=\"heading" + index + "\">" +
                        "<h4 class=\"panel-title\">" +
                        "<a role=\"button\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#" + selector + "collapse" + index + "\" aria-expanded=\"truecollapsed\" aria-controls=\"" + selector + "collapse" + index + "\" class=\"collapsed\"></a>" +
                        "</h4>" +
                        "</div>");
                    $first.append("<span class=\"fa toggle\"></span>");
                    $first.find("span").remove("span.title");
                    $this.children().not(":first").wrapAll("<div id=\"" + selector + "collapse" + index + "\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"heading" + index + "\">" +
                        "<div class=\"panel-body\"></div>" +
                        "</div>");
                });

                $("#" + selector + " td a").on("click", function () {
                    var index = $(this).parents("tr").data("index");
                    $(".panel-collapse.collapse").not("#collapse" + index).collapse("hide");
                });
            }
        }
    });

}

function onlyNonNegative(obj) {
    var inputChar = event.keyCode;
    //alert(event.keyCode);  

    //1.判断是否有多于一个小数点  
    if (inputChar == 190) {//输入的是否为.  
        var index1 = obj.value.indexOf(".") + 1;//取第一次出现.的后一个位置  
        var index2 = obj.value.indexOf(".", index1);
        while (index2 != -1) {
            //alert("有多个.");  

            obj.value = obj.value.substring(0, index2);
            index2 = obj.value.indexOf(".", index1);
        }
    }
    //2.如果输入的不是.或者不是数字，替换 g:全局替换  
    obj.value = obj.value.replace(/[^(\d|.)]/g, "");
}

//返回顶部
function goTop(acceleration, time) {
    acceleration = acceleration || 0.1;
    time = time || 16;

    var x1 = 0;
    var y1 = 0;
    var x2 = 0;
    var y2 = 0;
    var x3 = 0;
    var y3 = 0;

    if (document.documentElement) {
        x1 = document.documentElement.scrollLeft || 0;
        y1 = document.documentElement.scrollTop || 0;
    }
    if (document.body) {
        x2 = document.body.scrollLeft || 0;
        y2 = document.body.scrollTop || 0;
    }
    var x3 = window.scrollX || 0;
    var y3 = window.scrollY || 0;

    // 滚动条到页面顶部的水平距离 
    var x = Math.max(x1, Math.max(x2, x3));
    // 滚动条到页面顶部的垂直距离 
    var y = Math.max(y1, Math.max(y2, y3));

    // 滚动距离 = 目前距离 / 速度, 因为距离原来越小, 速度是大于 1 的数, 所以滚动距离会越来越小 
    var speed = 1 + acceleration;
    window.scrollTo(Math.floor(x / speed), Math.floor(y / speed));

    // 如果距离不为零, 继续调用迭代本函数 
    if (x > 0 || y > 0) {
        var invokeFunction = "goTop(" + acceleration + ", " + time + ")";
        window.setTimeout(invokeFunction, time);
    }
}

function OnSuccessCallBackForInvitation(data) {

    layer.msg(data.Message, { icon: data.Status, time: 1500 }, function () {
        if (data.Status == 1) {
            layer.close($("#InvitationChooseIndex").val());
        }
    });
}

//图片悬浮信息
$(document).on("mouseover", ".infoPopup", function () {
    if ($(this).find("input").val().length > 10) {
        layer.tips($(this).find("input").val(), $(this), {
            tips: [3, '#3595CC'],
            time: 4000
        });
    }
});

$(document).on("mouseout", ".infoPopup", function () {
    layer.closeAll();
});

//获取地区联动选中值
//Parameter: $picker 容器Id
//          whichvalue 要获取哪个值["province", "city", "district"]
function distpickerValue(pickerId, whichvalue) {
    var provinceEle = $("#" + pickerId + " select:first");
    var cityEle = provinceEle.next();
    var districtEle = cityEle.next();
    if (whichvalue === "province") {
        return provinceEle.find("option:selected").data("code");
    }
    if (whichvalue === "city") {
        return cityEle.find("option:selected").data("code");
    }
    if (whichvalue === "district") {
        return districtEle.find("option:selected").data("code");
    }
    return "0";
}

//生成唯一标识
function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};

//子页面返回并刷新Manage页
//Parameter: actionName: actionName; controllerName: controllerName
function goBack(actionName, controllerName) {
    window.parent.$(".J_menuTab[data-id='/" + controllerName + "/" + actionName + "']").trigger("dblclick");
}

/******** 高德地图 简单封装 ********/

//浏览器自动定位当前位置
//Parameter: map: 高德地图地图实体
function locationCurrent(map) {
    //添加设备自动定位当前位置
    map.plugin('AMap.Geolocation', function () {
        geolocation = new AMap.Geolocation({
            enableHighAccuracy: true,//是否使用高精度定位，默认:true
            timeout: 10000,          //超过10秒后停止定位，默认：无穷大
            buttonOffset: new AMap.Pixel(10, 20),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
            zoomToAccuracy: true,      //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            buttonPosition: 'RB'
        });
        map.addControl(geolocation);
        geolocation.getCurrentPosition();
    });
}

//添加一个地图标记
//Parameter: map: 高德地图地图实体; 
//          locationArray: 位置数组 eg:[12.232, 2121.32];
//          title: 标记的标题
function addMarker(map, locationArray, title) {
    var marker = new AMap.Marker({
        position: locationArray,
        title: title,
        offset: new AMap.Pixel(0, 0),
        map: map,
        icon: "http://webapi.amap.com/theme/v1.3/markers/n/mark_b.png",
        animation: "AMAP_ANIMATION_BOUNCE",
        draggable: true
    });
    return marker;
}


//返回数组
function GetArr(str) {
    var StrArr = str.toString().split(',');
    var ArrStr = "[";
    for (var i = 0; i < StrArr.length; i++) {
        if (i == 0) {
            ArrStr += "[" + StrArr[i] + "," + StrArr[i + 1] + "]";
        }
        else {
            ArrStr += ",[" + StrArr[i] + "," + StrArr[i + 1] + "]";
        }
        i++;
    }
    ArrStr += "]";
    return ArrStr;
}

