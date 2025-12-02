# Instruções do Agente para Conversão de HTML para SMC.Framework.UI.Mvc

Este documento descreve o processo para o agente de IA converter um arquivo HTML padrão em uma visão Razor (`.cshtml`) usando os componentes da biblioteca `SMC.Framework.UI.Mvc`.

## Princípio Fundamental

O objetivo principal é mapear elementos HTML semânticos e padrões de UI comuns (como formulários, tabelas e abas) para os métodos de auxílio (HTML Helpers) fortemente tipados correspondentes, fornecidos pelo `SMC.Framework.UI.Mvc`. O agente analisará o HTML fornecido, identificará esses padrões e gerará a sintaxe Razor equivalente.

O fluxo de trabalho do agente é o seguinte:

1.  **Analisar o ViewModel:** O agente deve primeiro receber a classe C# do ViewModel que será usada para a visão. As propriedades desta classe são cruciais para gerar os auxiliares fortemente tipados.
2.  **Mapear HTML para os Helpers SMC:** O agente analisará a estrutura HTML e substituirá os elementos padrão por seus equivalentes `@Smc.` ou `@Html.SMC...`.
3.  **Gerar a Visão Razor:** A saída será um único arquivo `.cshtml` que usa os helpers SMC e está vinculado ao ViewModel especificado.

## Mapeamento de Elementos HTML para Helpers SMC

O agente usará o seguinte mapeamento como referência. Esta lista não é exaustiva e será expandida à medida que o agente aprender mais sobre o framework.

| Elemento / Padrão HTML | Helper SMC Equivalente (Exemplo) | Notas |
| --- | --- | --- |
| `<form>` | `@using (Html.BeginForm()) { ... }` | Manipulação de formulário padrão do MVC. |
| `<label for="Nome">Nome</label>` | `@Html.SMCLabelFor(m => m.Nome)` | Associa o rótulo a uma propriedade do modelo. |
| `<input type="text" id="Nome">` | `@Html.SMCTextBoxFor(m => m.Nome)` | Cria uma caixa de texto vinculada à propriedade do modelo. |
| `<input type="password">` | `@Html.SMCPasswordFor(m => m.Senha)` | Cria um campo de senha. |
| `<input type="checkbox">` | `@Html.SMCCheckBoxFor(m => m.Ativo)` | Cria uma caixa de seleção estilizada. |
| `<input type="radio">` | `@Html.SMCRadioButtonFor(m => m.Opcao)` | Cria um botão de rádio estilizado. |
| `<textarea>` | `@Html.SMCTextAreaFor(m => m.Descricao)` | Cria uma área de texto. |
| `<table>` | `@(Html.SMCGrid<MeuViewModel.GridItem>() ... )` | Uma tabela HTML simples será convertida em uma grade SMC. Requer um modelo específico para os itens da grade. |
| `<div>` (com estrutura de abas) | `@(Html.SMCTabs() ... )` | Para conteúdo em abas. |
| `<div>` (com acordeão) | `@(Html.SMCAccordion() ... )` | Para painéis de acordeão. |
| `<button type="submit">` | `@Html.SMCSubmitButton("Salvar")` | Cria um botão de envio estilizado. |

## Exemplo de Conversão

Dado um ViewModel:

```csharp
public class UsuarioViewModel
{
    [Display(Name = "Nome de Usuário")]
    public string NomeUsuario { get; set; }

    [DataType(DataType.Password)]
    public string Senha { get; set; }

    public bool Ativo { get; set; }
}
```

E o seguinte HTML simples:

```html
<form>
    <div>
        <label for="NomeUsuario">Nome de Usuário</label>
        <input type="text" id="NomeUsuario">
    </div>
    <div>
        <label for="Senha">Senha</label>
        <input type="password" id="Senha">
    </div>
    <div>
        <label for="Ativo">Ativo</label>
        <input type="checkbox" id="Ativo">
    </div>
    <button type="submit">Enviar</button>
</form>
```

O agente produzirá o seguinte arquivo `.cshtml`:

```csharp
@model MeuProjeto.ViewModels.UsuarioViewModel

@using (Html.BeginForm("Salvar", "Usuario", FormMethod.Post))
{
    @Html.ValidationSummary(true)

    <fieldset>
        <legend>Detalhes do Usuário</legend>

        <div class="editor-label">
            @Html.SMCLabelFor(model => model.NomeUsuario)
        </div>
        <div class="editor-field">
            @Html.SMCTextBoxFor(model => model.NomeUsuario)
            @Html.ValidationMessageFor(model => model.NomeUsuario)
        </div>

        <div class="editor-label">
            @Html.SMCLabelFor(model => model.Senha)
        </div>
        <div class="editor-field">
            @Html.SMCPasswordFor(model => model.Senha)
            @Html.ValidationMessageFor(model => model.Senha)
        </div>

        <div class="editor-label">
            @Html.SMCLabelFor(model => model.Ativo)
        </div>
        <div class="editor-field">
            @Html.SMCCheckBoxFor(model => model.Ativo)
            @Html.ValidationMessageFor(model => model.Ativo)
        </div>

        <p>
            @Html.SMCSubmitButton("Enviar")
        </p>
    </fieldset>
}
```

## Limitações e Envolvimento do Desenvolvedor

O agente foi projetado para lidar com o scaffolding inicial e a conversão da visão. Ele não substitui um desenvolvedor. As seguintes tarefas ainda exigirão intervenção manual:

*   **Lógica Complexa:** O agente não pode implementar lógica de negócios complexa, manipulação de eventos ou atualizações dinâmicas da UI.
*   **Configuração da Fonte de Dados:** A configuração das fontes de dados para controles complexos como Grids ou TreeViews deve ser feita por um desenvolvedor.
*   **Estilização e Scripts Personalizados:** Embora os componentes tenham um estilo padrão, qualquer CSS ou JavaScript personalizado precisará ser adicionado manualmente.
*   **Criação do ViewModel:** O agente requer um ViewModel preexistente para funcionar.

Este processo acelerará significativamente o desenvolvimento da UI, automatizando a tarefa repetitiva de converter esboços de HTML em visões do `SMC.Framework.UI.Mvc`.