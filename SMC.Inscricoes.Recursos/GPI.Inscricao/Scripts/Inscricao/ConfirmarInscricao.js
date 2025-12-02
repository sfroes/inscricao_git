
$(document).ready(function () {
    ModificarTabelaTdVazio("formulario");
    FildsetCamposVazios("formulario");
    $(document).on('smcconfirmed', function () {
        smc.core.displayLoadingPanel();
    });
});

function ModificarTabelaTdVazio(idTable) {
    var cabecalhos = $("#" + idTable).find("table th");
    var linhas = $("#" + idTable).find("table tr");

    for (var i = 0; i < cabecalhos.length; i++) {

        for (var l = 0; l < linhas.length; l++) {
            if ($(linhas[l]).find("td").eq(i).find("div").text()) {
                $(linhas[l]).find("td").eq(i).find("div").before("<label class='smc-grid-label'>" + $(cabecalhos[i]).html() + "</label>");

            } else {                
               $(linhas[l]).find("td").eq(i).hide();
            }
        }
    }

    $("#" + idTable).find("table thead").hide();
}

function FildsetCamposVazios(iDiv) {
    var fieldSets = $("#" + iDiv).find("fieldset");

    for (var i = 0; i < fieldSets.length; i++) {
        var divsDisplay = $(fieldSets[i]).find("div[class='smc-display']");
        var divsHide = 0;
        for (var l = 0; l < divsDisplay.length; l++) {
            var ps = $(divsDisplay[l]).find('p');
            $(ps).each(function () {
                if (!$(this).text()) {
                    $(divsDisplay[l]).hide();
                    $(divsDisplay[l]).parent().hide();
                    divsHide++;
                }
            });

            if (divsDisplay.length === divsHide) {
                $(fieldSets[i]).hide();
            }
        }
    }
}
