$(document).on('smcload', function () {
    var valuesSelected = $('[data-type="smc-datagrid-container"]').find('input[name="SelectedValues"]');
    if (valuesSelected == undefined || $(valuesSelected).length == 0) {
        $('#divBototesLote').find('button').attr('disabled', 'disabled').addClass('smc-btn-desabilitado');
    } else {
        $('#divBototesLote').find('button').removeAttr('disabled').removeClass('smc-btn-desabilitado');
    }
    $('[data-type="smc-datagrid-container"]').not('[data-gpi-lote-configurated]').find('input[type="checkbox"]').on('change', function () {
        var checkBox = $(this);
        var grid = $(checkBox).closest('[data-type="smc-datagrid-container"]')
        var selected = $(grid).find('input[name="SelectedValues"]');
        var situacao = checkBox.closest('tr').find("td[data-model-name=ListarInscritosChekinLote_PossuiCheckin] p").text();
        console.log(situacao);
        if (selected == undefined || $(selected).length == 0) {
            //desabilitar botoes

            $('#divBototesLote').find('button').attr('disabled', 'disabled').addClass('smc-btn-desabilitado');
        } else {
            //habilitar botoes
            $('#divBototesLote').find('button').removeAttr('disabled').removeClass('smc-btn-desabilitado');
        }

    }).attr('data-gpi-lote-configurated', 'true');

    var realizarCheckin = $('#Btn_RealizarCheckin');
    var desfazerCheckin = $('#Btn_DesfazerCheckin');

    realizarCheckin.hide();
    desfazerCheckin.hide();

    var selectCheckinRealizado = $("#select_CheckinRealizado");

    selectCheckinRealizado.click(function () {

        selectCheckinRealizado = $(this).val();


        if (selectCheckinRealizado == "True") {
            realizarCheckin.hide();
            desfazerCheckin.show();

        }
        else if (selectCheckinRealizado == "False") {
            desfazerCheckin.hide();
            realizarCheckin.show();
        }
        else {
            realizarCheckin.hide();
            desfazerCheckin.hide();
        }
    })


    //var debounce = setInterval(function () {
    //    var selectCheckinRealizado = $("#select_CheckinRealizado").val()

    //    if (selectCheckinRealizado == "True") {
    //        $('#Btn_RealizarCheckin').hide();
    //        $('#Btn_DesfazerCheckin').show();

    //    }
    //    else if (selectCheckinRealizado == "False") {
    //        $('#Btn_DesfazerCheckin').hide();
    //        $('#Btn_RealizarCheckin').show();
    //    }
    //}, 100)

});