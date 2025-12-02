# Guia Completo de **Testes Unitários** em Angular

> Destinado a **totais iniciantes** e também a quem já tem experiência e quer uma referência rápida.

---

## 📚 Índice

1. [Por que testar?](#por-que-testar)
2. [Glossário rápido](#glossario)
3. [Ambiente & configuração](#ambiente)
4. [Estrutura de arquivos](#estrutura)
5. [Anatomia de um teste (AAA)](#aaa)
6. [Exemplo 0 — serviço simples](#exemplo0)
7. [NgModule × Stand‑alone](#standalone)
8. [Signals nos testes](#signals)
9. [Mocks e **spies**](#mocks)
10. [Testando **serviços**](#servicos)
11. [Testando **componentes** stand‑alone](#componentes)
12. [Testes assíncronos](#async)
13. [Observables & marble testing](#observables)
14. [Debugando](#debug)
15. [Cobertura de código](#cobertura)
16. [Erros comuns & soluções](#erros)
17. [Boas práticas & checklist](#boaspraticas)

---

<a name="por-que-testar"></a>

## 1. Por que testar?

| Sintoma                                | Como um teste ajuda                               |
| -------------------------------------- | ------------------------------------------------- |
| *Conserto um bug e quebro outro lugar* | Falha de teste sinaliza imediatamente.            |
| *Medo de refatorar*                    | Teste age como rede de segurança.                 |
| *Requisitos mudam*                     | Ajuste o código **e** o teste; garante aderência. |
| *Documentação viva*                    | Exemplos de uso ficam codificados.                |

---

<a name="glossario"></a>

## 2. Glossário rápido

| Termo            | Resumo                                                 |
| ---------------- | ------------------------------------------------------ |
| **Jasmine**      | Framework de asserções (`describe`, `it`, `expect`).   |
| **Karma**        | Test‑runner que abre o navegador e executa os specs.   |
| **TestBed**      | “Mini‑módulo” Angular criado para cada suíte de teste. |
| **spy / spyObj** | Mock que registra chamadas e permite definir retornos. |
| **AAA**          | "Arrange – Act – Assert", padrão de escrita de testes. |

---


| Função / objeto                                      | O que faz                                                                                                          |
| ---------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------ |
| **`describe()`**                                     | Define um grupo de testes (suite). Pode aninhar vários.                                                            |
| **`it()`**                                           | Define um teste individual (*spec*). Dentro dele você faz *arrange–act–assert*.                                    |
| **`beforeEach()`**                                   | Roda antes de **cada** `it()` dentro do `describe` atual. Serve para configurar ambiente limpo.                    |
| **`TestBed.configureTestingModule()`**               | Cria um “mini-módulo” Angular só para o teste. Aqui registramos providers, imports, etc.                           |
| **`TestBed.inject()`**                               | Pede ao Angular a instância configurada (injeção de dependência) do serviço/componente.                            |
| **`jasmine.createSpyObj('Nome', ['metodo1', ...])`** | Cria um *mock* com métodos espiões (spies). Cada método registra quantas vezes foi chamado e com quais argumentos. |
| **`spyOn(obj, 'metodo')`**                           | Transforma um método real em *spy*, permitindo verificar chamadas, alterar retorno, etc.                           |
| **`expect(…)`**                                      | Faz a asserção. Combinadores comuns: `toBe()`, `toEqual()`, `toHaveBeenCalledTimes()`, etc.                        |


---

<a name="ambiente"></a>

## 3. Ambiente & configuração

```bash
ng new my-app            # CLI já vem com Jasmine + Karma
cd my-app
ng test                 # executa em modo watch
```

**Scripts importantes (`package.json`)**:

```jsonc
"test":          "ng test",
"test:once":     "ng test --watch=false --browsers=ChromeHeadless",
"test:coverage": "ng test --code-coverage"
```

> Angular 17+ (builder *vite*) mantém os mesmos comandos.

---

<a name="estrutura"></a>

## 4. Estrutura de arquivos

```
src/
 └─ app/
    └─ feature/
       ├─ minha-feature.service.ts
       └─ minha-feature.service.spec.ts  <-- arquivo de teste paralelo
```

Regras:

* Nome **termina em** `.spec.ts`.
* Fica em qualquer sub‑pasta dentro de `src/` (ou `projects/` em workspaces).

`tsconfig.spec.json` deve incluir `"src/**/*.spec.ts"` no array `include`.

---

<a name="aaa"></a>

## 5. Anatomia de um teste (AAA)

```ts
describe('Nome da unidade', () => {
  it('deve realizar a ação X', () => {
    // 1. Arrange – preparar ambiente
    // 2. Act     – executar
    // 3. Assert  – verificar
  });
});
```

---

<a name="exemplo0"></a>

## 6. Exemplo 0 — Serviço simples

```ts
@Injectable({ providedIn: 'root' })
export class SomaService {
  soma(a: number, b: number) { return a + b; }
}
```

```ts // soma.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { SomaService } from './soma.service';

describe('SomaService', () => {
  let service: SomaService;

  beforeEach(() => {
    TestBed.configureTestingModule({}); // nada especial
    service = TestBed.inject(SomaService);
  });

  it('soma 2 + 2 = 4', () => {
    expect(service.soma(2, 2)).toBe(4);
  });
});
```

---

<a name="standalone"></a>

## 7. NgModule × Stand‑alone

| O que mudou                | Como fica no TestBed                 |
| -------------------------- | ------------------------------------ |
| **Serviço**                | `providers: [MeuService]` (igual)    |
| **Componente stand‑alone** | `imports: [MeuComponente]`           |
| **Funções `provide*()`**   | `providers: [provideHttpClient()]`   |
| **Módulos clássicos**      | `imports: [HttpClientTestingModule]` |

---

<a name="signals"></a>

## 8. Trabalhando com **signals**

```ts
// escrita
service.$contador.set(5);
// leitura
expect(service.$contador()).toBe(5);
```

> Nos testes, trate um signal como "state container" com APIs `.set()` e chamada como função.

---

<a name="mocks"></a>

## 9. Mocks e **spies**

### 9.1 `createSpyObj`

```ts
const httpSpy = jasmine.createSpyObj('HttpClient', ['get', 'post']);
httpSpy.get.and.returnValue(of({foo: 'bar'}));
```

### 9.2 Substituindo pelo provider

```ts
providers: [{ provide: HttpClient, useValue: httpSpy }]
```

### 9.3 Objeto vazio

```ts
{ provide: AlgumService, useValue: {} } // se método algum não será chamado
```

---

<a name="servicos"></a>

## 10. Testando **serviços**

1. Crie e configure spies para TODAS as dependências.
2. Monte estado interno (signals, subjects, etc.).
3. Chame o método → verifique retornos **e** efeitos colaterais (chamadas a spies).

*Exemplo*: ver teste de `ordernar()` e `apagarOferta()` mostrado anteriormente.

---

<a name="componentes"></a>

## 11. Testando **componentes** stand‑alone

```ts
import { MeuComponent } from './meu.component';
import { RouterTestingModule } from '@angular/router/testing';

beforeEach(async () => {
  await TestBed.configureTestingModule({
    imports: [MeuComponent, RouterTestingModule]
  }).compileComponents();

  fixture = TestBed.createComponent(MeuComponent);
  fixture.detectChanges();
});
```

Verifique template via `fixture.nativeElement` ou `By.css()` (Test Harness).

---

<a name="async"></a>

## 12. Testes assíncronos

| Cenário                | Como testar                                                |
| ---------------------- | ---------------------------------------------------------- |
| `setTimeout` ou timers | `fakeAsync` + `tick(tempo)`                                |
| Promises               | `waitForAsync` ou `fakeAsync`                              |
| Observables            | Assinar direto ou usar marbles (rxjs-marbles/jest-marbles) |

Exemplo com timer:

```ts
it('muda flag após 500 ms', fakeAsync(() => {
  service.iniciar();
  tick(500);
  expect(service.flag()).toBeTrue();
}));
```

---

<a name="observables"></a>

## 13. Observables & marble testing (extra)

Para fluxos complexos, use libs como **rxjs-marbles**:

```ts
it('debounce 300 ms', marbles(m => {
  const entrada  =  m.hot('-a--b----|');
  const esperada = m.cold('---a--b--|');
  m.expect( service.debounce(entrada) ).toBeObservable(esperada);
}));
```

---

<a name="debug"></a>

## 14. Debugando

1. Rodar sem headless: `ng test --browsers=Chrome`.
2. Clique **DEBUG** → DevTools → aba *Sources*.
3. **Breakpoints** ou `debugger;` no código.
4. Para focar num teste: `fit()` / `fdescribe()` ou flag `--include`.

---

<a name="cobertura"></a>

## 15. Cobertura de código

```bash
ng test --code-coverage
open coverage/index.html
```

*Meta recomendada*: ≥ 80 % no serviço/componente.

---

<a name="erros"></a>

## 16. Erros comuns & soluções

| Erro                                      | Possível causa & correção                                                           |
| ----------------------------------------- | ----------------------------------------------------------------------------------- |
| *No specs found*                          | Arquivo fora de `src/` ou nome não termina em `.spec.ts`; ver `tsconfig.spec.json`. |
| *NullInjectorError: No provider for X*    | Esqueceu de mockar ou importar módulo (p. ex. `HttpClientTestingModule`).           |
| *createSpyObj requires a non‑empty array* | Passou array vazio → use pelo menos um método ou objeto literal.                    |
| Test travado em `async`                   | Esqueceu `tick()` ou `done()`.                                                      |

---

<a name="boaspraticas"></a>

## 17. Boas práticas & checklist

* ✅ Nome claro para `describe` e `it` (documentação viva).
* ✅ AAA visível em todos os testes.
* ✅ Cada teste → **um** comportamento.
* ✅ Spies apenas no necessário; evite mocks excessivamente complexos.
* ✅ Use marbles para Observables com tempo.
* ✅ Rodar cobertura no CI.
* ✅ Revise testes ao refatorar código (e vice‑versa).
