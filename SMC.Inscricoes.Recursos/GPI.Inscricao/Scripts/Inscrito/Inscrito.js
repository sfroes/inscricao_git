

$(document).on('smcload', function () {
    
    var nacionalidade, select;

    nacionalidade = $("#Nacionalidade");

    if (nacionalidade.attr('data-custom-configurated') == undefined) {

        nacionalidade.attr('data-custom-configurated', 'true');

        select = new smc.core.uicontrol($('#CodigoPaisNacionalidade'));

        //Se a nacionalidade Brasileira for selecionada, trava a opção de selecionar o pais de origem e seleciona o Brasil
        nacionalidade.on('change', function () {
            if (smc.core.uicontrol(nacionalidade).getValue() == '1') {
                select.setValue(800);                              
            }
        });
    }

});