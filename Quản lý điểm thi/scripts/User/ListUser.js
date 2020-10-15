let ListUser = function () {
    let _ListUser = function () {
        $("#tblUser").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "pageLength": 10,
            "ajax": {
                "url": "/User/GetListUser",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.searchValue = () =>($('#search').val())
                },
            },
            "columns": [
                  { "data": "Id" },
                  { "data": "FullName" },
                  { "data": "Username" },
                  {
                      "render": function (data, type, full, meta) {
                          if (full.Birthday) {
                              let date = full.Birthday.replace('/Date(', '').replace(')/', '')
                              return new Date(Number(date)).toLocaleDateString().slice(0, 10)
                          } else {
                              return ''
                          }
                      }
                   },
                  { "data": "Mail" },
                  { "data": "Phone" },
                  {
                      "render": function (data, type, full, meta) {
                          return '<button type="button" data-toggle="modal" value =' + full.Id + '  style="padding:2px 6px;margin-left: 10px;border-radius: 10px !important;" class="btn btn-info btn-edit-user"><i class="fa fa-edit"></i></button>' +
                                '<button type="button"   value =' + full.Id + ' style="padding:2px 6px;margin-left: 10px;border-radius: 10px !important;" class="btn btn-danger btn-delete-user"><i class="fa fa-trash"></i></button>';
                      }
                  },
            ]
        })

        $('#btnrefresh').on('click', function () {
            $("#tblUser").ajax.reload();
        })
    }
    return {
        init: function () {
            _ListUser()
        }
    }
}();

document.addEventListener('DOMContentLoaded', function () {
    ListUser.init()
})
