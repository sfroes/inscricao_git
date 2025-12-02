import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { environment } from '../../environments/environment';
import { LayoutProcessoService } from './layout-processo.service';

describe('LayoutProcessoService', () => {
  let service: LayoutProcessoService;
  let httpMock: HttpTestingController;
  let activatedRoute: ActivatedRoute;

  const mockQueryParams = { seqInscricao: '12345' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        LayoutProcessoService,
        {
          provide: ActivatedRoute,
          useValue: {
            queryParams: of(mockQueryParams),
          },
        },
      ],
    });

    service = TestBed.inject(LayoutProcessoService);
    httpMock = TestBed.inject(HttpTestingController);
    activatedRoute = TestBed.inject(ActivatedRoute);
  });

  afterEach(() => {
    httpMock.verify(); // Garante que não há requisições pendentes
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });

  it('deve carregar o conteúdo do header', () => {
    const caminho = 'test/header';
    const mockHeaderHtml = '<h1>Header de Teste</h1>';
    const url = environment.frontUrl + caminho;

    service.carregarHeader(caminho).subscribe((response) => {
      expect(response).toEqual(mockHeaderHtml);
    });

    const req = httpMock.expectOne(url);
    expect(req.request.method).toBe('GET');
    req.flush(mockHeaderHtml);
  });

  it('deve buscar e definir a descrição do processo de inscrição', () => {
    const seqInscricao = '123';
    const mockDescricao = 'Descrição do Processo de Teste';
    const caminho = 'Ins/Front/BuscarDescricaoProcesso';
    const url = `${environment.frontUrl}${caminho}?seqInscricao=${seqInscricao}`;

    service.buscarDescricaoProcessoInscricao(seqInscricao);

    const req = httpMock.expectOne(url);
    expect(req.request.method).toBe('GET');
    req.flush(mockDescricao);

    expect(service.descricaoProcesso$()).toEqual(mockDescricao);
  });

  it('deve validar e atualizar a URL do CSS quando o href for @urlCSS', () => {
    // Prepara o DOM com o elemento CSS
    const linkElement = document.createElement('link');
    linkElement.id = 'cssProcesso';
    linkElement.setAttribute('href', '@urlCSS');
    document.head.appendChild(linkElement);

    const mockCssUrl = 'caminho/para/meu.css';
    const origem = window.location.origin;
    const isDev = origem.includes('localhost');
    let caminhoApi = `/GPI.Inscricao/Inscricao/BuscarUrlCss?seqInscricao=${mockQueryParams.seqInscricao}`;
    if (isDev) {
      caminhoApi = `/Dev${caminhoApi}`;
    }
    const urlApi = origem + caminhoApi;

    service.validarUrlCss();

    const req = httpMock.expectOne(urlApi);
    expect(req.request.method).toBe('GET');
    req.flush(mockCssUrl);

    const expectedHref = `${origem}/Recursos/Inscricoes/4.0/GPI.Inscricao/${mockCssUrl}`;
    expect(linkElement.getAttribute('href')).toBe(expectedHref);

    // Limpa o DOM
    document.head.removeChild(linkElement);
  });

  it('não deve fazer requisição de CSS se o href não for @urlCSS', () => {
    const linkElement = document.createElement('link');
    linkElement.id = 'cssProcesso';
    linkElement.setAttribute('href', 'caminho/ja/definido.css');
    document.head.appendChild(linkElement);

    service.validarUrlCss();

    httpMock.expectNone(() => true); // Nenhuma requisição HTTP deve ser feita

    document.head.removeChild(linkElement);
  });
});
