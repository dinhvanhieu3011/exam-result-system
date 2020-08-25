let ListGender = function () {
    let _ListGender = function () {
        $("#tblGender").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "pageLength": 10,
            "ajax": {
                "url": "/Gender/GetListGioiTinh",
                "type": "POST",
                "datatype": "json"
            },
            "columns": [
                  { "data": "Id" },
                  { "data": "Ten" },
                  { "data": "Mo_Ta" },
                  { "data": "Ghi_chu" },
                  {
                      "render": function (data, type, full, meta) {
                          return '<button type="button" data-toggle="modal" value =' + full.Id + '  style="padding:2px 6px;margin-left: 10px;border-radius: 10px !important;" class="btn btn-info btn-edit-gender"><i class="fa fa-edit"></i></button>' +
                                '<button type="button"   value =' + full.Id + ' style="padding:2px 6px;margin-left: 10px;border-radius: 10px !important;" class="btn btn-danger btn-delete-gender"><i class="fa fa-trash"></i></button>';
                      }
                  },
            ]
        })

        $('#btnrefresh').on('click', function () {
            $("#tblGender").ajax.reload();
        })
    }
    return {
        init: function () {
            _ListGender()
        }
    }
}();

document.addEventListener('DOMContentLoaded', function () {
    ListGender.init()
})
