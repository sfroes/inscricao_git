import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';
import { TreeNode } from 'primeng/api';
import { ToolsService } from '../../../../../core/tools/tools.service';
import { HierarquiaModel } from '../../model/herarquia.model';
import { Ofertas, SelecaoOfertaModel } from '../../model/selecao-oferta.model';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaHierarquiaService } from './selecao-oferta-hierarquia.service';

// fdescribe para focar a execução nesta suíte de testes durante o desenvolvimento
fdescribe('SelecaoOfertaHierarquiaService - Teste de Integração', () => {
  let service: SelecaoOfertaHierarquiaService;
  let selecaoOfertaDataService: SelecaoOfertaDataService;
  let sanitizer: DomSanitizer;
  let toolsService: ToolsService;

  // Mocks principais usados nos testes
  const mockHierarquia: HierarquiaModel[] = [
    { seq: 1, descricao: 'Raiz', seqPai: 0, isLeaf: false },
    {
      seq: 2,
      descricao: 'Filho 1',
      seqPai: 1,
      isLeaf: true,
      descricaoComplementar: 'Detalhes Filho 1',
    },
    { seq: 3, descricao: 'Filho 2', seqPai: 1, isLeaf: true },
  ];

  // Correção do Mock: Usamos 'as unknown as SelecaoOfertaModel' para dizer ao TypeScript
  // que estamos cientes da tipagem parcial, uma prática comum em testes.
  const mockSelecaoOfertaModel = {
    ofertas: [],
    numeroMaximoOfertaPorInscricao: 5,
    exibirArvoreFechada: false,
    secoes: [
      { token: 'INSTRUCOES_SELECAO_OFERTA', texto: '<span>Instruções</span>' },
    ],
    possuiBoletoPago: false,
    tipoCobrancaPorQuantidadeOferta: false,
    taxas: [],
  } as unknown as SelecaoOfertaModel;

  beforeEach(() => {
    // Mock do serviço de dados com spies do Jasmine
    const selecaoOfertaDataServiceMock = {
      $selecaoOferta: signal(mockSelecaoOfertaModel),
      $ofertasSelecionadas: signal([]),
      setOfertasSelecionadas: jasmine.createSpy('setOfertasSelecionadas'),
      addOfertaSelecioanda: jasmine.createSpy('addOfertaSelecioanda'),
      setAtivarBotaoSelecionaModalHierarquia: jasmine.createSpy(
        'setAtivarBotaoSelecionaModalHierarquia',
      ),
    };

    const toolsServiceMock = {
      decodeHtml: (text: string) => text,
    };

    const sanitizerMock = {
      bypassSecurityTrustHtml: (value: string) => value,
    };

    // Configuração do TestBed
    TestBed.configureTestingModule({
      providers: [
        SelecaoOfertaHierarquiaService,
        {
          provide: SelecaoOfertaDataService,
          useValue: selecaoOfertaDataServiceMock,
        },
        { provide: ToolsService, useValue: toolsServiceMock },
        { provide: DomSanitizer, useValue: sanitizerMock },
      ],
    });

    // Injeção dos serviços
    service = TestBed.inject(SelecaoOfertaHierarquiaService);
    selecaoOfertaDataService = TestBed.inject(SelecaoOfertaDataService);
    toolsService = TestBed.inject(ToolsService);
    sanitizer = TestBed.inject(DomSanitizer);
  });

  it('deve ser criado com sucesso', () => {
    // Assert
    expect(service).toBeTruthy();
  });

  describe('Funcionalidade de Montagem da Árvore', () => {
    it('deve montar a árvore de visualização (TreeNode) a partir dos dados da hierarquia', () => {
      // Arrange
      // O mock mockHierarquia já está disponível.

      // Act
      const tree = service.montarTreeView(mockHierarquia);

      // Assert
      expect(tree.length).toBe(1);
      expect(tree[0].label).toBe('Raiz');
      expect(tree[0].children?.length).toBe(2);
      expect(tree[0].children?.[0].label).toBe('Filho 1');
      expect(tree[0].children?.[0].type).toBe('folha');
      expect(service.$isLoading()).toBeFalse();
    });

    it('deve marcar os nós corretos na árvore baseado nas ofertas selecionadas', () => {
      // Arrange
      const ofertas: Ofertas[] = [
        { seqOferta: { seq: 2 } },
        { seqOferta: { seq: 3 } },
      ] as Ofertas[];
      (selecaoOfertaDataService.$selecaoOferta as any).update((v: any) => ({
        ...v,
        ofertas,
      }));
      const tree = service.montarTreeView(mockHierarquia);

      // Act
      service.marcarArvoreSelecionada(tree);

      // Assert
      expect(Array.isArray(service.ofertasSelecionados)).toBeTrue();
      expect(service.ofertasSelecionados.length).toBe(2);
      expect(service.ofertasSelecionados[0].key).toBe('2');
      expect(service.ofertasSelecionados[1].key).toBe('3');
    });
  });

  describe('Configuração do Tipo de Seleção da Árvore', () => {
    it('deve definir o tipo de seleção como "multiple" se o máximo de ofertas for > 1', () => {
      // Arrange
      // O valor padrão de numeroMaximoOfertaPorInscricao é 5, conforme mock.

      // Act
      service.tipoSelecaoArvore();

      // Assert
      expect(service.tipoSelecaoMultiplaSimples).toBe('multiple');
      expect(service.classeTree).toBe('smc-gpi-selecao-multipla');
    });

    it('deve definir o tipo de seleção como "single" se o máximo de ofertas for 1', () => {
      // Arrange
      (selecaoOfertaDataService.$selecaoOferta as any).update((v: any) => ({
        ...v,
        numeroMaximoOfertaPorInscricao: 1,
      }));

      // Act
      service.tipoSelecaoArvore();

      // Assert
      expect(service.tipoSelecaoMultiplaSimples).toBe('single');
      expect(service.classeTree).toBe('smc-gpi-selecao-simples');
    });
  });

  describe('Interação e Exibição de Informações', () => {
    it('deve montar as instruções do modal corretamente', () => {
      // Arrange
      // O mock já contém as seções necessárias.

      // Act
      service.montarIstrucoesModal();

      // Assert
      expect(service.selecaoOfertaModalInstrucoes).toBe(
        '<span>Instruções</span>',
      );
    });

    it('deve abrir o modal de "mais informações" com os dados corretos do nó', () => {
      // Arrange
      const mockNode: TreeNode = {
        key: '2',
        label: 'Filho 1',
        data: 'Detalhes Filho 1',
      };
      const mockEvent = {
        stopPropagation: jasmine.createSpy('stopPropagation'),
      };

      // Act
      service.abrirModal(mockEvent, mockNode);

      // Assert
      expect(mockEvent.stopPropagation).toHaveBeenCalledTimes(1);
      expect(service.habilitarModalMaisInformacoes).toBeTrue();
      expect(service.$tituloMaisInformacoes()).toBe('Filho 1');
      expect(service.$descricaoModalMaisInformacoes()).toBe('Detalhes Filho 1');
    });
  });

  describe('Atualização de Ofertas Selecionadas', () => {
    it('deve atualizar o data service para seleção múltipla', () => {
      // Arrange
      service.ofertasSelecionados = [{ key: '2' }, { key: '3' }];

      // Act
      service.atualizarOfertasSeleciondas(null);

      // Assert
      expect(
        selecaoOfertaDataService.setOfertasSelecionadas,
      ).toHaveBeenCalledWith([]);
      expect(
        selecaoOfertaDataService.addOfertaSelecioanda,
      ).toHaveBeenCalledTimes(2);

      const primeiraChamada = (
        selecaoOfertaDataService.addOfertaSelecioanda as jasmine.Spy
      ).calls.argsFor(0)[0];
      expect(primeiraChamada.seqOferta.seq).toBe(2);

      const segundaChamada = (
        selecaoOfertaDataService.addOfertaSelecioanda as jasmine.Spy
      ).calls.argsFor(1)[0];
      expect(segundaChamada.seqOferta.seq).toBe(3);
    });

    it('deve atualizar o data service para seleção única', () => {
      // Arrange
      service.ofertasSelecionados = { key: '2' };

      // Act
      service.atualizarOfertasSeleciondas(null);

      // Assert
      expect(
        selecaoOfertaDataService.setOfertasSelecionadas,
      ).toHaveBeenCalledWith([]);
      expect(
        selecaoOfertaDataService.addOfertaSelecioanda,
      ).toHaveBeenCalledTimes(1);

      const primeiraChamada = (
        selecaoOfertaDataService.addOfertaSelecioanda as jasmine.Spy
      ).calls.argsFor(0)[0];
      expect(primeiraChamada.seqOferta.seq).toBe(2);
    });
  });
});
