$(document).ready(function () {
    SelecionarProcesseo();
    SelecionarGrupoOferta();
    SelecionarHierarquia();
    SalvarConfiguracao();

    SelecionarUnicoElemento("processo");
    BindSelect("processo", "seq-processo");
    ClicarTodosCheckbox();
});

function BindSelect(select, seq) {
    var seqSession = $("#" + seq + "-session").val();
    var optionsSelect = $("#" + select + " option");

    if (optionsSelect.length == 2) {
        return;
    }

    optionsSelect.each(function () {
        if ($(this).val() == seqSession && $(this).val() != "") {
            $(this).attr("selected", "selected");
            $(this).change();
        }
    });
}

function BindCheckbox() {
    var seqsOfertaSession = $("#seqsOfertaSession").val();
    var seqsOferta = seqsOfertaSession.split("|");
    var checkBox = $(":checkbox");

    if (checkBox.length == 1) {
        return;
    }

    checkBox.each(function () {
        if (seqsOferta.indexOf($(this).val()) > -1) {
            $(this).click();
            HabiltarBotoesSalvar();
        }
    });

    var checkBoxMarcado = $(":checkbox:checked");
    if (checkBoxMarcado.length == 0) {
        DesabilitarBotoesSalvar();
    }
}

function SelecionarProcesseo() {
    $("#processo").change(function () {
        var seqProcesso = $(this).val();
        var tokenHistoricoSituacao = $(this).find(":selected").attr("data-token-historico");

        $("#token-historico-situacao").val(tokenHistoricoSituacao);
        $("#div-hierarquia").hide();
        $("#div-ofertas").hide();
        DesabilitarBotoesSalvar();
        $("#div-grupo-oferta").show();

        LimparSelect("grupoOferta");
        LimparCheckbox();

        $.ajax({
            url: "Checkin/BuscarGrupoOfertas?seqProcesso=" + seqProcesso,
            type: 'POST',
            data: JSON.stringify({ seqProcesso: seqProcesso }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $.each(data, function (i, item) {
                    $("#grupoOferta").append("<option value='" + item.SeqGrupoOferta + "'>" + item.DescricaoGrupoOferta + "</option>");
                });

                SelecionarUnicoElemento("grupoOferta");
                BindSelect("grupoOferta", "seq-grupo-oferta");
            }
        });
    });
}

function SelecionarGrupoOferta() {
    $("#grupoOferta").change(function () {
        var seqProcesso = $("#processo").val();
        var seqGrupoOferta = $(this).val();

        $("#div-ofertas").hide();
        DesabilitarBotoesSalvar();

        LimparSelect("hierarquia");
        LimparCheckbox();

        $.ajax({
            url: "Checkin/BuscarHierarquiaOfertas?seqProcesso=" + seqProcesso + "&seqGrupoOferta=" + seqGrupoOferta,
            type: 'POST',
            data: JSON.stringify({ seqProcesso: seqProcesso }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var seqGrupoOferta = $("#grupoOferta").val();
                //Se não existir hierarquia, não exibe o campo e já exibi a oferta diretamente
                if (data.length === 0) {
                    $("#hierarquia").change();
                    $("#div-hierarquia").hide();
                    return;
                }

                $("#div-hierarquia").show();

                $.each(data, function (i, item) {
                    $("#hierarquia").append("<option value='" + item.SeqHierarquia + "'>" + item.DescricaoHierarquia + "</option>");
                });

                SelecionarUnicoElemento("hierarquia");
                BindSelect("hierarquia", "seq-hierarquia");
            }
        });
    });
}

function SelecionarHierarquia() {

    $("#hierarquia").change(function () {
        var seqProcesso = $("#processo").val();
        var seqHierarquia = $(this).val();

        if (!seqHierarquia) {
            seqHierarquia = 0;
        }

        $("#div-ofertas").show();

        LimparSelect("ofertas");
        LimparCheckbox();

        $.ajax({
            url: "Checkin/BuscarOfertas?SeqHierarquia=" + seqHierarquia + "&seqProcesso=" + seqProcesso,
            type: 'POST',
            data: JSON.stringify({ seqProcesso: seqProcesso }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $.each(data, function (i, item) {
                    $("#div-itens-ofertas").append('<div class="smc-checkbox"><label><input id="seqsOferta" name="seqsOferta" type="checkbox" value="' + item.SeqOferta + '">' + item.DescricaoCompleta + ' </label></div>');
                });


                BindCheckbox();
                SelecionarOferta();
                SelecionarCheckboxUnico();

            }
        });
    });
}

function SelecionarOferta() {
    $(":checkbox").change(function () {
        ValidarCheckboxSelecionado();
    });
}

function SelecionarUnicoElemento(select) {
    var select = $("#" + select);
    if (select.children().length == 2) {
        select.val(select.children()[1].value);
        select.change();
    }
}

function SelecionarCheckboxUnico() {
    var checkbox = $(":checkbox");
    if (checkbox.length == 1) {
        checkbox.click();
    }
}

function SalvarConfiguracao() {
    $("#btn-salvar-qrcode").click(function () {
        var action = $(this).attr("data-url");
        var validoPreenchimento = ValidarCheckboxSelecionado();
        var frm = $("#form-config");
        if (validoPreenchimento) {
            frm.attr(action);
            $("#form-config").submit();
        } else {
            alert("Selecione ao menos uma oferta!");
            return;
        }
    });

    $("#btn-salvar-manual").click(function () {
        var action = $(this).attr("data-url");
        var validoPreenchimento = ValidarCheckboxSelecionado();
        var frm = $("#form-config");
        if (validoPreenchimento) {
            frm.attr(action);
            $("#form-config").submit();
        } else {
            alert("Selecione ao menos uma oferta!");
            return;
        }
    });
}

function LimparSelect(select) {
    var select = $("#" + select);
    select.empty();
    select.append("<option value=''>Selecionar...</option>");
}

function LimparCheckbox() {
    var divCheckBox = $("#div-itens-ofertas");
    divCheckBox.empty();
}

function ValidarCheckboxSelecionado() {
    var checkboxSelecionados = $(":checkbox:checked");
    if (checkboxSelecionados.length == 0) {
        $("#btn-salvar-qrcode").attr("disabled", "disabled");
        $("#btn-salvar-manual").attr("disabled", "disabled");
        return false;
    } else {
        HabiltarBotoesSalvar()
        return true;
    }
}

function ClicarTodosCheckbox() {
    var linkClicadoTodos = $("#link-clicar-todos");
    var linkClicadoDesmarcar = $("#link-clicar-desmarcar");

    linkClicadoTodos.click(function () {
        var checkbox = $(":checkbox");
        checkbox.each(function () {
            if (!$(this).is(":checked")) {
                $(this).click();
            }
        });
    });

    linkClicadoDesmarcar.click(function () {
        var checkbox = $(":checkbox");
        checkbox.each(function () {
            if ($(this).is(":checked")) {
                $(this).click();
            }
        });
    });
}

function HabiltarBotoesSalvar() {
    $("#btn-salvar-qrcode").removeAttr("disabled");
    $("#btn-salvar-manual").removeAttr("disabled");
}

function DesabilitarBotoesSalvar() {
    $("#btn-salvar-qrcode").attr("disabled", "disabled");
    $("#btn-salvar-manual").attr("disabled", "disabled");
}