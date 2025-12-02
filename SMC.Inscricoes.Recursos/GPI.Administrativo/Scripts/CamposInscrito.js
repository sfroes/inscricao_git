$(document).ready(function () {
    $('#select_SeqTipoProcesso').change(function () {
        var url = $('div[data-dependency-controlname="CamposInscrito"]').attr('data-dependency-url');
        var seq = $(this).val();
        $.ajax(
           {
               url: url,
               type: 'POST',
               data: JSON.stringify({ seqTipoProcesso: seq }),
               contentType: "application/json; charset=utf-8",
               dataType: "html",
                success: function (data, textStatus, jqXHR) {
                    var campos = $('#tabCamposCadastroInscrito');
                    campos.find('input[type="checkbox"]').each(function () {
                        var input = $(this);                        
                        var disabled = false;
                        //um arry do resultado transformando a string em array
                        var arrayResult = data.replace('[', '').replace(']', '').split(',');
                        //percorre o array e verifica se o valor do input está no array
                        for (var i = 0; i < arrayResult.length; i++) {
                            if (arrayResult[i] == input.val()) {
                                disabled = true;
                            }
                        }
                        //remove a propriedade readonly do input
                        input.removeAttr('readonly');
                        if (disabled) {
                            //remove a proprieadade checked do input para que no click ele seja marcado novamente
                            input.removeAttr('checked')
                            input.click();
                            input.attr('readonly', 'readoly');
                        }
                    });
                }
           });
    });
});