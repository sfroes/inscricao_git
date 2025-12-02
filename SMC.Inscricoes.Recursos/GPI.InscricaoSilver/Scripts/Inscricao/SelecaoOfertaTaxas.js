
$(document).ready(function () {
    var divInstrucoes = $('#divInstrucoesTaxa'),
        divListaTaxas = $('#divListaTaxas'),
        numeroMaximoOferta = new smc.hidden($('#NumeroMaximoOfertaPorInscricao')).getValue(),
        ofertas = $('#Ofertas'),
        permiteMultiplaOferta = numeroMaximoOferta == '' || parseInt(numeroMaximoOferta) > 1;

    var displayAdditionalInfo = function (element, loading) {
        var obj = smc.core.uicontrol(element),
            val = obj.getValue();

        // Se o processo permitir mais de uma oferta (ou se a tela estiver carregando), não deverá atualizar a taxa se já existir alguma preenchida.
        if ((permiteMultiplaOferta || loading) && divListaTaxas.html().trim() != '')
            return;

        if (val == undefined || val == "") {

            $(divInstrucoes).hide();
            $(divListaTaxas).html("");

        } else {

            var data = { 'seqOferta': val };
            $.post($(divListaTaxas).attr('data-url'), data, function (data) {
                $(divListaTaxas).html(data);
                if (data != "") {
                    $(divInstrucoes).show();

                    $('[data-cobrarporoferta="true"]').each(function () {
                        $(this).val(new smc.masterDetail(ofertas).count());
                    });
                }
            });

        }
    };

    $(document).on('change', 'div[data-type="smc-lookup"]', function () {
        displayAdditionalInfo($(this), false);
    });

    displayAdditionalInfo($('div[data-type="smc-lookup"]').first(), true);

    ofertas.on('detailadded', function () {

        // Se o processo não permitir mais de uma oferta, não deverá somar valores ao total de inscrições
        if (!permiteMultiplaOferta)
            return;

        $('[data-cobrarporoferta="true"]').each(function () {

            $(this).val(parseInt($(this).val()) + 1).trigger('change');

        });
    });

    ofertas.on('detailremoved', function () {

        // Se o processo não permitir mais de uma oferta, não deverá somar valores ao total de inscrições
        if (!permiteMultiplaOferta)
            return;

        $('[data-cobrarporoferta="true"]').each(function () {

            $(this).val(parseInt($(this).val()) - 1).trigger('change');

        });
    });
    
});