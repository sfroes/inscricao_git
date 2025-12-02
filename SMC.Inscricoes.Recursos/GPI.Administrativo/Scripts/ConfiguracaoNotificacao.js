
$(document).on('smcload', function () {
    var field, div;

    field = $('#SeqTipoNotificacao').not('[data-custom-configurated]');
    div = $('#ObsTipoNotificacao');

    var requestInfo = function () {
        $.ajax({
            method: "POST",
            url: div.attr('data-url'),
            data: { seqTipoNoficicacao: new smc.select(field).getValue() },
            success: function (data) {
                div.show();
                div.find('span').html(data);
            }
        });
    };

    if (field.length > 0) {
        field.on('change', function () {            
            requestInfo();
        });

        if (new smc.select(field).getValue() > 0)
            requestInfo();
    }

    field.attr('data-custom-configurated', 'true');
});