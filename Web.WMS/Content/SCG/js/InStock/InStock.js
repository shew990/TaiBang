function getDetail(ID) {
    $("#detailTable").html("");
    $.ajax({
        type: "get",
        url: "GetDetail?ID=" + ID,
        dataType: "json",
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var html = "<tr>";
                html += "<td>" + data[i].RowNo + "</td>"
                //html += "<td>" + data[i].RowNoDel + "</td>"
                html += "<td>" + data[i].MaterialNo + "</td>"
                html += "<td>" + data[i].MaterialDesc + "</td>"
                //html += "<td>" + data[i].FromBatchNo + "</td>"
                html += "<td>" + data[i].InStockQty + "</td>"
                html += "<td>" + data[i].RemainQty + "</td>"
                html += "<td>" + data[i].ReceiveQty + "</td>"
                html += "<td>" + data[i].FromErpWarehouse + "</td>"
 html += "<td>" + data[i].TracNo+ "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
}

$(".guanbi").click(function () {
    if ($('input[type=checkbox]:checked').length != 1) {
        alert("必须选中一行任务");
        return;
    }
    var ID = "";
    if (confirm("确定关闭这" + $('input[type=checkbox]:checked').length + "个任务单据?")) {
        $.each($('input:checkbox:checked'), function () {
            ID = $(this).val();
        });
        $.ajax({
            type: "POST",
            url: "CloseInstock?ID=" + ID,
            date: null,
            dataType: "json",
            async: false,
            success: function (data) {
                alert(data.obj);
                if (data.Status) {
                    window.location.reload();
                }
            },
            fail: function () {
                alert("提交失败！")
            }
        });
    }
})


