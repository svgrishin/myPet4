$(function () {
    $("#" + $("#select option:selected").val()).show();
    $("#select").change(function(){
        $(".select-blocks").hide();
        $("#" + $(this).val()).show();
    });
});