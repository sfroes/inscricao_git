$(document).ready(function () {
    EnviarFormualarioPesquisa();
    PesquisarNomeInscrito();
    FecharModalConfirmacaoCheckin();
    EfetuarCheckin();
    LimparFormulario();    
});

function PesquisarNomeInscrito() {

    $('#campo-nome').autocomplete({
        minLength: 1,
        autoFocus: true,
        delay: 300,
        position: {
            my: 'left top',
            at: 'right top'
        },
        appendTo: '#auto-pesquisa',
        source: function (request, response) {
            $.ajax({
                url: 'PesquiarNomeCheckinManual',
                type: 'POST',
                dataType: 'Json',
                data: { Nome: request.term }
            }).done(function (data) {
                if (data.Mensagem.length > 0) {
                    splitNome = data.Mensagem.split('|');
                    response($.each(splitNome, function (key, item) {
                        return ({
                            label: item
                        });
                    }));
                }
            });
        }
    });
}

function FecharModalConfirmacaoCheckin() {
    $("#botao-mensagem-sucesso").click(function () {
        $("#mensagem-sucesso").hide();
    });
    $("#botao-mensagem-erro").click(function () {
        $("#mensagem-erro").hide();
    });
}

function EfetuarCheckin() {
    $("#btn-salvar").click(function () {
        var guidCheckin = $("input:radio:checked").val();

        if (guidCheckin == undefined) {
            return;
        }

        $.ajax({
            url: "EfetuarCheckin",
            type: 'POST',
            data: JSON.stringify({ "guid": guidCheckin, "TipoCheckin": 2 }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#mensagem-sucesso").show();
                $("#botao-mensagem-sucesso").focus();
                $("#detalhes-sucesso").html(data.Mensagem);

                var data = new Date();
                var hora = data.getHours();
                var minuto = data.getMinutes() < 10 ? "0" + data.getMinutes() : data.getMinutes();
                $("#hora-checkin").text(hora + ":" + minuto);
                $("#resultado-pesquisa").empty();
                $("#btn-salvar").attr("disabled", "disabled");
            },
            error: function (data) {
                $("#mensagem-sucesso").hide();
                $("#mensagem-erro").show();
                $("#botao-mensagem-erro").focus();
                $("#detalhes-erro").html(data.responseJSON.Mensagem);
            }
        });
    });
}

function EnviarFormualarioPesquisa() {
    $("#form-pesquisa").submit(function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = { "nome": $("#campo-nome").val(), "cpf": $("#cpf").val() };

        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var divResultado = $("#resultado-pesquisa");
                divResultado.empty();
                $("#corpo-pesquisa").show();
                var resultado = "";                
                $.each(data, function (index, value) {
                    resultado += `<li class="checkin-resultados-item">
                        <ul>
                            <li>
                                <svg viewBox="0 0 256 512">
	                                <path d="M246.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-9.2-9.2-22.9-11.9-34.9-6.9s-19.8 16.6-19.8 29.6l0 256c0 12.9 7.8 24.6 19.8 29.6s25.7 2.2 34.9-6.9l128-128z"/>
                                </svg>
                                    ${value.DescricaoOferta}
                            </li>`;
                    $.each(value.Inscritos, function (i, v) {
                        if (v.CheckinEfetuado != true) {
                            resultado += `<li>
                                        <input type="radio" id="evento${index}${i}" name="evento" value="${v.GuidInscricao}" />
                                        <label for="evento${index}${i}">
                                            <span>${v.Nome}</span>
                                        </label>
                                    </li>`;
                        } else {
                            resultado += `<li class="inactive">                                
                                <label for="evento${index}${i}">
                                    <span>${v.Nome}</span>
                                </label>
                                <button type="button" class="smc-btn-desfazer" title="Desfazer check-in" data-guid="${v.GuidInscricao}" data-nome="${v.Nome}">Desfazer
                                    <svg viewBox="0 0 512 512"><path d="M48.5 224H40c-13.3 0-24-10.7-24-24V72c0-9.7 5.8-18.5 14.8-22.2s19.3-1.7 26.2 5.2L98.6 96.6c87.6-86.5 228.7-86.2 315.8 1c87.5 87.5 87.5 229.3 0 316.8s-229.3 87.5-316.8 0c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0c62.5 62.5 163.8 62.5 226.3 0s62.5-163.8 0-226.3c-62.2-62.2-162.7-62.5-225.3-1L185 183c6.9 6.9 8.9 17.2 5.2 26.2s-12.5 14.8-22.2 14.8H48.5z" /></svg>
                                </button>
                            </li>`;
                        }
                    });
                    resultado += '</ul></li>';
                });
                if (data.length == 0) {
                    $("#selecionar-inscrito").hide();
                    resultado = "<li class='smc-gpi-nao-resultado'>Inscrito não encontrado na(s) atividade(s) selecionada(s). Confira o ingresso.</li>";
                }
                divResultado.append(resultado);
                $("#btn-salvar").attr("disabled", "disabled");
                MensagemConfirmacao();
                ClicarOfertaInscrito();
                $("#mensagem-confirmacao").hide();
            },
            error: function (data) {
                $("#mensagem-sucesso").hide();
                $("#mensagem-erro").show();
                $("#detalhes-erro").text(data.responseJSON.Mensagem);
            }
        });
    });
}

function ClicarOfertaInscrito() {
    $("input:radio").click(function () {
        $("input:radio").each(function () {
            $(this).parent().removeClass("active");
            $("#btn-salvar").removeAttr("disabled");
            $("#btn-salvar").focus();
        })

        $(this).parent().addClass("active")
    });
}

function LimparFormulario() {
    $("#btn-limpar").click(function () {
        $("#corpo-pesquisa").hide();
        $("#resultado-pesquisa").empty();
        $("#btn-salvar").attr("disabled", "disabled");
    });
}

function MensagemConfirmacao() {
    $(".smc-btn-desfazer").click(function () {
        $("#mensagem-confirmacao").show();
        var guid = $(this).attr("data-guid");
        var nome = $(this).attr("data-nome");
        $("#botao-mensagem-confirmacao-sim").attr("data-guid", guid);
        $("#botao-mensagem-confirmacao-sim").focus();
        $("#detalhes-confirmacao").html(nome);
        MensagemConfirmacaoSim();
        MensagemConfirmacaoNao();
    });


}

function MensagemConfirmacaoSim() {
    $("#botao-mensagem-confirmacao-sim").unbind("click")
    $("#botao-mensagem-confirmacao-sim").click(function (e) {        
        $.ajax({
            type: "POST",
            url: "CheckoutInscricao",
            data: JSON.stringify({ "guid": $(this).attr("data-guid") }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#form-pesquisa").submit();            
            },
            error: function (data) {

            }
        });
    });

}

function MensagemConfirmacaoNao() {
    $("#botao-mensagem-confirmacao-nao").click(function () {
        $("#mensagem-confirmacao").hide();
    });
}


