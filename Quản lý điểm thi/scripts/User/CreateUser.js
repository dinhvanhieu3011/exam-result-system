let CreateUser = function () {
    let _CreateUser = function () {

        var image = document.getElementById('imageCreatePreview');
        var input = document.getElementById('inputCreateAvatar');
        var cropper;

        input.addEventListener('change', function (e) {
            var files = e.target.files;
            var done = function (url) {
                input.value = '';
                image.src = url;
            };
            var reader;
            var file;
            var url;
            if (files && files.length > 0) {
                file = files[0];
                if (URL) {
                    done(URL.createObjectURL(file));
                } else if (FileReader) {
                    reader = new FileReader();
                    reader.onload = function (e) {
                        done(reader.result);
                    };
                    reader.readAsDataURL(file);
                }
            }
            if (cropper) {
                cropper.destroy();
                cropper = null;
            }
            cropper = new Cropper(image, {
                dragMode: 'move',
                aspectRatio: 1 / 1,
                autoCropArea: 1,
                restore: false,
                guides: false,
                center: true,
                highlight: false,
                cropBoxMovable: false,
                cropBoxResizable: false,
                toggleDragModeOnDblclick: false,
            });
        });

        $('#btnCreateUser').on('click', function () {
            hideMessage()
            if (validateForm()) {
                var form = $("#frmCreateUser")[0]
                var formData = new FormData(form)
                if (cropper) {
                    canvas = cropper.getCroppedCanvas({
                        width: 160,
                        height: 160,
                    });
                    initialAvatarURL = image.src;
                    image.src = canvas.toDataURL();
                    canvas.toBlob(function (blob) {
                        formData.append('Avatar', blob, 'avatar.jpg');
                        $.ajax({
                            url: '/User/CreateNewUser',
                            type: "post",
                            data: formData,
                            cache: false,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                if (result.IsSuccess) {
                                    $("#createModal").modal("hide")
                                    $("#tblUser").DataTable().ajax.reload();
                                    hideMessage()
                                    showSuccessMessage('Thêm người dùng thành công.')
                                } else {
                                    showAlterMessage(result.Message)
                                }
                            },
                            error: function (err) {
                                alert(err.statusText);
                            }
                        });
                    });
                } else {
                    $.ajax({
                        url: '/User/CreateNewUser',
                        type: "post",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            if (result.IsSuccess) {
                                $("#createModal").modal("hide")
                                $("#tblUser").DataTable().ajax.reload();
                                hideMessage()
                                showSuccessMessage('Thêm người dùng thành công.')
                            } else {
                                showAlterMessage(result.Message)
                            }
                        },
                        error: function (err) {
                            alert(err.statusText);
                        }
                    });
                }

            }
        })

        $("#createModal").on('shown.bs.modal', function () {
            let role = RoleAction.Create
            $.ajax({
                url: '/Common/CheckRole',
                type: "post",
                data: { role },
                success: function (result) {
                    if (result.IsSuccess) {
                        resetForm()
                    } else {
                        hideMessage()
                        showModalAlterMessage(result.Message)
                        $("#createModal").modal('hide')
                    }
                },
                error: function (err) {
                    showAlterMessage(err.statusText);
                }
            });
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

        function showModalAlterMessage(mess) {
            $('#modalErroMess').modal('show')
            $('#modalErroMess .message-content').text(mess)
        }

        function showAlterMessage(mesg) {
            $('#err-message').show()
            $('#err-message-content').text(mesg)
        }

        function showSuccessMessage(mess) {
            $('#modalSuccessMess').modal('show')
            $('#modalSuccessMess .message-content').text(mess)
        }

        function hideMessage() {
            $('#success-message').modal('hide')
            $('#success-message .message-content').text('')
            $('#err-message-content').empty()
            $('#err-message').hide()
        }

        function resetForm() {
            $('.text-input').val('')
            $("input[type='checkbox']").prop('checked', false)
            if (cropper) {
                cropper.destroy();
                cropper = null;
                image.src = "https://via.placeholder.com/250"
            }
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
