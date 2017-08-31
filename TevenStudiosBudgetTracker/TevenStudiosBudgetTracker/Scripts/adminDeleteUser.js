$(document).on("click", ".deleteButton", function () {
    var userId = $(this).data('id');
    $(".modal-body #bookId").val(userId);
});