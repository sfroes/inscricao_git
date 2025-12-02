$(document).on('smcload', function () {
    var btn = $('button[data-behavior="ExcluirTaxaEmLote"]');
    if ($(btn).length>0 && $('#divExclusaoTaxaLote').find('input[type="checkbox"]:checked').length == 0) {
        smc.button($(btn)).disable('BTNEXCLUIR');
    }
    
    $('#divExclusaoTaxaLote').not('data-gpi-configurated').attr('data-gpi-configurated', 'true').find('input[type="checkbox"]').change(function () {
        var btn = $('button[data-behavior="ExcluirTaxaEmLote"]'),
            buttonObj = new smc.button($(btn));
        if (this.checked) {            
            buttonObj.enable('BTNEXCLUIR');
        } else {
            var divContainer = $(this).closest('div[id="divExclusaoTaxaLote"]');
            var checkeds = $(divContainer).find('input[type="checkbox"]:checked');
            if (checkeds.length == 0) {                
                buttonObj.disable('BTNEXCLUIR');
            }
        }
    });

    var btn = $('button[data-idmodal="modalIncluirTaxaLote"]');
    if ($(btn).length > 0 && $('#divInclusaoTaxaLote').find('input[type="checkbox"]:checked').length == 0)
    {
        smc.button($(btn)).disable('BTINCLUIR');
    }    
    $('#divInclusaoTaxaLote').not('data-gpi-configurated').attr('data-gpi-configurated', 'true').find('input[type="checkbox"]').change(function () {
        var btn = $('button[data-idmodal="modalIncluirTaxaLote"]'),
            buttonObj = new smc.button($(btn));
        if (this.checked) {            
            buttonObj.enable('BTINCLUIR');
        } else {
            var divContainer = $(this).closest('div[id="divInclusaoTaxaLote"]');
            var checkeds = $(divContainer).find('input[type="checkbox"]:checked');
            if (checkeds.length == 0) {
                buttonObj.disable('BTINCLUIR');
            }
        }
    });
});