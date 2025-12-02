$(document).on('smcload', function ()
{
    var fieldSet = $('#fieldsetRecuperarPaginaEtapa');
    if (fieldSet.length>0)
    {
        var mensagem = $(fieldSet).parent().find('p[data-type="gpi-mensagem"]');
        var botaoSalvar = $(fieldSet).closest('form').find('button[type="submit"]');
        if (mensagem.length>0) {
            $(botaoSalvar).attr('style','display:none!important;')
        } else
        {
            $(botaoSalvar).removeAttr('style');
        }
    }
    fieldSet = $('#fieldsetAlterarIdiomaEtapa');
    if (fieldSet.length > 0)
    {
        var idiomas = $(fieldSet).find('input[type="checkbox"]');
        var botaoSalvar = $(fieldSet).closest('form').find('button[type="submit"]');
        if (idiomas.length == 0) {
            $(botaoSalvar).attr('style', 'display:none!important;')
        } else {
            $(botaoSalvar).removeAttr('style');
        }
    }

    
})