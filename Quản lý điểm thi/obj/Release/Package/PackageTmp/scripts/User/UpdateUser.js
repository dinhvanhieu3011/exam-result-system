let UpdateUser = function () {
    let _UpdateUser = function () {
        $('#tblUser').on('click', '.btn-edit-user', function () {
            let id = $(this).val()
            getUserById(id)
            hideMessage()
            $('#updateModal').modal('show')
        })

        $('#btnUpdateUser').on('click', function () {
            hideMessage()
            if (validateForm()) {
                $.ajax({
                    url: '/User/UpdateUser',
                    type: "POST",
                    data: $("#frmUpdateUser").serialize(),
                    success: function (result) {
                        if (result.IsSuccess) {
                            $("#updateModal").modal("hide")
                            $("#tblUser").DataTable().ajax.reload();
                            showSuccessMessage('Cập nhật thành công')
                        } else {
                            showAlterMessage(result.Message)
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
        })

        function validateForm() {
            const comfirmPassword = $('#txtUpdateComfirmPassword').val()
            const userName = $('#txtUpdateUsername').val()
            const fullName = $('#txtUpdateFullName').val()
            const address = $('#txtUpdateAddress').val()
            const mail = $('#txtUpdateMail').val()
            const cmnd = $('#txtUpdateCMND').val()
            const phone = $('#txtUpdatePhone').val()
            const password = $('#txtUpdatePassword').val()
            const birthday = $('#txtUpdateBirthday').val()

            let validResult = validateInput(userName, "Bạn chưa nhập tên đăng nhập")
                && validateInput(fullName, "Bạn chưa nhập tên đầy đủ")
                && validateInput(address, "Bạn chưa nhập địa chỉ")
                && validateInput(mail, "Bạn chưa nhập địa chỉ email")
                && validateInput(cmnd, "Bạn chưa nhập chứng minh nhân dân")
                && validateInput(phone, "Bạn chưa nhập số điện thoại")
                && validateInput(password, "Bạn chưa nhập số mật khẩu")
                && validateInput(comfirmPassword, "Bạn chưa nhập mật khẩu xác thực")
                && validateInput(birthday, "Bạn chưa nhập ngay sinh")

            if (validResult) {
                if (password !== comfirmPassword) {
                    showAlterMessage("Mật khẩu xác thực phải trùng với mật khẩu")
                    validResult = false
                }
            }
            return validResult
        }

        function showAlterMessage(mesg) {
            $('#err-message').show()
            $('#err-message-content').text(mesg)
        }

        function getUserById(id) {
            let data
            $.ajax({
                url: '/User/GetById',
                type: "Get",
                asysnc: true,
                data: { Id: id },
                success: function (result) {
                    resetForm()
                    setDataToControl(result.User)
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }

        function setDataToControl(data) {
            $('#txtUpdateComfirmPassword').val(data.Password)
            $('#txtUpdateUsername').val(data.Username)
            $('#txtUpdateFullName').val(data.FullName)
            $('#txtUpdateAddress').val(data.Address)
            $('#txtUpdateMail').val(data.Mail)
            $('#txtUpdateCMND').val(data.CMND)
            $('#txtUpdatePhone').val(data.Phone)
            $('#txtUpdatePassword').val(data.Password)
            $('#hdUpdateId').val(data.Id)
            const roles = JSON.parse(data.Image)
            if (roles) {
                const chkRoles = $("input[name='Role']")
                Array.prototype.forEach.call(chkRoles, function (chkRole) {
                    let chkVal = $(chkRole).attr('value');
                    chkVal = Number(chkVal)
                    $(chkRole).prop('checked', roles.includes(chkVal));
                })
            }
            const date = data.Birthday.replace('/Date(','').replace(')/','')
            $('#txtUpdateBirthday').val(new Date(Number(date)).toISOString().slice(0, 10))
            $('#chkAdmin').prop('checked', data.IsAdmin|| data.IsAdmin == 'true')
        }

        function resetForm() {
            $('.text-input').val('')
            $("input[type='checkbox']").prop('checked', false)
        }

        function validateInput(inputText, alertMessage) {
            if (!inputText || inputText === '') {
                showAlterMessage(alertMessage)
                return false
            }
            return true;
        }

        function showAlterMessage(mess) {
            $('#err-update-message').show()
            $('#err-update-message-content').text(mess)
        }

        function showSuccessMessage(mess) {
            $('#modalSuccessMess').modal('show')
            $('#modalSuccessMess .message-content').text(mess)
        }

        function hideMessage() {
            $('#success-message').modal('hide')
            $('#success-message .message-content').text('')
            $('#err-update-message-content').empty()
            $('#err-update-message').hide()
        }
    }
    let _DeleteUser = function () {
        $('#tblUser').on('click', '.btn-delete-user', function () {
            let id = $(this).val()
            $("#deleteUserId").val(id)
            $('#modalAlertDelete').modal('show')
        })

        $('#modalAlertDelete').on('hide.bs.modal', function (event) {
            $("#deleteUserId").val('')
        })

        $('#btnDeleteAccess').on('click', function () {
            hideMessage();
            $.ajax({
                url: '/user/delete',
                type: "POST",
                data: $("#formDeleteUser").serialize(),
                success: function (result) {
                    if (result.IsSuccess) {
                        $("#tblUser").DataTable().ajax.reload();
                    } else {
                        showAlterMessage(result.Message)
                    }
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
            $('#modalAlertDelete').modal('hide')
        })

        function showSuccessMessage(mess) {
            $('#modalSuccessMess').modal('show')
            $('#modalSuccessMess .message-content').text(mess)
        }
        
        function showAlterMessage(mess) {
            $('#modalErroMess').modal('show')
            $('#modalErroMess .message-content').text(mess)
        }

        function hideMessage() {
            $('#success-message').modal('hide')
            $('#success-message .message-content').text('')
            $('#modalErroMess').modal('hide')
            $('#modalErroMess .message-content').text('')
        }
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
