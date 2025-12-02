$(document).on('smcload', function () {
    var linhas = $('div[data-type="smc-gpi-linha-documentos"]').not('smc-gpi-binded');

    $(linhas).each(function (index, item) {
        $(item).find('input,select').not('[type="button"]').change(
            function () {
                var div = $(this).closest('div[data-type="gpi-div-documento-entregue"]');
                $(div).addClass('smc-gpi-linha-documento-alterada');
            });
    });
});

// Quando clica em duplicar documento, a janela estava ficando com top 0 e left 0, devido a perder a referência do botão que abriu a mesma (pois recarrega o conteúdo da lista)
$(document).on("click", "[data-behavior='DuplicarDocumentoEntregue']", function () {
    if (smc.contextWindow)
        smc.contextWindow.fechaJanelas();
});