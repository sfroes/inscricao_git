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
        if (selected == undefined || $(selected).length == 0) {
            //desabilitar botoes

            $('#divBototesLote').find('button').attr('disabled', 'disabled').addClass('smc-btn-desabilitado');
        } else {
            //habilitar botoes
            $('#divBototesLote').find('button').removeAttr('disabled').removeClass('smc-btn-desabilitado');
        }

    }).attr('data-gpi-lote-configurated', 'true');
});