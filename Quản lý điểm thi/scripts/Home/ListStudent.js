let ListStudent = function () {
    let _Select2Component = function () {
        $('.select2').select2()
    }
    let _ListStudent = function () {
        var table = $("#tblStudent").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "pageLength": 50,
            "searching": false,
            "lengthChange": false,
            "ajax": {
                "url": "/Home/GetListStudent",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.identifyNumber = () =>($('#txtIdNumber').val())
                    data.examRoom = () =>($('#selExamRoom').val())
                    data.candidateNumber = () =>($('#txtCandidateNumber').val())
                    data.examCouncil = () =>($('#selExamCouncil').val())
                    data.fullName = () =>($('#txtFullName').val())
                    data.exam = () =>($('#selExam').val())
                    data.fromBirthday = () => ($('#txtFromBirthday').val())
                    data.toBirthday = () => ($('#txtToBirthay').val())
                    data.toTestDay = () => ($('#txtToTestDay').val())
                    data.fromTestDay = () => ($('#txtFromTestDay').val())
                    data.ketQua = () => ($('#selKetQua').val())
                    data.truong = () => ($('#selTruong').val())
                    data.gioiTinh = () => ($('#selGioiTinh').val())
                    data.loaiTN = () => ($('#selLoaiTN').val())
                    data.hanhKiem = () => ($('#selHanhKiem').val())
                    data.hocLuc = () => ($('#selHocLuc').val())
                    data.dienUT = () => ($('#selDienUT').val())
                },
            },

            "columnDefs":
                [
                    { "width": "50px", "targets": 1 },
                    { "width": "50px", "targets": 0 },
                    //{ "width": "200px", "targets": 6 },
                    //{
                    //    "targets": [6],
                    //    "searchable": false,
                    //    "orderable": false,
                    //    "className": 'text-center'
                    //}
                ],
            "columns":
                [
                    { "data": "Id", "name": "Id", "autoWidth": true },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="/Student/detail/' + full.Id + '"><img src="/dist/img/icons8-pdf-48.png"  alt="pdf-diem-thi" style="width:30px;height:30px;margin-left:20px;cursor:pointer"></a>';
                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="/Student/detail/' + full.Id + '">' + full.ho_ten + '</a>';
                        }
                    },
                    { "data": "sbd", "name": "sbd", "autoWidth": true },
                    { "data": "ngay_sinh", "name": "ngay_sinh", "autoWidth": true },
                    { "data": "ketqua_thi", "name": "ketqua_thi", "autoWidth": true },
                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return '<button type="button"  onclick = "DeleteData(' + full.Id + ')" style="padding:2px 6px;margin-left: 10px;border-radius: 10px !important;" class="btn btn-danger"><i class="fa fa-trash"></i></button>'
                    //    }
                    //},
                ],
        })
        table.on('draw.dt', function () {
            var PageInfo = table.page.info();
            table.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            });
        });
        $('#btnrefresh, #btnSearch').on('click', function () {
            $('#tblStudent').DataTable().ajax.reload();;
        })
    }
    let _RefeshSelect2Control = function () {
        function getSelectData(urlApi, id, selectControl, textHolder) {
            $.post(urlApi, { id: id },
                 function (data) {
                     appendOption(data, selectControl, textHolder)
                 }
            );
        }

        function appendOption(data, selectControl, textHolder) {
            $(selectControl).empty().trigger('change')
            console.log({ data })
            const arrs = data.arrs
            console.log({ arrs })

            let newOption = new Option(textHolder, '', true, true)
            $(selectControl).append(newOption).trigger('change')
            arrs.forEach(function (data) {
                var newOption = new Option(data.value_1, data.Id, false, false)
                $(selectControl).append(newOption).trigger('change')
            })
        }

        $('#selExam').on('change', function () {
            let id = $(this).val()
            getSelectData('/Home/GetHDongThi', id, $('#selExamCouncil'), '-Chọn hội đồng-')
        })

        $('#selExamCouncil').on('change', function () {
            let id = $(this).val()
            getSelectData('/Home/GetExamRoom', id, $('#selExamRoom'), '-Chọn phòng thi-')
        })
    }
    return {
        init: function () {
            _Select2Component()
            _ListStudent()
            _RefeshSelect2Control()
        }
    }
}();
let ExportFile = function () {
    let _ExportFile = function () {
        $('#btn_export_excels').on('click', function () {
            $.ajax({
                url: '/Home/ExportExcelSearchResult',
                type: "post",
                data: {
                    identifyNumber: $('#txtIdNumber').val(),
                    examRoom: $('#selExamRoom').val(),
                    candidateNumber: $('#txtCandidateNumber').val(),
                    examCouncil: $('#selExamCouncil').val(),
                    fullName: $('#txtFullName').val(),
                    exam: $('#selExam').val(),
                    fromBirthday: $('#txtFromBirthday').val(),
                    toBirthday: $('#txtToBirthay').val(),
                    toTestDay: $('#txtToTestDay').val(),
                    fromTestDay: $('#txtFromTestDay').val(),
                    ketQua: $('#selKetQua').val(),
                    truong: $('#selTruong').val(),
                    gioiTinh: $('#selGioiTinh').val(),
                    loaiTN: $('#selLoaiTN').val(),
                    hanhKiem: $('#selHanhKiem').val(),
                    hocLuc: $('#selHocLuc').val(),
                    dienUT: $('#selDienUT').val(),
                },
                success: function (result) {
                    if (result.IsSuccess) {
                        window.location.href = '/Home/DownloadExcel';
                    } else {
                        alert(result.Message)
                    }
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        })
    }
    return {
        init: function () {
            _ExportFile()
        }
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    ListStudent.init()
    ExportFile.init()
})
