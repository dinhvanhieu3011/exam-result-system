
    
        function reload()
        {
            location.reload();
            document.getElementById('search').value = "";
        }
    

    function Save()
    {
        $.post('@Url.Action("saveJS", "dm_Chuong")'),
    {
        id: document.getElementById('txtID').value,
        machuong: document.getElementById('txtMaChuong').value,
        tenchuong: document.getElementById('txtTenChuong').value,
        capNganSach: document.getElementById('txtCapNganSach').value,
        ghiChu: document.getElementById('txtGhiChu').value,
    },
        function (data) {
            if (data == -1) {

            }
            else {

            }
        };
}


    function SaveAndNew()
    {
        $.post('@Url.Action("saveAndNewJS", "dm_Chuong")'),
    {
        machuong: document.getElementById('txtMaChuong').value,
        tenchuong: document.getElementById('txtTenChuong').value,
        capNganSach: document.getElementById('txtCapNganSach').value,
        ghiChu: document.getElementById('txtGhiChu').value ,
        },
        function (data) {
            if (data == -1) {

            }
            else {

            }
        };
}


    function DeleteData(id)
    {
        $.post('@Url.Action("deleteJS", "dm_Chuong")'),
    {
        id: id
    },
        function(data) {
            if (data == -1) {

            }
            else {

            }
        };
}


    function EditData(id)
    {
        $.post('@Url.Action("getDataJS", "dm_Chuong")'),
    {
        id: id
    },
        function (data) {
            if (data == -1) {
            }
            else {
                document.getElementById('txtID').value = data.id;
                document.getElementById('txtMaChuong').value = data.maChuong;
                document.getElementById('txtTenChuong').value = data.tenChuong;
                document.getElementById('txtCapNganSach').value = data.Cap_NganSach;
                document.getElementById('txtGhiChu').value = data.ghiChu;

                document.getElementById('btnSave').style.visibility = "visible";
                document.getElementById('btnSaveAndNew').style.visibility = "hidden";
    
            }
        };
}




    $(document).ready(function () {
        $("#demoGrid").DataTable({
            initComplete: function() {
                $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
            },
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "pageLength": 5,

            "ajax": {
                "url": "/DanhMuc_DuAn/LoadData",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
[
{
    "targets": [0,2, 6],
    "visible": false,
    "searchable": false
},
{
    "targets": [7],
    "searchable": false,
    "orderable": false
},
],

            "columns": [
                  { "data": "Id", "name": "Id", "autoWidth": true },
                  { "data": "MaChuong", "name": "MaChuong", "autoWidth": true },
                  { "data": "TenChuong", "title": "TenChuong", "name": "TenChuong", "autoWidth": true },
                  { "data": "Ten", "name": "Ten", "autoWidth": true },
                  { "data": "Cap_NganSach", "name": "Cap_NganSach", "autoWidth": true },
                  { "data": "GhiChu", "name": "GhiChu", "autoWidth": true },
                  { "data": "Nam", "name": "Nam", "autoWidth": true },
                  {
                      "render": function (data, type, full, meta)

                      {
                          return '<button type="button" data-toggle="modal" onclick = "EditData(' + full.Id + ')" data-target="#createModal" style="padding:2px 6px;margin-left: 4px;border-radius: 10px !important;" class="btn btn-info"><i class="fa fa-edit"></i></button>' +
                                '<button type="button" onclick = "DeleteData('+full.Id+')" style="padding:2px 6px;margin-left: 4px;border-radius: 10px !important;" class="btn btn-danger"><i class="fa fa-trash"></i></button>';
                      }
                  },

            ]
        });
        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.Id + "'); >Delete</a>";
        document.getElementById("search").value = "";
    });
