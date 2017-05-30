var chat = $.connection.signalRHub;

$(function () {
    //用户上线
    chat.client.Online = function () {
        chat.server.online($("#userId").val());
    };

    //客户端弹出通知
    chat.client.Info = function (content, title, url) {
        if (url) {
            toastr.options.onclick = function () {
                layer.open({
                    type: 2,
                    title: title,
                    shadeClose: true,
                    shade: 0.8,
                    content: url
                });
            }
        }
        toastr.info(content, title);
    };

    chat.client.SignInNotice = function (result, cnName, enName, className, adviser, isFinal, msg) {
        var final = isFinal ? "该学员为最后一次上课！" : "";
        var isResult = result ? "<button class='btn btn-info btn-circle btn-xl' type='button'><i class='fa fa-check'></i></button>" :
            "<button class='btn btn-danger btn-circle btn-xl' type='button'><i class='fa fa-times'></i></button>";
        var html = '<div class="col-sm-7">' +
                        '<div class="form-group">中文名：' + cnName + '</div>' +
                        '<div class="form-group">英文名：' + enName + '</div>' +
                        '<div class="form-group">所在班级：' + className + '</div>' +
                        '<div class="form-group">所属顾问：' + adviser + '</div>' +
                        '<div class="form-group final"><p>' + final + '</p></div>' +
                   '</div>' +
                   '<div class="col-sm-5 result">' + isResult + '<p>' + msg + '<p/></div>';
        layer.open({
            title: "签到",
            type: 1,
            skin: 'layui-signin',
            shadeClose: false,
            shade: 0,
            area: ['500px'],
            content: '<div class="ibox-content">' + html + '</div>'
        })
        var audio = $("#audio")[0];
        if (audio != null && audio.canPlayType && audio.canPlayType("audio/mpeg")) {
            audio.pause(); 
            audio.src = "../../Content/ring.mp3"; 
            audio.play();
        }

    }

    $.connection.hub.start().done(function () {
        //layer.msg("signalR初始化完毕", { icon: 1 });
    });
});
