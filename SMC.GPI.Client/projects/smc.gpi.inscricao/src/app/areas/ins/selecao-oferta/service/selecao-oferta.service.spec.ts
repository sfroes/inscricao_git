import { BreakpointObserver } from '@angular/cdk/layout';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { of } from 'rxjs';
import { environment } from '../../../../core/environments/environment';
import { LayoutNavegacaoDataService } from '../../../../core/layout-processo/componente/layout-navegacao/service/layout-navegacao-data.service';
import { LayoutProcessoDataService } from '../../../../core/layout-processo/service/layout-processo-data.service';
import { ToolsService } from '../../../../core/tools/tools.service';
import { DatasourceItemModel } from '../model/datasource-item.model';
import { HierarquiaModel } from '../model/herarquia.model';
import { SelecaoOfertaModel, Taxa } from '../model/selecao-oferta.model';
import { FiltroPaginaModel } from './../../../../core/layout-processo/model/filtro-pagina.model';
import { SelecaoOfertaDataService } from './selecao-oferta-data.service';
import { SelecaoOfertaService } from './selecao-oferta.service';

describe('SelecaoOfertaService', () => {
  let service: SelecaoOfertaService;
  let httpMock: HttpTestingController;
  let layoutProcessoDataService: LayoutProcessoDataService;
  let selecaoOfertaDataService: SelecaoOfertaDataService;
  let layoutNavegacaoDataService: LayoutNavegacaoDataService;

  // Mock data
  const mockSelecaoOfertaModel: SelecaoOfertaModel = {
    titulo: 'Seleção de Oferta',
    alertaOferta: 'Alerta de teste',
    labelOferta: 'Oferta',
    ofertas: [],
    taxas: [],
    totalGeral: 0,
    possuiBoletoPago: false,
    secoes: [{ token: 'INSTRUCOES', texto: '<p>Instruções</p>' }],
    seqInscricao: 123,
    aptoBolsa: false,
    bolsaExAluno: false,
    chaveTextoBotaoAnterior: 'Anterior',
    chaveTextoBotaoProximo: 'Próximo',
    cobrancaPorOferta: false,
    descricaoGrupoOferta: 'Grupo de Oferta Teste',
    descricaoOfertas: 0,
    descricaoProcesso: 'Processo Teste',
    descricaoSituacaoAtual: 'Em Andamento',
    exibeTermoOrientacaoPDF: false,
    exigeJustificativaOferta: false,
    fluxoPaginas: [],
    gestaoEventos: true,
    gruposTaxa: [],
    habilitaCheckin: true,
    idioma: 'pt-BR',
    imagemCabecalho: null,
    inscricaoIniciada: true,
    labelCodigoAutorizacao: null,
    labelGrupoOferta: 'Grupo Oferta',
    navigationGroup: null,
    numeroMaximoConvocacaoPorInscricao: null,
    numeroMaximoOfertaPorInscricao: null,
    numeroOpcoesDesejadas: null,
    numeroPaginaAtual: 1,
    opcoesParaConvocacao: 0,
    ordem: 1,
    orientacaoAceiteConversaoArquivosPDF: null,
    seqConfiguracaoEtapa: 1,
    seqConfiguracaoEtapaEncrypted: 'abc',
    seqConfiguracaoEtapaPagina: 2,
    seqConfiguracaoEtapaPaginaAnterior: 1,
    seqConfiguracaoEtapaPaginaAnteriorEncrypted: 'xyz',
    seqConfiguracaoEtapaPaginaEncrypted: 'def',
    seqConfiguracaoEtapaPaginaProxima: 3,
    seqConfiguracaoEtapaPaginaProximaEncrypted: 'ghi',
    seqGrupoOferta: 3,
    seqGrupoOfertaEncrypted: 'jkl',
    seqInscricaoEncrypted: 'mno',
    seqProcesso: 10,
    termoAceiteConversaoArquivosPDF: null,
    tituloPago: false,
    token: 'token-pagina-atual',
    tokenPaginaAnterior: 'token-pagina-anterior',
    tokenPaginaAnteriorEncrypted: 'pqr',
    tokenProximaPagina: 'token-proxima-pagina',
    tokenProximaPaginaEncrypted: 'stu',
    tokenResource: 'resource-token',
    tokenSituacaoAtual: 'situacao-token',
    uidProcesso: 'uid-123',
    urlCss: '',
    _seqConfiguracaoEtapaPaginaProxima: 3,
    _tokenProximaPagina: 'token-proxima-pagina',
    exibirArvoreFechada: false,
    tipoCobrancaPorQuantidadeOferta: false,
    instrucaoTaxa: null,
    processoPossuiTaxa: false,
    permiteAlterarBoleto: false,
  };

  const mockHierarquia: HierarquiaModel[] = [
    { seq: 1, descricao: 'Nível 1', isLeaf: false, seqPai: 1 },
    { seq: 2, descricao: 'Nível 2', isLeaf: true, seqPai: 2 },
  ];

  beforeEach(() => {
    const mockActivatedRoute = {
      queryParams: of({
        seqConfiguracaoEtapa: '1',
        idioma: 'pt-BR',
        seqConfiguracaoEtapaPagina: '2',
        seqGrupoOferta: '3',
        seqInscricao: '4',
      }),
    };

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        SelecaoOfertaService,
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        {
          provide: DomSanitizer,
          useValue: { bypassSecurityTrustHtml: (v: any) => v },
        },
        MessageService,
        BreakpointObserver,
        SelecaoOfertaDataService,
        LayoutProcessoDataService,
        LayoutNavegacaoDataService,
        ToolsService,
      ],
    });

    service = TestBed.inject(SelecaoOfertaService);
    httpMock = TestBed.inject(HttpTestingController);
    layoutProcessoDataService = TestBed.inject(LayoutProcessoDataService);
    selecaoOfertaDataService = TestBed.inject(SelecaoOfertaDataService);
    layoutNavegacaoDataService = TestBed.inject(LayoutNavegacaoDataService);

    // Spy on services to avoid actual method calls
    spyOn(layoutProcessoDataService, 'setDadosMenuLateral');
    spyOn(layoutProcessoDataService, 'setDadosNavegacao');
    spyOn(layoutProcessoDataService, 'setDesativarBotaoProximo');
    spyOn(layoutNavegacaoDataService, 'setErrosBotaoProximo');
    spyOn(selecaoOfertaDataService, 'setSelecaoOferta');
    spyOn(selecaoOfertaDataService, 'setListaOfertas');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });

  it('deve buscar os dados da seleção de oferta e inicializar as propriedades em buscarSelecaoOferta', () => {
    service.buscarSelecaoOferta();

    const reqSelecao = httpMock.expectOne((req) =>
      req.url.includes('/INS/Inscricao/SelecaoOfertaAngular'),
    );
    expect(reqSelecao.request.method).toBe('GET');
    reqSelecao.flush(mockSelecaoOfertaModel);

    const reqHierarquia = httpMock.expectOne((req) =>
      req.url.includes('/INS/Inscricao/BuscarHierarquiaAngular'),
    );
    expect(reqHierarquia.request.method).toBe('GET');
    reqHierarquia.flush(mockHierarquia);

    expect(service.$titulo()).toBe(mockSelecaoOfertaModel.titulo);
    expect(service.$alertaOferta()).toBe(mockSelecaoOfertaModel.alertaOferta);
    expect(selecaoOfertaDataService.setSelecaoOferta).toHaveBeenCalledWith(
      mockSelecaoOfertaModel,
    );
    expect(service.$hierarquia()).toEqual(mockHierarquia);
    expect(layoutProcessoDataService.setDadosMenuLateral).toHaveBeenCalled();
    expect(layoutProcessoDataService.setDadosNavegacao).toHaveBeenCalled();
    expect(service.$isLoading()).toBe(false);
  });

  it('deve obter a seleção de oferta com getBuscarSelecaoOferta', () => {
    const filtro: FiltroPaginaModel = {
      seqGrupoOferta: '1',
    } as FiltroPaginaModel;
    const path = '/INS/Inscricao/SelecaoOfertaAngular';

    service.getBuscarSelecaoOferta(path, filtro).subscribe((res) => {
      expect(res).toEqual(mockSelecaoOfertaModel);
    });

    const req = httpMock.expectOne(
      `${environment.frontUrl}${path}?seqGrupoOferta=1`,
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockSelecaoOfertaModel);
  });

  it('deve buscar a hierarquia com buscarHierarquia', () => {
    const filtro: FiltroPaginaModel = {
      seqGrupoOferta: '1',
    } as FiltroPaginaModel;
    const path = '/INS/Inscricao/BuscarHierarquiaAngular';

    service.buscarHierarquia(path, filtro).subscribe((res) => {
      expect(res).toEqual(mockHierarquia);
    });

    const req = httpMock.expectOne(
      `${environment.frontUrl}${path}?seqGrupoOferta=1`,
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockHierarquia);
  });

  it('deve listar as taxas com listarTaxasOfertaInscricaoAngular', async () => {
    const mockTaxas: Taxa[] = [{ seqTaxa: 1, descricao: 'Taxa 1' } as Taxa];
    const path = '/INS/Inscricao/ListarTaxasOfertaInscricaoAngular';
    const seqOferta = 1;
    const seqInscricao = 123;

    const promise = service.listarTaxasOfertaInscricaoAngular(
      path,
      seqOferta,
      seqInscricao,
    );

    const req = httpMock.expectOne(
      (req) =>
        req.url === `${environment.frontUrl}${path}` &&
        req.params.get('seqOferta') === seqOferta.toString() &&
        req.params.get('seqInscricao') === seqInscricao.toString(),
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockTaxas);

    const result = await promise;
    expect(result).toEqual(mockTaxas);
  });

  it('deve buscar a hierarquia completa com buscarHierarquiaCompletaOferta', async () => {
    const path = '/INS/Inscricao/BuscarHierarquiaCompletaAngular';
    const seqOferta = 1;

    const promise = service.buscarHierarquiaCompletaOferta(path, seqOferta);

    const req = httpMock.expectOne(
      `${environment.frontUrl}${path}?seqOferta=${seqOferta}`,
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockHierarquia);

    const result = await promise;
    expect(result).toEqual(mockHierarquia);
  });

  it('deve buscar as descrições das ofertas com buscarDescricaoSelecaoOfertasInscricaoSeqsOfertas', async () => {
    const mockDescricoes: DatasourceItemModel[] = [
      { seq: 1, descricao: 'Oferta 1' },
    ];
    const path =
      '/INS/Inscricao/BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas';
    const seqsOfertas = [1, 2];

    const promise = service.buscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(
      path,
      seqsOfertas,
    );

    const req = httpMock.expectOne(
      (req) =>
        req.url === `${environment.frontUrl}${path}` &&
        req.params.get('seqsOfertas') === '1,2',
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockDescricoes);

    const result = await promise;
    expect(result).toEqual(mockDescricoes);
  });

  it('deve retornar verdadeiro para exibirBotaoExcluir quando as condições são atendidas', () => {
    // numeroNoFolha > 1 && numeroMaximoOfertaPorInscricao != 1
    let result = service.exibirBotaoExcluir(2, 2, false);
    expect(result).toBe(true);

    // ofertaImpedida is true
    result = service.exibirBotaoExcluir(1, 1, true);
    expect(result).toBe(true);
  });

  it('deve retornar falso para exibirBotaoExcluir quando as condições não são atendidas', () => {
    // numeroNoFolha <= 1 or numeroMaximoOfertaPorInscricao == 1
    let result = service.exibirBotaoExcluir(1, 2, false);
    expect(result).toBe(false);

    result = service.exibirBotaoExcluir(2, 1, false);
    expect(result).toBe(false);
  });

  it('deve retornar verdadeiro para exibirBotaoSelecionarHierarquia quando numeroNoFolha > 1', () => {
    service['_numeroNoFolha'].set(2);
    const result = service.exibirBotaoSelecionarHierarquia();
    expect(result).toBe(true);
  });

  it('deve retornar falso para exibirBotaoSelecionarHierarquia quando numeroNoFolha <= 1', () => {
    service['_numeroNoFolha'].set(1);
    const result = service.exibirBotaoSelecionarHierarquia();
    expect(result).toBe(false);
  });

  it('deve lidar com o salvamento da seleção com sucesso', async () => {
    const postData = { statusCode: 200, data: 'param=value' };
    spyOnProperty(window, 'location', 'get').and.returnValue({
      href: '',
    } as any);

    const promise = service.salvarSelecaoOferta();

    const req = httpMock.expectOne(
      `${environment.frontUrl}INS/Inscricao/SalvarSelecaoOfertaAngular`,
    );
    expect(req.request.method).toBe('POST');
    req.flush(postData);

    const result = await promise;
    expect(result).toBe(true);
    expect(window.location.href).toBe(
      `${environment.frontUrl}INS/Inscricao/UrlPagina?param=value`,
    );
  });

  it('deve lidar com o salvamento da seleção com código de status de erro', async () => {
    const postData = { statusCode: 500, data: 'Error message' };
    const messageService = TestBed.inject(MessageService);
    spyOn(messageService, 'add');

    const promise = service.salvarSelecaoOferta();

    const req = httpMock.expectOne(
      `${environment.frontUrl}INS/Inscricao/SalvarSelecaoOfertaAngular`,
    );
    expect(req.request.method).toBe('POST');
    req.flush(postData);

    const result = await promise;
    expect(result).toBe(true); // The function returns true even on error to stop the chain
    expect(messageService.add).toHaveBeenCalledWith(
      jasmine.objectContaining({ severity: 'error' }),
    );
  });
});
