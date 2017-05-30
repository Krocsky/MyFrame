$(function () {
    $("input[type='file']").livequery(function () {
        var $this = $(this);
        var data = eval("(" + $this.attr("data") + ")");
        data.uploadExtraData = eval("(" + data.uploadExtraData + ")")
        $this.fileinput(data);
    });
});