$(document).ready(function () {
    var exibirFormularioImpacto = $("#exibir-formulario-impacto").val();
    if (exibirFormularioImpacto == "True") {
        $("#btn-formulario-impacto").click();
    }
    ConfigurarEstrelas();
});

function ConfigurarEstrelas() {
    CarregarClickEstrelas();
    $("#btn-formulario-impacto").click(function () {
        CarregarClickEstrelas();
    });
}

function CarregarClickEstrelas() {
    var debouce = setInterval(function () {
        var formularioImpacto = $("#formulario-impacto");
        if (formularioImpacto.length > 0) {
            //Ajustes para quando tiver master detalhes
            var btnMasterDetail = $('button[data-behavior="smc-editabledetail-insert"]');
            if (btnMasterDetail.length > 0) {
                btnMasterDetail.click(function () {
                    ControlarClickEstrelas();
                });
            }
            clearInterval(debouce);
            ControlarClickEstrelas();
        }
    }, 500);
}

function ControlarClickEstrelas() {
    $('input[type=radio]').click(function () {
        var valorInputOriginal = +$(this).val().split("|")[0];
        var divmae = $(this).closest('div.smc-radiobutton-list');
        divmae.find("input:radio").each(function () {
            var valorInputAtual = +$(this).val().split("|")[0];
            var parent = $(this).closest("div.smc-radiobutton");
            parent.removeClass("estrela-amarela");
            if (valorInputAtual < valorInputOriginal) {
                parent.addClass("estrela-amarela");
            }
        })
    })
}

