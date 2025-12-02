import { inject, Injectable, signal } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { TreeNode } from 'primeng/api';
import { ToolsService } from '../../../../../core/tools/tools.service';
import { HierarquiaModel } from '../../model/herarquia.model';
import { Ofertas } from '../../model/selecao-oferta.model';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';

@Injectable({
  providedIn: 'root',
})
export class SelecaoOfertaHierarquiaService {
  //#region variaveis
  files!: TreeNode[];
  habilitarModalMaisInformacoes: boolean = false;
  ofertasSelecionados: any;
  selecaoOfertaModalInstrucoes: any;
  classeTree: string = '';
  tipoSelecaoMultiplaSimples:
    | 'single'
    | 'multiple'
    | 'checkbox'
    | null
    | undefined = 'multiple';
  numeroMaximoOferta: number | null = null;
  tituloAtividade: string = '';
  //#endregion

  //#region signals
  $descricaoModalMaisInformacoes = signal('');
  $tituloMaisInformacoes = signal('');
  $isLoading = signal(true);
  //#endregion

  //#region injeção de dependencias
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  toolsService = inject(ToolsService);
  sanitizer = inject(DomSanitizer);
  //#endregion

  constructor() {}

  validarInputComponente(data: HierarquiaModel[]) {
    if (data && data.length > 0) {
      this.files = this.montarTreeView(data);
      this.marcarArvoreSelecionada(this.files);
      this.montarIstrucoesModal();
      if (this.selecaoOfertaDataService.$selecaoOferta().ofertas.length > 0) {
        this.atualizarOfertasSeleciondas('');
      }
    } else {
      this.files = [];
    }
  }

  //#region  montagem da arvore
  /**
   * Monta uma estrutura de árvore (TreeView) a partir de um array de dados hierárquicos.
   *
   * @param data - Array de objetos do tipo `HierarquiaModel` que representam os dados hierárquicos.
   * @returns Um array de objetos do tipo `TreeNode` que representam a estrutura de árvore.
   *
   * O método realiza as seguintes operações:
   * - Mapeia os nós pelo `seq` para facilitar a busca dos pais.
   * - Cria a estrutura de saída `rootNodes` que contém os nós raiz.
   * - Itera sobre os dados para montar os nós e adicioná-los ao mapa e à estrutura de saída.
   * - Verifica se o nó é raiz ou filho e adiciona-o ao respectivo pai, se aplicável.
   * - Ordena os nós recursivamente com base no rótulo (`label`) em ordem alfabética.
   * - Chama métodos auxiliares `tipoSelecaoArvore` e `isLoading.set(false)` após a montagem da árvore.
   */
  montarTreeView(data: HierarquiaModel[]): TreeNode[] {
    // Mapear nós pelo seq para facilitar a busca dos pais
    const nodeMap = new Map<number, TreeNode>();

    // Estrutura de saída
    const rootNodes: TreeNode[] = [];

    let numeroFolhas = 0;
    // Iterar sobre os dados e montar os nós
    data.forEach((item) => {
      const node: TreeNode = {
        key: item.seq.toString(),
        label: item.descricao,
        data: item.descricaoComplementar || null,
        //icon: item.isLeaf ? 'pi pi-fw pi-file' : 'pi pi-fw pi-folder',
        selectable: item.isLeaf ? true : false,
        type: item.isLeaf ? 'folha' : 'none',
        children: item.isLeaf ? undefined : [],
        expanded: this.selecaoOfertaDataService.$selecaoOferta()
          .exibirArvoreFechada
          ? false
          : true,
      };

      if (item.isLeaf) {
        numeroFolhas++;
      }

      // Adicionar nó ao mapa
      nodeMap.set(item.seq, node);

      // Verificar se o nó é raiz ou filho
      if (item.seqPai === 0) {
        rootNodes.push(node); // Nó raiz
      } else {
        // Adicionar como filho do pai
        const parent = nodeMap.get(item.seqPai);
        if (parent && parent.children) {
          parent.children.push(node);
        }
      }
    });

    // Função recursiva para ordenar os nós
    function ordenarNodes(nodes: TreeNode[]): void {
      nodes.sort((a, b) => {
        if (a.label && b.label) {
          return a.label.localeCompare(b.label, 'pt-BR');
        } else {
          return 0; // ou algum outro valor padrão
        }
      });

      nodes.forEach((node) => {
        if (node.children) {
          ordenarNodes(node.children);
        }
      });
    }

    // Ordenar a hierarquia inteira
    ordenarNodes(rootNodes);
    // Validar se o node é folha e desabilita ele
    if (numeroFolhas == 1) {
      const desabilitarFolhas = (nodes: TreeNode[]) => {
        nodes.forEach((node) => {
          if (node.type === 'folha') {
            node.selectable = false;
          }
          if (node.children) {
            desabilitarFolhas(node.children);
          }
        });
      };

      //desabilitarFolhas(rootNodes);
    }

    this.tipoSelecaoArvore();
    this.$isLoading.set(false);
    return rootNodes;
  }

  /**
   * Define o tipo de seleção da árvore e a classe CSS correspondente com base no número máximo de ofertas permitidas.
   *
   * - Se `numeroMaximoOferta` for `null` ou maior que 1, o tipo de seleção será 'multiple' e a classe CSS será 'smc-gpi-selecao-multipla'.
   * - Caso contrário, o tipo de seleção será 'single' e a classe CSS será 'smc-gpi-selecao-simples'.
   *
   * @returns {void}
   */
  tipoSelecaoArvore() {
    this.numeroMaximoOferta =
      this.selecaoOfertaDataService.$selecaoOferta().numeroMaximoOfertaPorInscricao;

    if (
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago &&
      this.selecaoOfertaDataService.$selecaoOferta()
        .tipoCobrancaPorQuantidadeOferta
    ) {
      this.numeroMaximoOferta =
        this.selecaoOfertaDataService.$selecaoOferta().ofertas.length;
    }
    this.tipoSelecaoMultiplaSimples =
      this.numeroMaximoOferta == null || this.numeroMaximoOferta > 1
        ? 'multiple'
        : 'single';

    this.classeTree =
      this.numeroMaximoOferta == null || this.numeroMaximoOferta > 1
        ? 'smc-gpi-selecao-multipla'
        : 'smc-gpi-selecao-simples';
  }

  /**
   * Marca os nós da árvore selecionada com base nas ofertas selecionadas.
   *
   * @param nos - Array de nós da árvore a serem marcados.
   *
   * O método percorre os nós da árvore e verifica se o identificador do nó (key) está presente na lista de ofertas selecionadas.
   * Se estiver, o nó é marcado como selecionado. A seleção pode ser simples (single) ou múltipla.
   *
   * - Se a seleção for simples, apenas um nó é marcado.
   * - Se a seleção for múltipla, todos os nós correspondentes são marcados.
   *
   * O método utiliza uma função recursiva `marcarNos` para percorrer todos os nós e seus filhos.
   */
  marcarArvoreSelecionada(nos: TreeNode[]) {
    const ofertas = this.selecaoOfertaDataService.$selecaoOferta().ofertas;
    if (ofertas.length > 0) {
      const seqsOfertasSelecionadas = ofertas.map((m) => m.seqOferta.seq);

      this.ofertasSelecionados = undefined;

      this.ofertasImpedidas();

      const marcarNos = (nodes: TreeNode[]) => {
        nodes.forEach((node) => {
          if (seqsOfertasSelecionadas.includes(+node.key!)) {
            if (this.tipoSelecaoMultiplaSimples == 'single') {
              this.ofertasSelecionados = node;
            } else {
              if (this.ofertasSelecionados == undefined) {
                this.ofertasSelecionados = [];
                this.ofertasSelecionados.push(node);
              } else {
                this.ofertasSelecionados.push(node);
              }
            }
          }
          if (node.children) {
            marcarNos(node.children);
          }
        });
      };

      marcarNos(nos);
    }
    if (this.ofertasSelecionados == undefined) {
      this.ofertasSelecionados =
        this.tipoSelecaoMultiplaSimples == 'multiple' ? [] : {};
    }
  }

  ofertasImpedidas() {
    const ofertas = this.selecaoOfertaDataService.$selecaoOferta().ofertas;
    if (ofertas.length > 0) {
      this.ofertasSelecionados = [];
      ofertas.forEach((oferta) => {
        if (
          oferta.ofertaImpedida &&
          !this.ofertasSelecionados.find(
            (f: any) => f.key == oferta.seqOferta.seq,
          )
        ) {
          const ofertaImpedita = {
            children: undefined,
            data: '',
            expand: false,
            key: oferta.seqOferta.seq,
            label: '',
            selectable: false,
            type: 'folha',
          };
          this.ofertasSelecionados.push(ofertaImpedita);
        }
      });
    }
  }

  //#endregion montage da arvore

  //#region modal de mais informações
  /**
   * Monta as instruções do modal de seleção de oferta.
   *
   * Este método decodifica o HTML das instruções de seleção de oferta,
   * utilizando o serviço `toolsService` para decodificação e o serviço
   * `sanitizer` para garantir a segurança do HTML decodificado.
   *
   * @remarks
   * O método busca a seção de instruções de seleção de oferta no objeto
   * `selecaoOferta` do serviço `selecaoOfertaDataService`, decodifica o HTML
   * encontrado e o atribui à propriedade `selecaoOfertaModalInstrucoes` após
   * sanitização.
   *
   * @returns {void}
   */
  montarIstrucoesModal() {
    // Decodifica o html das instrucoes
    const instrucao = this.selecaoOfertaDataService
      .$selecaoOferta()
      .secoes.find((f: any) => f.token == 'INSTRUCOES_SELECAO_OFERTA')?.texto;
    if (instrucao) {
      const instrucoesDecode = this.toolsService.decodeHtml(instrucao);

      // Cria um elemento temporário para extrair o texto
      const tempElement = document.createElement('div');
      tempElement.innerHTML = instrucoesDecode;

      // Obtém o texto e remove espaços em branco extras
      const textoConteudo =
        tempElement.textContent || tempElement.innerText || '';
      const textoLimpo = textoConteudo.trim();

      if (textoLimpo.length > 0) {
        this.selecaoOfertaModalInstrucoes =
          this.sanitizer.bypassSecurityTrustHtml(instrucoesDecode);
      }
    }
  }

  /**
   * Abre um modal com mais informações sobre o nó selecionado na árvore.
   *
   * @param event - O evento que disparou a abertura do modal.
   * @param node - O nó da árvore que contém as informações a serem exibidas no modal.
   *
   * Este método define o título do modal com o rótulo do nó e habilita a exibição do modal.
   * Além disso, define a descrição do modal com os dados do nó.
   */
  abrirModal(event: any, node: TreeNode) {
    event.stopPropagation();
    this.$tituloMaisInformacoes.set(node.label!);
    this.habilitarModalMaisInformacoes = true;
    this.$descricaoModalMaisInformacoes.set(node.data);
  }
  //#endregion modal de mais informações

  atualizarOfertasSeleciondas(event: any) {
    // Inicializa a lista de ofertas selecionadas como um array vazio
    this.selecaoOfertaDataService.setOfertasSelecionadas([]);

    // Verifica se 'ofertasSelecionados' é um array
    if (Array.isArray(this.ofertasSelecionados)) {
      if (
        this.numeroMaximoOferta != null &&
        this.ofertasSelecionados.length > this.numeroMaximoOferta
      ) {
        this.ofertasSelecionados.pop()!;
      }

      let ultimaOpcao = this.selecaoOfertaDataService
        .$selecaoOferta()
        .ofertas.reduce(
          (maior, oferta) => Math.max(maior, oferta.numeroOpcao),
          0, // valor inicial, assume que começa com 0
        );

      let proximaOpcao = ultimaOpcao + 1;

      // Itera sobre cada item do array 'ofertasSelecionados'
      this.ofertasSelecionados.forEach((item: any) => {
        // Cria um objeto 'oferta' do tipo 'Ofertas'
        //Valida se ela ja esta selecionada na oferta com seus dados anteriores
        const ofertaJaPreenchida = this.selecaoOfertaDataService
          .$selecaoOferta()
          .ofertas.find((f) => f.seqOferta.seq == +item.key);
        let oferta: Ofertas = {} as Ofertas;
        if (ofertaJaPreenchida != null) {
          oferta.seqOferta = { seq: ofertaJaPreenchida.seqOferta.seq };
          oferta.exibirMensagemOferta = ofertaJaPreenchida.exibirMensagemOferta;
          oferta.justificativaInscricao =
            ofertaJaPreenchida.justificativaInscricao;
          oferta.mensagemOferta = ofertaJaPreenchida.mensagemOferta;
          oferta.numeroOpcao = ofertaJaPreenchida.numeroOpcao;
          oferta.seq = ofertaJaPreenchida.seq;
          oferta.ativo = ofertaJaPreenchida.ativo;
          oferta.ofertaImpedida = ofertaJaPreenchida.ofertaImpedida;
        } else {
          oferta.seqOferta = { seq: +item.key! }; // Converte a chave para número e atribui a 'seqOferta'
          oferta.exibirMensagemOferta = false; // Inicializa 'exibirMensagemOferta' como falso
          oferta.justificativaInscricao = ''; // Inicializa 'justificativaInscricao' como string vazia
          oferta.mensagemOferta = ''; // Inicializa 'mensagemOferta' como string vazia
          oferta.numeroOpcao = proximaOpcao; // Inicializa 'numeroOpcao' como 0
          oferta.seq = 0; // Inicializa 'seq' como 0
          oferta.ativo = true;
          oferta.ofertaImpedida = false;
          proximaOpcao++;
        }

        // Adiciona o objeto 'oferta' ao array 'ofertasSelecionadas'
        this.selecaoOfertaDataService.addOfertaSelecioanda(oferta);
      });
    } else if (this.ofertasSelecionados != null) {
      // Se 'ofertasSelecionados' não é um array e não é nulo
      // Cria um objeto 'oferta' do tipo 'Ofertas' para um item único
      const oferta: Ofertas = {
        seqOferta: { seq: +(this.ofertasSelecionados as any).key! }, // Converte a chave para número e atribui a 'seqOferta'
        exibirMensagemOferta: false, // Inicializa 'exibirMensagemOferta' como falso
        justificativaInscricao: '', // Inicializa 'justificativaInscricao' como string vazia
        mensagemOferta: '', // Inicializa 'mensagemOferta' como string vazia
        numeroOpcao: 0, // Inicializa 'numeroOpcao' como 0
        seq: 0, // Inicializa 'seq' como 0
        ativo: true,
        ofertaImpedida: false,
      };
      // Adiciona o objeto 'oferta' ao array 'ofertasSelecionadas'
      this.selecaoOfertaDataService.addOfertaSelecioanda(oferta);
    }
    if (event) {
      this.habilitarBotaoSelecionar();
    }
  }
  habilitarBotaoSelecionar() {
    this.selecaoOfertaDataService.setAtivarBotaoSelecionaModalHierarquia(false);
    if (
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago &&
      this.ofertasSelecionados.length == 0
    ) {
      this.selecaoOfertaDataService.setAtivarBotaoSelecionaModalHierarquia(
        true,
      );
    }

    if (
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago &&
      this.selecaoOfertaDataService.$selecaoOferta()
        .tipoCobrancaPorQuantidadeOferta
    ) {
      const seqOfertasSelecionadas = this.ofertasSelecionados.map(
        (m: any) => +m.key,
      );
      const seqOfertasInDB = this.selecaoOfertaDataService
        .$selecaoOferta()
        .ofertas.map((m) => m.seqOferta.seq);
      if (
        this.validarArrayIguais(seqOfertasInDB, seqOfertasSelecionadas) ||
        this.selecaoOfertaDataService.$selecaoOferta().ofertas.length !=
          this.ofertasSelecionados.length
      ) {
        this.selecaoOfertaDataService.setAtivarBotaoSelecionaModalHierarquia(
          true,
        );
      }
    }
  }

  validarArrayIguais<T>(arrayA: T[], arrayB: T[]) {
    if (arrayA.length !== arrayB.length) {
      return false;
    }

    arrayA = arrayA.sort((a, b) => +a - +b);
    arrayB = arrayB.sort((a, b) => +a - +b);

    return JSON.stringify(arrayA) == JSON.stringify(arrayB);
  }
}
