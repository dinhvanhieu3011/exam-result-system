let UpdateProfile = function () {
    let _UpdateProfile = function () {
        $('#btnSave_Change_Profile').on('click', function () {
            hideMessage()
            if (validateForm()) {
                $.ajax({
                    url: '/Home/UpdateProfile',
                    type: "post",
                    data: $("#frmUpdateProfile").serialize(),
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
            const email = $('#txtEmail_Edit').val()
            const phone = $('#txtDienThoai_Edit').val()
            const fullName = $('#txtHoTen_Edit').val()

            let validResult = validateInput(phone, "Bạn chưa nhập số điện thoại")
                && validateInput(email, "Bạn chưa nhập email")
                && validateInput(fullName, "Bạn chưa nhập họ tên")

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

        $('#modal_Message_Success').on('hidden.bs.modal', function () {
            location.reload();
        })

        function showAlterMessage(mess) {
            $('#notif-profile').show()
            $('#notif-profile').text(mess)
        }

        function hideMessage() {
            $('#modal_Message_Success').modal('hide')
            $('#modal_Message_Success .message-content').text('')
            $('#notif-profile').empty()
            $('#notif-profile').hide()
        }
    }
    return {
        init: function () {
            _UpdateProfile()
        }
    }
}()
document.addEventListener('DOMContentLoaded', function () {
    UpdateProfile.init()
})
