import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../environments/environment';
import { LayoutProcessoService } from './service/layout-processo.service';

// describe('LayoutProcessoService', () => {
//   let service: LayoutProcessoService;
//   let httpMock: HttpTestingController;

//   beforeEach(() => {
//     TestBed.configureTestingModule({
//       imports: [HttpClientTestingModule],
//       providers: [LayoutProcessoService],
//     });
//     service = TestBed.inject(LayoutProcessoService);
//     httpMock = TestBed.inject(HttpTestingController);
//   });

//   afterEach(() => {
//     httpMock.verify(); // Verifica se todas as requisições foram realizadas
//   });

//   it('deve ser criado', () => {
//     expect(service).toBeTruthy();
//   });

//   it('deve carregar o header dinamicamente', (done) => {
//     const caminho = '/header/conteudo';
//     const mockResponse = '<header>Header HTML</header>';

//     service.carregarHeader(caminho).subscribe((conteudo) => {
//       expect(conteudo).toBe(mockResponse);
//       done();
//     });

//     const req = httpMock.expectOne(`${environment.frontUrl}${caminho}`);
//     expect(req.request.method).toBe('GET');
//     expect(req.request.responseType).toBe('text');

//     req.flush(mockResponse);
//   });

//   it('deve carregar o footer dinamicamente', (done) => {
//     const caminho = '/footer/conteudo';
//     const mockResponse = '<footer>Footer HTML</footer>';

//     service.carregarFooter(caminho).subscribe((conteudo) => {
//       expect(conteudo).toBe(mockResponse);
//       done();
//     });

//     const req = httpMock.expectOne(`${environment.frontUrl}${caminho}`);
//     expect(req.request.method).toBe('GET');
//     expect(req.request.responseType).toBe('text');

//     req.flush(mockResponse);
//   });

//   it('deve buscar a descrição do processo de inscrição', (done) => {
//     const caminho = '/processo/descricao';
//     const seqInscricao = '12345';
//     const mockResponse = { descricao: 'Processo de inscrição detalhado' };

//     service
//       .buscarDescricaoProcessoInscricao(caminho, seqInscricao)
//       .subscribe((resposta) => {
//         expect(resposta).toEqual(mockResponse);
//         done();
//       });

//     const req = httpMock.expectOne(
//       `${environment.frontUrl}${caminho}?seqInscricao=${seqInscricao}`
//     );
//     expect(req.request.method).toBe('GET');

//     req.flush(mockResponse);
//   });
// });
