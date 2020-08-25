let CreateUser = function () {
    let _CreateUser = function () {
        $('#btnCreateUser').on('click', function () {
            hideMessage()
            if (validateForm()) {
                $.ajax({
                    url: '/User/CreateNewUser',
                    type: "POST",
                    data: $("#frmCreateUser").serialize(),
                    success: function (result) {
                        if (result.IsSuccess) {
                            $("#createModal").modal("hide")
                            $("#tblUser").DataTable().ajax.reload();
                            hideMessage()
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

        $("#createModal").on('shown.bs.modal', function () {
            resetForm()
        });

        function validateForm() {
            const comfirmPassword = $('#txtComfirmPassword').val()
            const userName = $('#txtUsername').val()
            const fullName = $('#txtFullName').val()
            const address = $('#txtAddress').val()
            const mail = $('#txtMail').val()
            const cmnd = $('#txtCMND').val()
            const phone = $('#txtPhone').val()
            const password = $('#txtPassword').val()
            const updateBirthday = $('#txtBirthday').val()

            let validResult = validateInput(userName, "Bạn chưa nhập tên đăng nhập")
                && validateInput(fullName, "Bạn chưa nhập tên đầy đủ")
                && validateInput(address, "Bạn chưa nhập địa chỉ")
                && validateInput(mail, "Bạn chưa nhập địa chỉ email")
                && validateInput(cmnd, "Bạn chưa nhập chứng minh nhân dân")
                && validateInput(phone, "Bạn chưa nhập số điện thoại")
                && validateInput(password, "Bạn chưa nhập số mật khẩu")
                && validateInput(comfirmPassword, "Bạn chưa nhập mật khẩu xác thực")
                && validateInput(updateBirthday, "Bạn chưa nhập ngay sinh")

            if (validResult) {
                if (password !== comfirmPassword) {
                    showAlterMessage("Mật khẩu xác thực phải trùng với mật khẩu")
                    validResult = false
                }
            }
            return validResult
        }

        function validateInput(inputText, alertMessage) {
            if (!inputText || inputText === '') {
                showAlterMessage(alertMessage)
                return false
            }
            return true;
        }

        function showAlterMessage(mesg) {
            $('#err-message').show()
            $('#err-message-content').text(mesg)
        }

        function hideMessage() {
            $('#err-message-content').empty()
            $('#err-message').hide()
        }

        function resetForm() {
            $('.text-input').val('')
            $("input[type='checkbox']").prop('checked',false)
        }
    }
    return {
        init: function () {
            _CreateUser()
        }
    }
}()

document.addEventListener('DOMContentLoaded', function () {
    CreateUser.init();
})
