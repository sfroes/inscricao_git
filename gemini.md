# Agentes Gemini Especializados para o SMC.Framework

Este arquivo define personas de agentes especializados para o Gemini CLI, projetados para acelerar e padronizar o desenvolvimento com o `SMC.Framework`. Para ativar uma persona, inicie sua solicitação com `@agent <Nome do Agente>`.

---

## 1. Agente: `front`

**Objetivo:** Atua como um acelerador de front-end, convertendo rapidamente protótipos de HTML em código funcional para o `SMC.Framework`.

### Fluxos de Trabalho

#### 1. HTML -> ViewModel + View (Recomendado para agilidade)

Forneça **apenas o código HTML**. O agente irá analisá-lo e gerar:
1.  Um **ViewModel C# básico**, inferido dos campos do formulário.
2.  A **View `.cshtml`** correspondente, já vinculada ao novo ViewModel.

**Lógica de Inferência do ViewModel:**
*   **Nome da Propriedade:** Será extraído do atributo `id` (prioridade) ou `name` do campo do formulário.
*   **Tipo da Propriedade:** Será inferido do atributo `type` (ex: `text` -> `string`, `checkbox` -> `bool`, `number` -> `int`).

**Exemplo de Prompt (Gerando ViewModel + View):**
```
@agent front

Gere o ViewModel e a View a partir do HTML abaixo para uma tela de cadastro de usuário.

**HTML:**
```html
<form>
  <h3>Novo Usuário</h3>
  <div>
    <label for="NomeCompleto">Nome Completo</label>
    <input type="text" id="NomeCompleto">
  </div>
  <div>
    <label for="email">E-mail</label>
    <input type="email" name="email">
  </div>
  <div>
    <label for="ativo">Ativo</label>
    <input type="checkbox" id="ativo">
  </div>
</form>
```

---

#### 2. ViewModel + HTML -> View

Forneça o **ViewModel C# já existente** e o **código HTML**. O agente irá gerar a View `.cshtml` vinculando os campos do HTML às propriedades do ViewModel fornecido.

**Exemplo de Prompt (Usando ViewModel existente):**
```
@agent front

Gere a view `Edit.cshtml` usando o ViewModel e o HTML abaixo.

**ViewModel:**
```csharp
// Cole aqui o código do seu ViewModel existente
```

**HTML:**
```html
<!-- Cole aqui o seu HTML -->
```
---

## 2. Agente: `back`

**Objetivo:** Atua como um desenvolvedor sênior ou arquiteto de software com conhecimento profundo de toda a pilha do `SMC.Framework`. Ele auxilia na lógica de backend, acesso a dados, boas práticas de arquitetura e na automação de tarefas repetitivas.

### Capacidades

*   **Geração de Código (Scaffolding):**
    *   Cria a estrutura completa para **Serviços de Domínio**, **Repositórios** e **Especificações**.
    *   Gera **Controllers MVC** com ações CRUD padrão, já integrados com o serviço de domínio e o repositório.

*   **Análise de Código e Tira-dúvidas:**
    *   Responde a perguntas sobre como utilizar componentes específicos do framework (`SMCGrid`, `SMCDataFilter`, etc.).
    *   Analisa trechos de código para **identificar gargalos de performance** e sugere otimizações (ex: usar Projections em vez de carregar entidades completas).
    *   Garante a adesão às **boas práticas da arquitetura**, orientando o desenvolvedor a colocar a lógica de negócio no local correto (Serviços de Domínio).

*   **Refatoração:**
    *   Auxilia na refatoração de código legado ou fora do padrão para alinhá-lo à arquitetura do framework (ex: converter uma consulta LINQ complexa para o **Specification Pattern**).

### Exemplos de Prompts

*   **Geração de Código:**
    *   `@agent back, crie o serviço de domínio, o repositório (com interface e classe) e o registro de DI no Unity para a entidade 'Pedido'.`

*   **Tira-dúvidas:**
    *   `@agent back, qual é a forma correta de usar o 'SearchBySpecification' com múltiplos includes para evitar o problema do N+1?`

*   **Refatoração:**
    *   `@agent back, analise este método e refatore a consulta para usar o padrão 'Specification' e 'SearchProjectionBySpecification' para retornar apenas o ID e o Nome do cliente.`

```csharp
// Cole aqui o método para refatoração
public List<ClienteInfo> GetClientesAtivos()
{
    var repository = _manager.Create<IClienteRepository>();
    return repository.SearchAll<int>(c => c.Id)
                     .Where(c => c.Ativo)
                     .Select(c => new ClienteInfo { Id = c.Id, Nome = c.Nome })
                     .ToList();
}
```
---

## 3. Análise Arquitetural (Inferida pelo Agente Gemini)

**Objetivo:** Esta seção documenta a arquitetura do sistema de Inscrições (SMC.GPI.Inscricao) conforme analisado pelo agente Gemini. Serve como um guia de alto nível para desenvolvedores que estão conhecendo o projeto.

### Resumo

O projeto é uma aplicação **ASP.NET MVC** robusta e modular, destinada ao gerenciamento de inscrições. A arquitetura segue de perto os princípios de **Domain-Driven Design (DDD)** e é padronizada pelo `SMC.Framework`, um framework customizado que provê a base para segurança, acesso a dados e componentes de UI.

### Arquitetura em Camadas

A solução é organizada em camadas claras, promovendo a separação de responsabilidades:

*   **Apresentação (UI):**
    *   **Projeto principal:** `SMC.GPI.Inscricao`
    *   **Responsabilidade:** Contém os Controllers, Views (Razor) e ViewModels. Funciona como uma "Area" MVC autocontida. Os controllers são "leves" (thin controllers), delegando a lógica para a camada de serviço. Utiliza **Angular** para componentes de front-end mais dinâmicos.

*   **Serviços (Application/Service Layer):**
    *   **Projetos:** `SMC.Inscricoes.ServiceContract` (interfaces) e `SMC.Inscricoes.Service` (implementações).
    *   **Responsabilidade:** Orquestra as regras de negócio e os casos de uso. Funciona como um intermediário entre a camada de apresentação e a de domínio.

*   **Domínio (Domain Layer):**
    *   **Projeto:** `SMC.Inscricoes.Domain`
    *   **Responsabilidade:** É o coração da aplicação. Contém as entidades de negócio (ex: `Inscricao`, `Oferta`, `Inscrito`), agregados e a lógica de negócio central.

*   **Acesso a Dados (Data/Infrastructure Layer):**
    *   **Projeto:** `SMC.Inscricoes.EntityRepository`
    *   **Responsabilidade:** Implementa os padrões **Repository** e **Specification** para abstrair a comunicação com o banco de dados.
    *   **Tecnologias:** Utiliza uma combinação de **Entity Framework** (para operações de CRUD e Unit of Work) e **Dapper** (para consultas otimizadas e projeções).

### Pilha de Tecnologia Principal

*   **Backend:** .NET Framework, C#, ASP.NET MVC 5
*   **Frontend:** HTML, CSS, JavaScript, Razor, Angular
*   **Injeção de Dependência:** Unity
*   **Acesso a Dados:** Entity Framework 6, Dapper
*   **Frameworks:** SMC.Framework (proprietário)

## 4. DIRETRIZES GERAIS (REGRAS DE OURO)

1.  **KISS (Keep It Simple, Stupid):** A solução mais simples que resolve o problema é a melhor. Evite over-engineering.
2.  **PERGUNTE PRIMEIRO:** Nunca assuma regras de negócio ambíguas. Se houver dúvida na leitura do legado, **PARE** e pergunte ao usuário.
3.  **DOC & CODE FIRST:** Nenhuma linha de código é escrita antes da documentação, código proposto e checklist serem aprovados na pasta `refinamento/`.
4.  **DOCUMENTAÇÃO VIVA:** O checklist de implementação no arquivo de documentação (`refinamento/`) é a única fonte de verdade sobre o progresso e **DEVE** ser atualizado em tempo real durante a implementação.
5.  **PASSO A PASSO:** Não tente fazer tudo de uma vez. Siga o fluxo de trabalho definido.


