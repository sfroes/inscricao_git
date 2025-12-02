function showLoadingPage() {
    smc.core.displayLoadingPanel();
} function hideLoadingPage() {
    setTimeout(function () {
        smc.core.hideLoadingPanel();
    },1000);
}
$(window).on("load", function () {
    //Flag para campo formulario seminario == campo projeto
    var idSelectProjeto = BuscarLabelForm("Projeto");
    var formularioSeminario = idSelectProjeto ? true : false;

    //Caso exista aplica-se a regra do dependecy
    //Ao preencher o formulário, se o tipo de processo do processo em questão estiver configurado para integrar com o GPC e existir o campo PROJETO,
    //ao selecionar um valor para este campo, preencher automaticamente os campos abaixo, da seguinte forma:
    // AREA_DE_CONHECIMENTO: Descrição da área de conhecimento informada para o projeto selecionado.
    // NOME_ORIENTADOR: Nome do orientador * informado para o projeto selecionado.
    // EMAIL_ORIENTADOR: E - mail do orientador * informado para o projeto selecionado.
    // ALUNOS: Para cada aluno associado ao projeto no GPC, incluir um novo registro no detalhe editável, preenchendo no campo NOME_ALUNO o nome do respectivo aluno.
    // * Para os projetos FAPEMIG, retornar os dados do orientador.Para os projetos PIBIC / PIBIT, FIP / Projeto de Pesquisa, IC Voluntário, retornar os dados do coordenador.
    if (formularioSeminario) {
        //Desabilita os campos do MasterDetails do Aluno
        DesabilitarMasterDetails();
        var selectProjeto = $('select[name="' + idSelectProjeto + '"]');
        selectProjeto.on("change", function () {
            //recupera se valor do select de projeto
            var valorSelect = $(this).val();            

            if (valorSelect) {
                //recupera o id do projeto
                var seqProjeto = valorSelect.split('|')[0];
                var seqInscricao = $('input[name="SeqInscricaoEncrypted"').val();
                var seqProcesso = $('input[name="SeqProcesso"').val();
                showLoadingPage();
                $.ajax({
                    type: 'POST',
                    url: 'DadosFormularioSeminarioSGF',
                    data: { seqProjeto: seqProjeto, seqProcesso: seqProcesso, seqInscricao: seqInscricao },
                    async: false
                }).done(function (data) {
                    //Preenche os campos
                    PreencherAreaConhecimento(data.AreaConhecimento);
                    PreencherOrientadorEmail(data.NomeOrientador, data.EmailOrientador);
                    PreencherAlunos(data.Alunos);
                    hideLoadingPage();
                });
            }            
        });        
    }

    //Busca o campo dentro do formulário
    //Recebe o nome do campo através do label
    function BuscarLabelForm(labelSearch) {
        //Recupera todos os labels para identificar os campos
        var labelsForm = $("#formulario label");
        var idLabel = '';
        labelsForm.each(function (index) {
            if ($(this).text() === labelSearch) {
                //Recupera o nome do campo que é o id da div
                idLabel = $(this).parent().attr("id");
                return;
            }
        });
        return idLabel;
    }

    //Função para descobrir o Id do campo dentro do fieldset
    //Recebe o nome do fieldset e o nome do campo através do label
    function BuscarFielSetCampo(fieldSet, labelSearch) {
        //Recupera todos as legendas para identificar os campos
        var fieldsSetLegend = $('fieldset legend');
        var idLabel = '';
        fieldsSetLegend.each(function (index) {
            if ($(this).text() === fieldSet) {
                //Recupera o nome do fieldset que é o id da div
                var idFielSet = $(this).parent().attr("id");
                //Recuperar todos os labels do fieldset para encontrar o campo
                var labelsFieldsSet = $("#" + idFielSet + " label");
                labelsFieldsSet.each(function () {
                    if ($(this).text() === labelSearch) {
                        idLabel = $(this).parent().attr("id");
                        return;
                    }
                })
            }
        });

        return idLabel;
    }

    //Preenche a Area de conhecimento
    function PreencherAreaConhecimento(valorAreaConhecimento) {
        idAreaConhecimento = BuscarLabelForm("Área de conhecimento");
        var campoAreaConhecimento = $('input[name="' + idAreaConhecimento + '"]');
        campoAreaConhecimento.val(valorAreaConhecimento);
    }

    //Preenche o Orientador
    function PreencherOrientadorEmail(nomeOrientador, emailOrientador) {
        idNomeOrientador = BuscarFielSetCampo("Orientador", "Nome")
        var campoNomeOrientador = $('input[name="' + idNomeOrientador + '"]');
        campoNomeOrientador.val(nomeOrientador);

        idEmailOrientador = BuscarFielSetCampo("Orientador", "E-mail")
        var campoEmailOrientador = $('input[name="' + idEmailOrientador + '"]');
        campoEmailOrientador.val(emailOrientador);
    }

    //Preenche alunos
    function PreencherAlunos(alunos) {
        var mastersDetailsAluno = $("span.smc-detalhe-editavel-titulo");
        mastersDetailsAluno.each(function () {
            idMasterDetails = '';
            if ($(this).text() === "Alunos") {
                idMasterDetails = $(this).parent().attr('id');
                PreencheLinhasMasterDetails(idMasterDetails, alunos);
            }
        });
    }

    //Preenche o MasterDetails
    function PreencheLinhasMasterDetails(idMasterDetails, dados) {
        var containerMasterDetails = $('div[id="' + idMasterDetails +'"] div.smc-detalhe-editavel-organizador div[data-type="smc-editabledetail-data-container"]');
        var linhasMasterDetails = $('div[id="' + idMasterDetails + '"] div.smc-detalhe-editavel-organizador div[data-type="smc-editabledetail-data-container"] div.smc-detalhe-editavel-linha-dados');
        var identificadorCampo = linhasMasterDetails.find($("div[data-control='textbox']")).attr("id").split("__")[1].split("_")[1];

        linhasMasterDetails.each(function () {
            $(this).remove();
        });

        for (var i = 0; i < dados.length; i++) {
            var newLinha = CriarLinha(dados[i].Nome, idMasterDetails, identificadorCampo, i);
            $(containerMasterDetails).append(newLinha);
        }        
    }

    //Cria as linhas do MasterDetails
    function CriarLinha(value, idMasterDetail, identificadorCampo, index) {
        var campoId = idMasterDetails + "_" + index + "__Campo_" + identificadorCampo;
        var spanDataId = idMasterDetails + "[" + index + "].Campo_" + identificadorCampo;
        var inputDataName = "Campo_" + identificadorCampo;
        var inputName = idMasterDetails + "[" + index + "].Campo_" + identificadorCampo;
        var inputNameIdCorrelacao = idMasterDetails + "[" + index + "].IdCorrelacao";
        var spanDataValmsgFor = idMasterDetails + "[" + index + "].Campo_" + identificadorCampo;
        var valueInput = value;
        var divDataType = idMasterDetails + "_" + index + "__IdCorrelacao";
        var spanDataValmsgForCorrelacao = idMasterDetails + "[" + index + "].IdCorrelacao";
        return `<div class="smc-detalhe-editavel-linha-dados" data-control-child="masterdetail">
            <div class="smc-size-md-24" data-type="smc-editabledetail-data-line">
                <div class="smc-size-md-12 smc-size-xs-24 smc-size-sm-12 smc-size-lg-12" data-control="textbox" data-type="smc-textbox" id=${campoId} data-configurated="true">
                    <span class="hidden" data-id="${spanDataId}" data-type="smc-required-span"></span><label>Nome</label><input class="smc-horizontal-align-left " data-name="${inputDataName}" data-parsley-required="false" data-parsley-required-message="Preenchimento obrigatório" data-parsley-unique="true" data-parsley-unique-message="Campo com valor repetido" data-unique-ignorecase="true" data-unique-ignorediacritics="true" name="${inputName}" readonly="readonly" tabindex="-1" type="text" value="${valueInput}" data-oldvalue=""><span class="field-validation-valid span-validation-error" data-valmsg-for="${spanDataValmsgFor}" data-valmsg-replace="true"></span></div>
                    <div data-control="hidden" data-type="smc-hidden" id="${divDataType}" data-configurated="true"><input data-name="IdCorrelacao" name="${inputNameIdCorrelacao}" type="hidden" value="00000000-0000-0000-0000-000000000000" data-oldvalue="00000000-0000-0000-0000-000000000000"><span class="field-validation-valid span-validation-error smc-span-error-hidden" data-valmsg-for="${spanDataValmsgForCorrelacao}" data-valmsg-replace="true"></span></div>
                        <div class="smc-detalhe-editavel-estrutura"></div>
                    </div>
                </div>`;
    }

    function DesabilitarMasterDetails() {
        var mastersDetailsAluno = $("span.smc-detalhe-editavel-titulo");
        mastersDetailsAluno.each(function () {
            idMasterDetails = '';
            if ($(this).text() === "Alunos") {
                idMasterDetails = $(this).parent().attr('id');
                $('div[id="' + idMasterDetails + '"] div.smc-buttonset-horizontal-primario div.smc-button button.smc-detalhe-editavel-botao-inserir').attr("disabled", "disabled");
                var buttonExcluirMasterDetails = $('div[id="' + idMasterDetails + '"] div.smc-detalhe-editavel-organizador div[data-type="smc-editabledetail-data-container"] div[data-control="button"]');
                buttonExcluirMasterDetails.each(function () {
                    $(this).hide();
                });
            }
        });
        
    }
    
});