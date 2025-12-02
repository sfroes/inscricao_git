
function exibeMensagemTipoTemplateDiferente(input) {
    var val = smc.core.uicontrol(input).getValue();
    if (val > 0) {
        var origem = $('input[name=SeqTipoTemplateProcessoOrigem]');
        var check = new smc.checkBox($('input[name="CopiarPaginas"]'));
        if ($(origem).val() != val) {
            $('[id="spanMensagemTipoTemplateDiferente"]').show();
            check.uncheck();
            check.disable('CopiarPaginas');
        } else {
            $('[id="spanMensagemTipoTemplateDiferente"]').hide();
            check.check();
            check.enable('CopiarPaginas');
        }
    }
}

function exibeMensagemIdiomas(input) {
    var check = new smc.checkBox($('input[name="CopiarPaginas"]'));
    if (input.val() == "false") {
        $('[id="spanMensagemIdiomas"]').show();
        check.uncheck();
        check.disable('CopiarPaginas2');
    } else {
        $('[id="spanMensagemIdiomas"]').hide();
        check.check();
        check.enable('CopiarPaginas2');
    }
}

$(document).on('smcload', function () {

    var inputTemplate = $('input[name="SeqTipoTemplateProcessoDestino"]').not('[data-gpi-configurated]').attr('data-gpi-configurated', 'true'),
        inputIdiomas = $('input[name="NaoPossuiIdiomasComuns"]').not('[data-gpi-configurated]').attr('data-gpi-configurated', 'true');

    inputTemplate.change(function () {
        exibeMensagemTipoTemplateDiferente($(this));
    });

    inputIdiomas.change(function () {
        exibeMensagemIdiomas($(this));
    });

});