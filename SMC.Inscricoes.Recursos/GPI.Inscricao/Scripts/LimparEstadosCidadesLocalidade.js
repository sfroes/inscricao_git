
$(window).ready(function () {
    //Ocorre em intervalos de 2 segundos para verificar se o campo de nacionalidade foi preenchido
    setInterval(init, 2000);

    //variaveis de contexto
    function init() {
        var codigoPaisNacionalidade = $("#select_CodigoPaisNacionalidade");
        var ufNaturalidade = $("#select_EstadoCidade_Estado");
        var cidadeNaturalidade = $("#select_EstadoCidade_SeqCidade");
        var nacionalidade = $("#select_Nacionalidade");
        var descricaoNaturalidade = $("#DescricaoNaturalidadeEstrangeira");
        var fieldDescricaoNaturalidade = $("[name='DescricaoNaturalidadeEstrangeira']");

        // Observa se o pais de origem mudou de brasil para limpar os campos de cidade e uf
        $(codigoPaisNacionalidade).on("change", function () {
            if ($(this).val() !== '800') {
                ufNaturalidade.val("").change();
                cidadeNaturalidade.val("");
            }
            else {
                fieldDescricaoNaturalidade.val("");
            }
        });

        //Em caso de exception limpa os campos de cidade e uf
        if (codigoPaisNacionalidade.val() !== '800') {
            ufNaturalidade.val("").change();
            cidadeNaturalidade.val("");
        }
        else {
            fieldDescricaoNaturalidade.val("");
        }

        //Observa a Nacionalidade para ver se foi escolhida brasileira para fazer os depencies necessarios
        $(nacionalidade).on("change", function () {
            if ($(this).val() === '1') {
                // Esconde a descrição da naturalidade e tira a obrigatoriedade
                fieldDescricaoNaturalidade.attr('data-parsley-required', false);
                descricaoNaturalidade.hide();
                //Exibe as div's dos controles
                $('div[data-type="smc-statecity"]').show();
                $('#EstadoCidade_Estado').show();
                //Manipula as uf
                ufNaturalidade.removeAttr('disabled')
                $('span[data-id="EstadoCidade.Estado"]').removeClass('hidden');
                ufNaturalidade.attr('data-parsley-required', true);
                //Manipula a cidade
                $('#EstadoCidade_SeqCidade').show();
                cidadeNaturalidade.attr('data-parsley-required', true)
                $('span[data-id="EstadoCidade.SeqCidade"]').removeClass('hidden');
                //Change na mudança de nacionalidade automatica
                codigoPaisNacionalidade.val('800').change();
            }
        });

        //Observa se a uf mudou para remover o disable do select de cidade
        $(ufNaturalidade).on("change", function () {
            if ($(this).val() !== '') {
                cidadeNaturalidade.removeAttr('disabled');
            }
        });
    }
});

