let UpdateUser = function () {
    let _UpdateUser = function () {
        $('#tblGender').on('click', '.btn-edit-gender', function () {
            let id = $(this).val()
           
            let role = RoleAction.Edit
            $.ajax({
                url: '/Common/CheckRole',
                type: "post",
                data: { role },
                success: function (result) {
                    if (result.IsSuccess) {
                        getUserById(id)
                        hideMessage()
                        $('#updateModal').modal('show')
                    } else {
                        showAlterMessage(result.Message)
                    }
                },
                error: function (err) {
                    showAlterMessage(err.statusText);
                }
            });
        })

        $('#btnUpdateGender').on('click', function () {
            hideMessage()
            if (validateForm()) {
                $.ajax({
                    url: '/Gender/UpdateGioiTinh',
                    type: "POST",
                    data: $("#frmUpdateGender").serialize(),
                    success: function (result) {
                        if (result.IsSuccess) {
                            $("#updateModal").modal("hide")
                            $("#tblGender").DataTable().ajax.reload();
                            showSuccessMessage('Cập nhật thành công')
                        } else {
                            showAlterMessagelb(result.Message)
                        }
                    },
                    error: function (err) {
                        showAlterMessagelb(err.statusText);
                    }
                });
            }
        })

        function validateForm() {
            const name = $('#txtUpdateName').val()
            let validResult = validateInput(name, "Bạn chưa nhập tên giới tính")
            return validResult
        }

        function showAlterMessagelb(mesg) {
            $('#err-message').show()
            $('#err-message-content').text(mesg)
        }

        function getUserById(id) {
            let data
            $.ajax({
                url: '/Gender/GetById',
                type: "Get",
                asysnc: true,
                data: { Id: id },
                success: function (result) {
                    resetForm()
                    setDataToControl(result.GioiTinh)
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }

        function setDataToControl(data) {
            $('#txtUpdateDes').val(data.Mo_Ta)
            $('#txtUpdateNote').val(data.Ghi_chu)
            $('#txtUpdateName').val(data.Ten)
            $('#hdUpdateId').val(data.Id)
        }

        function resetForm() {
            $('.text-input').val('')
            $("input[type='checkbox']").prop('checked', false)
        }

        function validateInput(inputText, alertMessage) {
            if (!inputText || inputText === '') {
                showAlterMessagelb(alertMessage)
                return false
            }
            return true;
        }

        function showAlterMessagelb(mess) {
            $('#err-update-message').show()
            $('#err-update-message-content').text(mess)
        }
    }
    let _DeleteUser = function () {
        $('#tblGender').on('click', '.btn-delete-gender', function () {
            let id = $(this).val()
            let role = RoleAction.Delete
            $.ajax({
                url: '/Common/CheckRole',
                type: "post",
                data: { role},
                success: function (result) {
                    if (result.IsSuccess) {
                        $("#deleteId").val(id)
                        $('#modalAlertDelete').modal('show')
                    } else {
                        showAlterMessage(result.Message)
                    }
                },
                error: function (err) {
                    showAlterMessage(err.statusText);
                }
            });
        })

        $('#modalAlertDelete').on('hide.bs.modal', function (event) {
            $("#deleteId").val('')
        })

        $('#btnDeleteAccess').on('click', function () {
            hideMessage();
            $.ajax({
                url: '/Gender/delete',
                type: "POST",
                data: $("#formDelete").serialize(),
                success: function (result) {
                    if (result.IsSuccess) {
                        showSuccessMessage('Xóa giới tính thành công')
                        $("#tblGender").DataTable().ajax.reload();
                    } else {
                        showAlterMessage(result.Message)
                    }
                },
                error: function (err) {
                    showAlterMessage(err.statusText);
                }
            });
            $('#modalAlertDelete').modal('hide')
        })
    }
    return {
        init: function () {
            _UpdateUser()
            _DeleteUser()
        }
    }
}()
document.addEventListener('DOMContentLoaded', function () {
    UpdateUser.init()
})
