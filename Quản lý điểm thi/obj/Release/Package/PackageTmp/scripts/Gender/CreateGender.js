let CreateGender = function () {
    let _CreateGender = function () {
        $('#btnCreateGender').on('click', function () {
            hideMessagelb()
            if (validateForm()) {
                $.ajax({
                    url: '/Gender/CreateGioiTinh',
                    type: "POST",
                    data: $("#frmCreateGender").serialize(),
                    success: function (result) {
                        if (result.IsSuccess) {
                            $("#createModal").modal("hide")
                            $("#tblGender").DataTable().ajax.reload();
                            hideMessagelb()
                            showSuccessMessage('Thêm mới giới tính thành công')
                        } else {
                            showAlterMessagelb(result.Message)
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
            const name = $('#txtName').val()
            let validResult = validateInput(name, "Bạn chưa nhập tên giới tính")
            return validResult
        }

        function validateInput(inputText, alertMessage) {
            if (!inputText || inputText === '') {
                showAlterMessagelb(alertMessage)
                return false
            }
            return true;
        }

        function showAlterMessagelb(mesg) {
            $('#err-message').show()
            $('#err-message-content').text(mesg)
        }

        function hideMessagelb() {
            $('#err-message-content').empty()
            $('#err-message').hide()
        }

        function resetForm() {
            $('.text-input').val('')
            $("input[type='checkbox']").prop('checked', false)
        }
    }
    return {
        init: function () {
            _CreateGender()
        }
    }
}()

document.addEventListener('DOMContentLoaded', function () {
    CreateGender.init();
})
