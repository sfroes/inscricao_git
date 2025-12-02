$(document).ready(function () {

    var tipoTemplate = $("#SeqTipoTemplateProcessoSGF");

    tipoTemplate.find('select').on("change", function (e) {

        var url = $(this).attr('data-ajax-url');
        var descricaoTipoProcesso = $("#Descricao").first().val();

        if (!existeModificacao()) {
            buscaConfiguracaoTemplateProcesso(descricaoTipoProcesso, $(this).val(), url);
            $.data(document.body, 'current', $(this).val());
            return;
        }

        if (!$(this).is("[data-confirm]")) {
            smc.core.confirm($(this).attr('data-changetitle'), $(this).attr('data-changemsg'), $(this), 'change');
            $(this).attr('data-selected', $("option:selected", this).val());
            $(this).val($.data(document.body, 'current'));
            return false;
        } else  {
            if ($(this).attr("data-confirm") == "true") {
                $(this).val($(this).attr('data-selected'));
                buscaConfiguracaoTemplateProcesso(descricaoTipoProcesso, $(this).val(), url);
            }
            $(this).removeAttr("data-confirm");
        }

        $.data(this, 'current', $(this).val());

    }).attr('data-configurated', 'true');

    $.data(document.body, 'current', $(tipoTemplate).val());

    function existeModificacao() {
        var templates = $('[data-detailid="Templates"]');
        
        if (templates.length > 0) {

            if (smc.masterDetail(templates).count() > 0) {
                return true;
            }

        }
        return false;
    }

    function buscaConfiguracaoTemplateProcesso(descricaoTipoProcesso, seqTipoTemplateProcesso, url) {
        $.ajax(
            {
                url: url,
                type: 'POST',
                data: JSON.stringify({ descricaoTipoProcesso: descricaoTipoProcesso, seqTipoTemplateProcesso: seqTipoTemplateProcesso }),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (data, textStatus, jqXHR) {
                    $("#divConfiguracaoTipoProcesso").html(data);
                }
            }
        );
    }


});