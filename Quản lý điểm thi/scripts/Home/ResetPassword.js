let ResetPass = function () {
    let _ResetPass = function () {
        $('#btnSave_Change_Pass').on('click', function () {
            hideMessage()
            if (validateForm()) {
                $.ajax({
                    url: '/Home/ResetPassword',
                    type: "post",
                    data: $("#frmResetPassword").serialize(),
                    success: function (result) {
                        if (result.IsSuccess) {
                            $("#myModal_change_pass").modal("hide")
                            showSuccessMessage('Thay đổi mật khẩu thành công')
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
            const comfirmPassword = $('#txt_cormfirm_pass').val()
            const password = $('#txt_new_pass').val()
            const oldPassword = $('#txt_oldpass').val()

            let validResult = validateInput(password, "Bạn chưa nhập số mật khẩu")
                && validateInput(comfirmPassword, "Bạn chưa nhập mật khẩu xác thực")
                && validateInput(oldPassword, "Bạn chưa nhập mật khẩu cũ")

            if (validResult) {
                if (password !== comfirmPassword) {
                    showAlterMessage("Mật khẩu xác thực phải trùng với mật khẩu")
                    validResult = false
                }
            }
            return validResult
        }

        function showSuccessMessage(mess) {
            $('#modal_Message_Success').modal('show')
            $('#modal_Message_Success .message-content').text(mess)
        }

        function validateInput(inputText, alertMessage) {
            if (!inputText || inputText === '') {
                showAlterMessage(alertMessage)
                return false
            }
            return true;
        }

        function showAlterMessage(mess) {
            $('#notif-pass').show()
            $('#notif-pass').text(mess)
        }

        function hideMessage() {
            $('#modal_Message_Success').modal('hide')
            $('#modal_Message_Success .message-content').text('')
            $('#notif-pass').empty()
            $('#notif-pass').hide()
        }
    }
    return {
        init: function () {
            _ResetPass()
        }
    }
}()

document.addEventListener('DOMContentLoaded', function () {
    ResetPass.init()
})
