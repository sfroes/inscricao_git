import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';
import { LayoutProcessoDataService } from '../../../../../core/layout-processo/service/layout-processo-data.service';
import { ToolsService } from '../../../../../core/tools/tools.service';
import { TipoCobranca } from '../../model/enums/tipo-cobranca.enum';
import { ListaOfertaModel } from '../../model/lista-oferta.model';
import {
  Ofertas,
  SelecaoOfertaModel,
  Taxa,
} from '../../model/selecao-oferta.model';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../../service/selecao-oferta.service';
import { SelecaoOfertaComTaxaService } from './selecao-oferta-com-taxa.service';

// fdescribe para focar a execução nesta suíte de testes
fdescribe('SelecaoOfertaComTaxaService - Teste de Integração', () => {
  let service: SelecaoOfertaComTaxaService;
  let selecaoOfertaDataService: SelecaoOfertaDataService;
  let selecaoOfertaService: SelecaoOfertaService;

  // Mocks com a estrutura de dados completa e correta
  const mockOfertas: Ofertas[] = [
    {
      seq: 1,
      seqOferta: { descricao: '', seq: 101 },
      numeroOpcao: 1,
      ofertaImpedida: false,
      exibirMensagemOferta: false,
      mensagemOferta: '',
      justificativaInscricao: '',
      ativo: true,
    },
    {
      seq: 2,
      seqOferta: { descricao: '', seq: 102 },
      numeroOpcao: 2,
      ofertaImpedida: true,
      exibirMensagemOferta: false,
      mensagemOferta: '',
      justificativaInscricao: '',
      ativo: true,
    },
  ];

  const mockListaOferta: ListaOfertaModel[] = [
    {
      oferta: mockOfertas[0],
      hierquia: { seq: 10, descricao: 'Oferta 1', seqPai: 0, isLeaf: true },
    },
    {
      oferta: mockOfertas[1],
      hierquia: { seq: 20, descricao: 'Oferta 2', seqPai: 0, isLeaf: true },
    },
  ];

  const mockSelecaoOfertaModel: SelecaoOfertaModel = {
    ofertas: mockOfertas,
    taxas: [],
    gruposTaxa: [],
    totalGeral: 100,
    secoes: [{ token: 'INSTRUCOES_TAXA', texto: 'Instruções de Taxa' }],
    numeroMaximoConvocacaoPorInscricao: 2,
    possuiBoletoPago: false,
  } as unknown as SelecaoOfertaModel;

  let selecaoOfertaDataServiceMock: Partial<SelecaoOfertaDataService>;
  let selecaoOfertaServiceMock: Partial<SelecaoOfertaService>;

  beforeEach(() => {
    // Arrange: Mocks para os serviços de dependência
    selecaoOfertaDataServiceMock = {
      $selecaoOferta: signal(mockSelecaoOfertaModel),
      $listaOfertas: signal(mockListaOferta),
      $ofertasSelecionadas: signal([]),
      updateTotalGeralSelecaoOferta: jasmine.createSpy(
        'updateTotalGeralSelecaoOferta',
      ),
      updateListaOfertas: jasmine.createSpy('updateListaOfertas'),
      setListaOfertas: jasmine.createSpy('setListaOfertas'),
      setOrdernarLista: jasmine.createSpy('setOrdernarLista'),
      setBotaoCancelarHierarquia: jasmine.createSpy(
        'setBotaoCancelarHierarquia',
      ),
      setOfertasSelecionadas: jasmine.createSpy('setOfertasSelecionadas'),
      updateTaxasPorGrupo: jasmine.createSpy('updateTaxasPorGrupo'),
      updateItemTaxaNaSelecaoOferta: jasmine.createSpy(
        'updateItemTaxaNaSelecaoOferta',
      ),
      botaoSelecionarOfertaAcionado: false,
    };

    selecaoOfertaServiceMock = {
      validarLiberarBotaoProximo: jasmine.createSpy(
        'validarLiberarBotaoProximo',
      ),
      setNumeroMinimoGrupoNaoAtingido: jasmine.createSpy(
        'setNumeroMinimoGrupoNaoAtingido',
      ),
      descricaoGrupoTaxasNaoAtingidos: new Set<string>(),
    };

    TestBed.configureTestingModule({
      providers: [
        SelecaoOfertaComTaxaService,
        {
          provide: SelecaoOfertaDataService,
          useValue: selecaoOfertaDataServiceMock,
        },
        { provide: SelecaoOfertaService, useValue: selecaoOfertaServiceMock },
        {
          provide: ToolsService,
          useValue: { decodeHtml: (text: string) => text },
        },
        {
          provide: DomSanitizer,
          useValue: { bypassSecurityTrustHtml: (value: string) => value },
        },
        { provide: LayoutProcessoDataService, useValue: {} },
      ],
    });

    service = TestBed.inject(SelecaoOfertaComTaxaService);
    selecaoOfertaDataService = TestBed.inject(SelecaoOfertaDataService);
    selecaoOfertaService = TestBed.inject(SelecaoOfertaService);

    // Arrange: Garante que o estado inicial do signal no serviço seja uma cópia profunda do mock
    service.$listaOferta.set(JSON.parse(JSON.stringify(mockListaOferta)));
  });

  it('deve ser criado com sucesso', () => {
    // Assert
    expect(service).toBeTruthy();
  });

  describe('Gerenciamento de Ofertas', () => {
    it('deve reordenar a lista de ofertas após um evento de drag-and-drop', () => {
      // Arrange
      spyOn(service, 'organizarLista').and.callThrough();
      const event = { previousIndex: 0, currentIndex: 1 } as CdkDragDrop<
        ListaOfertaModel[]
      >;
      const listaOriginal = service.$listaOferta();

      // Act
      service.ordernar(event);

      // Assert
      const listaReordenada = service.$listaOferta();
      expect(listaReordenada[0].oferta.numeroOpcao).toBe(
        listaOriginal[1].oferta.numeroOpcao,
      );
      expect(listaReordenada[1].oferta.numeroOpcao).toBe(
        listaOriginal[0].oferta.numeroOpcao,
      );
      expect(service.organizarLista).toHaveBeenCalledTimes(1);
    });

    it('deve apagar uma oferta da lista e atualizar os estados dependentes', () => {
      // Arrange
      const numeroOpcaoParaApagar = 1;
      const totalOfertasAntes = service.$listaOferta().length;

      // Act
      service.apagarOferta(numeroOpcaoParaApagar);

      // Assert
      const listaAtual = service.$listaOferta();
      expect(listaAtual.length).toBe(totalOfertasAntes - 1);
      expect(
        listaAtual.find((o) => o.oferta.numeroOpcao === numeroOpcaoParaApagar),
      ).toBeUndefined();
      expect(selecaoOfertaDataService.updateListaOfertas).toHaveBeenCalled();
      expect(
        selecaoOfertaDataService.setBotaoCancelarHierarquia,
      ).toHaveBeenCalledWith(true);
      expect(
        selecaoOfertaService.validarLiberarBotaoProximo,
      ).toHaveBeenCalledTimes(1);
    });

    it('deve organizar a lista, reatribuindo o numeroOpcao sequencialmente', () => {
      // Arrange
      const listaDesorganizada = [
        { oferta: { numeroOpcao: 3 } },
        { oferta: { numeroOpcao: 1 } },
      ] as ListaOfertaModel[];
      service.$listaOferta.set(listaDesorganizada);

      // Act
      service.organizarLista();

      // Assert
      const listaOrganizada = service.$listaOferta();
      expect(listaOrganizada[0].oferta.numeroOpcao).toBe(1);
      expect(listaOrganizada[1].oferta.numeroOpcao).toBe(2);
      expect(selecaoOfertaDataService.setListaOfertas).toHaveBeenCalledWith(
        listaOrganizada,
      );
      expect(selecaoOfertaDataService.setOrdernarLista).toHaveBeenCalledWith(
        true,
      );
    });

    it('deve atualizar o texto da justificativa de uma oferta específica', () => {
      // Arrange
      const novaJustificativa = 'Nova Justificativa';
      const mockEvent = { target: { value: novaJustificativa } };
      const numeroOpcao = 1;

      // Act
      service.atulizarTextoJustificativa(mockEvent, numeroOpcao);

      // Assert
      const ofertaAtualizada = service
        .$listaOferta()
        .find((o) => o.oferta.numeroOpcao === numeroOpcao);
      expect(ofertaAtualizada?.oferta.justificativaInscricao).toBe(
        novaJustificativa,
      );
      expect(
        selecaoOfertaService.validarLiberarBotaoProximo,
      ).toHaveBeenCalledTimes(1);
    });

    it('deve retornar a classe CSS "oferta-desativada" para uma oferta impedida', () => {
      // Arrange
      const ofertaImpedida = mockListaOferta.find(
        (o) => o.oferta.ofertaImpedida,
      )!;

      // Act
      const classe = service.verificaOfertaAtiva(ofertaImpedida);

      // Assert
      expect(classe).toBe(service.ofertaDesativada);
    });

    it('deve retornar uma string vazia como classe CSS para uma oferta ativa', () => {
      // Arrange
      const ofertaAtiva = mockListaOferta.find(
        (o) => !o.oferta.ofertaImpedida,
      )!;

      // Act
      const classe = service.verificaOfertaAtiva(ofertaAtiva);

      // Assert
      expect(classe).toBe('');
    });
  });

  describe('Gerenciamento de Taxas', () => {
    it('deve montar e definir as instruções de taxas corretamente', () => {
      // Arrange
      spyOn(service, 'setInstrucoes');
      const textoInstrucoes = 'Instruções de Taxa';
      (selecaoOfertaDataServiceMock.$selecaoOferta as any).set({
        ...mockSelecaoOfertaModel,
        secoes: [{ token: 'INSTRUCOES_TAXA', texto: textoInstrucoes }],
      });

      // Act
      service.montarIstrucoesTaxas();

      // Assert
      expect(service.setInstrucoes).toHaveBeenCalledWith(textoInstrucoes);
    });

    it('deve montar e agrupar as taxas por oferta sem erros', () => {
      // Arrange
      // CORREÇÃO: Adicionamos a propriedade 'regras: []' ao mock de cada taxa.
      // Isso evita o erro 'Cannot read properties of undefined (reading 'some')'
      // se o método tentar acessar uma propriedade de array opcional.
      service.taxas = [
        {
          seqOferta: 101,
          tipoCobranca: TipoCobranca.porOferta,
          descricao: 'Taxa A',
          valorItem: 10,
          seqTaxa: 1,
        },
        {
          seqOferta: 102,
          tipoCobranca: TipoCobranca.porOferta,
          descricao: 'Taxa B',
          valorItem: 20,
          seqTaxa: 2,
        },
        {
          seqOferta: 101,
          tipoCobranca: TipoCobranca.porOferta,
          descricao: 'Taxa C',
          valorItem: 30,
          seqTaxa: 3,
        },
      ] as Taxa[]; // O tipo 'Taxa' deve ser ajustado para incluir a propriedade opcional

      // Act
      service.montarTaxasPorOferta();

      // Assert
      const taxasMontadas = service.$taxasPorOferta();
      const grupoOferta101 = taxasMontadas.find((t) => t.seqOferta === 101);
      const grupoOferta102 = taxasMontadas.find((t) => t.seqOferta === 102);

      expect(taxasMontadas.length).toBe(2); // Deve criar 2 grupos (para as ofertas 101 e 102)

      expect(grupoOferta101).toBeDefined();
      expect(grupoOferta101?.taxasOferta.length).toBe(2); // Grupo 101 deve ter 2 taxas
      expect(grupoOferta101?.taxasOferta.map((t) => t.seqTaxa)).toEqual([1, 3]); // Verifica as taxas corretas

      expect(grupoOferta102).toBeDefined();
      expect(grupoOferta102?.taxasOferta.length).toBe(1); // Grupo 102 deve ter 1 taxa
      expect(grupoOferta102?.taxasOferta[0].seqTaxa).toBe(2); // Verifica a taxa correta
    });

    it('deve montar taxas por quantidade de ofertas selecionadas', () => {
      // Arrange
      service.taxas = [
        {
          seqTaxa: 1,
          tipoCobranca: TipoCobranca.porQuantidadeOfertas,
          valorItem: 50,
        },
      ] as Taxa[];
      (selecaoOfertaDataServiceMock.$listaOfertas as any).set([{}, {}]); // Simula 2 ofertas

      // Act
      service.montarTaxasPorQuantidadeOferta();

      // Assert
      const taxasMontadas = service.$taxasPorQuantidadeOferta();
      expect(taxasMontadas.taxasOferta.length).toBe(1);
      expect(taxasMontadas.taxasOferta[0].numeroItens).toBe(2);
      expect(taxasMontadas.taxasOferta[0].valorTotalTaxa).toBe(100);
    });
  });
});
