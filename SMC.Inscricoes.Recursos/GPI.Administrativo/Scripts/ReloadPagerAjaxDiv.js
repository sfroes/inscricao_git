reloadPagerAjaxDiv = function () {
    var dadosForm = $("#formPesquisaInscrito").serialize();
    var url = $("#formPesquisaInscrito").attr('action');
    $.ajax({
        url: url,
        type: "POST",
        data: dadosForm,
        success: function (resposta) {
            $("#divGridAcompanhamentoInscrito").html(resposta);
        }
    });
}
