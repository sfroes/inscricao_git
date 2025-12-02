$(document).ready(function () {

    $('select[name="SeqUnidadeResponsavel"]').change(function ()
    {
        var url = $(this).attr('data-url-reload');
        var seq = $(this).val();
        $.ajax(
           {
               url: url,
               type: 'POST',
               data: JSON.stringify({ seqUnidadeResponsavel: seq }),
               contentType: "application/json; charset=utf-8",
               dataType: "html",
               success: function (data, textStatus, jqXHR) {
                   $("#divContatosProcesso").html(data).show();
               }
           });
    });
});