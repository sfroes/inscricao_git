reloadPagerCheckinEmLote = function () {
    var dadosForm = $("#formPesquisaInscritos").serialize();
    var url = $("#formPesquisaInscritos").attr('action');
    $.ajax({
        url: url,
        type: "POST",
        data: dadosForm,
        success: function (resposta) {
            $("#divGridAcompanhamentoInscritoCheckinLote").html(resposta);
        }
    });
}
