import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../../environments/environment';
import { LayoutMenuLateralService } from './layout-menu-lateral.service';

// describe('LayoutMenuLateralService', () => {
//   let service: LayoutMenuLateralService;
//   let httpMock: HttpTestingController;

//   beforeEach(() => {
//     TestBed.configureTestingModule({
//       imports: [HttpClientTestingModule],
//       providers: [LayoutMenuLateralService],
//     });
//     service = TestBed.inject(LayoutMenuLateralService);
//     httpMock = TestBed.inject(HttpTestingController);
//   });

//   afterEach(() => {
//     httpMock.verify(); // Verifica se todas as requisições foram resolvidas
//   });

//   it('deve criar o serviço', () => {
//     expect(service).toBeTruthy();
//   });

//   it('deve atualizar o conteúdo do menu lateral com sucesso', () => {
//     const caminho = '/menu/conteudo';
//     const modelo = { chave: 'valor' };
//     const mockResponse = '<div>Conteúdo do Menu</div>';

//     service.atualizarConteudoMenuLateral(caminho, modelo);

//     const req = httpMock.expectOne(`${environment.frontUrl}${caminho}`);
//     expect(req.request.method).toBe('POST');
//     expect(req.request.body).toEqual(JSON.stringify(modelo));
//     expect(req.request.headers.get('Content-Type')).toBe('application/json');

//     // Simula a resposta do servidor
//     req.flush(mockResponse);

//     // Verifica se o BehaviorSubject foi atualizado corretamente
//     service.menuLateral$.subscribe((conteudo) => {
//       expect(conteudo).toBe(mockResponse);
//     });
//   });

//   it('deve tratar erro ao atualizar o conteúdo do menu lateral', () => {
//     const caminho = '/menu/conteudo';
//     const modelo = { chave: 'valor' };
//     const mockError = { status: 500, statusText: 'Internal Server Error' };

//     spyOn(console, 'error'); // Espiona chamadas ao console.error

//     service.atualizarConteudoMenuLateral(caminho, modelo);

//     const req = httpMock.expectOne(`${environment.frontUrl}${caminho}`);
//     expect(req.request.method).toBe('POST');

//     // Simula um erro na resposta do servidor
//     req.flush(null, mockError);

//     // Verifica se o erro foi registrado no console
//     expect(console.error).toHaveBeenCalledWith(
//       'Erro ao fazer a requisição:',
//       jasmine.objectContaining(mockError)
//     );
//   });
// });
