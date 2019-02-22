// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function(){
    $(".share-button").click(function() {

        var dataId = $(this).attr("data-id");
        $("#FileId").val(dataId);
        $("#share-modal").modal();
    });
    
    $("#btn-share").click(function() {

        var dataId = $("#FileId").val();

        $.ajax({
            url: '/Home/Share',
            type: 'post',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                ShareEmail: $("#ShareEmail").val(),
                FileId: $("#FileId").val()
            }),
            success: function (data) {
                window.location = "/";
            },
            error: function(data){
            }
        });
    });

   
});