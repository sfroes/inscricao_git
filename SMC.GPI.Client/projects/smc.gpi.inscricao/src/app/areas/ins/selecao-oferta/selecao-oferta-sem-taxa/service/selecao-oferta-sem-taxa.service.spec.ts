import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { signal } from '@angular/core';
import { fakeAsync, TestBed, tick } from '@angular/core/testing';

import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../../service/selecao-oferta.service';
import { LayoutProcessoDataService } from './../../../../../core/layout-processo/service/layout-processo-data.service';
import { SelecaoOfertaSemTaxaService } from './selecao-oferta-sem-taxa.service';

import { ListaOfertaModel } from '../../model/lista-oferta.model';
import { Ofertas, SelecaoOfertaModel } from '../../model/selecao-oferta.model';

// Mock para as dependências do serviço
const selecaoOfertaDataServiceMock = {
  // ... outros mocks
  $selecaoOferta: signal<SelecaoOfertaModel>({
    ofertas: [],
    taxas: [],
    numeroMaximoConvocacaoPorInscricao: 3,
  } as any), // <-- CORREÇÃO APLICADA AQUI,
  $ofertasSelecionadas: signal<Ofertas[]>([]),
  setOfertasSelecionadas: jasmine.createSpy('setOfertasSelecionadas'),
  setListaOfertas: jasmine.createSpy('setListaOfertas'),
  setOrdernarLista: jasmine.createSpy('setOrdernarLista'),
};

const selecaoOfertaServiceMock = {
  validarLiberarBotaoProximo: jasmine.createSpy('validarLiberarBotaoProximo'),
};

const layoutProcessoDataServiceMock = {
  // Adicione mocks para os métodos/propriedades usados, se houver
};

fdescribe('SelecaoOfertaSemTaxaService - Teste de Integração', () => {
  let service: SelecaoOfertaSemTaxaService;
  let dataService: SelecaoOfertaDataService;
  let ofertaService: SelecaoOfertaService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SelecaoOfertaSemTaxaService,
        {
          provide: SelecaoOfertaDataService,
          useValue: selecaoOfertaDataServiceMock,
        },
        { provide: SelecaoOfertaService, useValue: selecaoOfertaServiceMock },
        {
          provide: LayoutProcessoDataService,
          useValue: layoutProcessoDataServiceMock,
        },
      ],
    });

    service = TestBed.inject(SelecaoOfertaSemTaxaService);
    dataService = TestBed.inject(SelecaoOfertaDataService);
    ofertaService = TestBed.inject(SelecaoOfertaService);

    // Reset spies and mocks before each test
    selecaoOfertaDataServiceMock.setOfertasSelecionadas.calls.reset();
    selecaoOfertaDataServiceMock.setListaOfertas.calls.reset();
    selecaoOfertaDataServiceMock.setOrdernarLista.calls.reset();
    selecaoOfertaServiceMock.validarLiberarBotaoProximo.calls.reset();
  });

  it('deve ser criado com sucesso', () => {
    // Assert
    expect(service).toBeTruthy();
  });

  describe('Funcionalidade: Ordenação de Ofertas', () => {
    it('deve reordenar a lista de ofertas e chamar a organização', () => {
      // Arrange
      const listaInicial: ListaOfertaModel[] = [
        // Adicionamos um identificador único (seq) para cada oferta
        {
          oferta: { seqOferta: { seq: 101 }, numeroOpcao: 1 } as Ofertas,
          hierquia: null,
        },
        {
          oferta: { seqOferta: { seq: 102 }, numeroOpcao: 2 } as Ofertas,
          hierquia: null,
        },
        {
          oferta: { seqOferta: { seq: 103 }, numeroOpcao: 3 } as Ofertas,
          hierquia: null,
        },
      ];
      service.$listaOferta.set(listaInicial);
      const event = {
        previousIndex: 0,
        currentIndex: 2,
      } as CdkDragDrop<ListaOfertaModel[]>;
      spyOn(service, 'organizarLista').and.callThrough();

      // Act
      service.ordernar(event);

      // Assert
      const listaFinal = service.$listaOferta();

      // A lista foi movida de [101, 102, 103] para [102, 103, 101]
      // E então o `organizarLista` re-numerou o `numeroOpcao` para [1, 2, 3]

      // Verifica se o objeto correto está na posição e se seu `numeroOpcao` foi atualizado
      expect(listaFinal[0].oferta.seqOferta.seq).toBe(102); // O item 102 é o primeiro
      expect(listaFinal[0].oferta.numeroOpcao).toBe(1); // E seu numeroOpcao agora é 1

      expect(listaFinal[2].oferta.seqOferta.seq).toBe(101); // O item 101 é o último
      expect(listaFinal[2].oferta.numeroOpcao).toBe(3); // E seu numeroOpcao agora é 3

      expect(service.organizarLista).toHaveBeenCalledTimes(1);
    });
  });

  describe('Funcionalidade: Exclusão de Oferta', () => {
    it('deve apagar uma oferta da lista, atualizar serviços e reordenar', () => {
      // Arrange
      const ofertaParaManter: Ofertas = {
        numeroOpcao: 1,
        seqOferta: { seq: 10 },
      } as Ofertas;
      const ofertaParaExcluir: Ofertas = {
        numeroOpcao: 2,
        seqOferta: { seq: 20 },
      } as Ofertas;

      const listaInicial: ListaOfertaModel[] = [
        { oferta: ofertaParaManter, hierquia: null },
        { oferta: ofertaParaExcluir, hierquia: null },
      ];
      service.$listaOferta.set(listaInicial);

      selecaoOfertaDataServiceMock.$selecaoOferta.set({
        ofertas: [ofertaParaManter, ofertaParaExcluir],
        taxas: [{ seqOferta: 10 }, { seqOferta: 20 }],
      } as any);

      selecaoOfertaDataServiceMock.$ofertasSelecionadas.set([
        ofertaParaManter,
        ofertaParaExcluir,
      ]);
      spyOn(service, 'organizarLista').and.callThrough();

      // Act
      service.apagarOferta(2); // Apaga a oferta com numeroOpcao 2

      // Assert
      expect(service.$listaOferta().length).toBe(1);
      expect(service.$listaOferta()[0].oferta.numeroOpcao).toBe(1);

      expect(dataService.setOfertasSelecionadas).toHaveBeenCalledWith(
        jasmine.arrayWithExactContents([ofertaParaManter]),
      );
      expect(dataService.$selecaoOferta().ofertas.length).toBe(1);
      expect(dataService.$selecaoOferta().taxas.length).toBe(1);
      expect(dataService.$selecaoOferta().taxas[0].seqOferta).toBe(10);
      expect(dataService.setListaOfertas).toHaveBeenCalledWith(
        service.$listaOferta(),
      );
      expect(service.organizarLista).toHaveBeenCalledTimes(1);
      expect(ofertaService.validarLiberarBotaoProximo).toHaveBeenCalledTimes(1);
    });
  });

  describe('Funcionalidade: Organização de Lista', () => {
    it('deve redefinir o numeroOpcao corretamente e notificar o dataService', () => {
      // Arrange
      const lista: ListaOfertaModel[] = [
        { oferta: { numeroOpcao: 3 } as Ofertas, hierquia: null },
        { oferta: { numeroOpcao: 1 } as Ofertas, hierquia: null },
      ];
      service.$listaOferta.set(lista);

      // Act
      service.organizarLista();

      // Assert
      const listaOrganizada = service.$listaOferta();
      expect(listaOrganizada[0].oferta.numeroOpcao).toBe(1);
      expect(listaOrganizada[1].oferta.numeroOpcao).toBe(2);
      expect(dataService.setOrdernarLista).toHaveBeenCalledWith(true);
    });
  });

  describe('Funcionalidade: Atualização de Justificativa', () => {
    it('deve atualizar o texto da justificativa da oferta correta', () => {
      // Arrange
      const lista: ListaOfertaModel[] = [
        {
          oferta: { numeroOpcao: 1, justificativaInscricao: '' } as Ofertas,
          hierquia: null,
        },
        {
          oferta: { numeroOpcao: 2, justificativaInscricao: '' } as Ofertas,
          hierquia: null,
        },
      ];
      service.$listaOferta.set(lista);
      const evento = { target: { value: 'Nova justificativa' } };
      const numeroOpcaoAlvo = 2;

      // Act
      service.atulizarTextoJustificativa(evento, numeroOpcaoAlvo);

      // Assert
      const ofertaAtualizada = service
        .$listaOferta()
        .find((o) => o.oferta.numeroOpcao === numeroOpcaoAlvo);
      expect(ofertaAtualizada?.oferta.justificativaInscricao).toBe(
        'Nova justificativa',
      );
      expect(ofertaService.validarLiberarBotaoProximo).toHaveBeenCalledTimes(1);
    });
  });

  describe('Funcionalidade: Opções de Convocação', () => {
    it('deve retornar um array de números de 1 até o máximo de convocação', () => {
      // Arrange
      selecaoOfertaDataServiceMock.$selecaoOferta.set({
        numeroMaximoConvocacaoPorInscricao: 4,
      } as any);

      // Act
      const opcoes = service.opcoesParaConvocacao();

      // Assert
      expect(opcoes).toEqual([1, 2, 3, 4]);
    });

    it('deve retornar um array vazio se o máximo de convocação não for definido', () => {
      // Arrange
      selecaoOfertaDataServiceMock.$selecaoOferta.set({
        numeroMaximoConvocacaoPorInscricao: null,
      } as any);

      // Act
      const opcoes = service.opcoesParaConvocacao();

      // Assert
      expect(opcoes).toEqual([]);
    });

    it('deve atualizar o numeroOpcoesDesejadas ao selecionar uma opção', () => {
      // Arrange
      const evento = { value: 3 };
      service.selecaoOferta = new SelecaoOfertaModel();

      // Act
      service.opcaoSelecionadaConvocacao(evento);

      // Assert
      expect(service.selecaoOferta.numeroOpcoesDesejadas).toBe(3);
    });
  });

  describe('Funcionalidade: Verificação de Oferta Ativa', () => {
    it('deve retornar uma classe de desativado se a oferta estiver impedida', () => {
      // Arrange
      const listaOferta: ListaOfertaModel = {
        oferta: { ofertaImpedida: true } as Ofertas,
      } as ListaOfertaModel;

      // Act
      const classe = service.verificaOfertaAtiva(listaOferta);

      // Assert
      expect(classe).toBe(service.ofertaDesativada);
    });

    it('deve retornar uma string vazia se a oferta não estiver impedida', () => {
      // Arrange
      const listaOferta: ListaOfertaModel = {
        oferta: { ofertaImpedida: false } as Ofertas,
      } as ListaOfertaModel;

      // Act
      const classe = service.verificaOfertaAtiva(listaOferta);

      // Assert
      expect(classe).toBe('');
    });
  });

  describe('Funcionalidade: Manipulação de Classes CSS para Drag-and-Drop', () => {
    let divPai: HTMLDivElement;

    beforeEach(() => {
      divPai = document.createElement('div');
      divPai.setAttribute('cdkDrag', '');
      document.body.appendChild(divPai);
    });

    afterEach(() => {
      document.body.removeChild(divPai);
    });

    it('adcionarClasseMobile: deve adicionar classe após delay', fakeAsync(() => {
      // 1. Envolva o teste com fakeAsync
      // Arrange
      const evento = { target: divPai } as any;

      // Act
      service.adcionarClasseMobile(evento);
      tick(501); // 2. Use tick() para avançar o tempo

      // Assert
      expect(divPai.classList.contains('smc-gpi-selecao-touch')).toBeTrue();
    }));

    it('removeClasseMobile: deve remover classe e limpar timeout', fakeAsync(() => {
      // 1. Envolva o teste com fakeAsync
      // Arrange
      const evento = { target: divPai } as any;
      divPai.classList.add('smc-gpi-selecao-touch');
      spyOn(window, 'clearTimeout').and.callThrough();

      // Act
      service.adcionarClasseMobile(evento); // Inicia um timeout
      service.removeClasseMobile(evento); // Limpa o timeout
      tick(101); // 2. Avança o tempo para garantir que o segundo timeout também seja executado

      // Assert
      expect(clearTimeout).toHaveBeenCalledWith(service.timeoutDragdrop);
      expect(divPai.classList.contains('smc-gpi-selecao-touch')).toBeFalse();
    }));

    // Os testes síncronos permanecem inalterados
    it('adcionarClasseDesktop: deve adicionar classe imediatamente', () => {
      const evento = { target: divPai } as any;
      service.adcionarClasseDesktop(evento);
      expect(divPai.classList.contains('smc-gpi-selecao-touch')).toBeTrue();
    });

    it('removeClasseDesktop: deve remover a classe de todos os elementos arrastáveis', () => {
      // ... (código do teste sem alterações)
      divPai.classList.add('smc-gpi-selecao-touch');
      service.removeClasseDesktop();
      expect(divPai.classList.contains('smc-gpi-selecao-touch')).toBeFalse();
    });
  });
});
