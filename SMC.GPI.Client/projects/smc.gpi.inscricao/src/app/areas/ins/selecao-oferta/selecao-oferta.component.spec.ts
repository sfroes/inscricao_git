import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http'; // Importa HttpClientModule
import { of, BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { SelecaoOfertaComponent } from './selecao-oferta.component';
import { SelecaoOfertaService } from './service/selecao-oferta.service';
import { LayoutMenuLateralService } from '../../../core/layout-processo/componente/layout-menu-lateral/service/layout-menu-lateral.service';
import { FiltroPaginaModel } from '../../../core/layout-processo/model/filtro-pagina.model';
import { SelecaoOfertaModel } from './model/selecao-oferta.model';

// describe('SelecaoOfertaComponent', () => {
//   let component: SelecaoOfertaComponent;
//   let fixture: ComponentFixture<SelecaoOfertaComponent>;
//   let selecaoOfertaServiceMock: jasmine.SpyObj<SelecaoOfertaService>;
//   let menuLateralServiceMock: jasmine.SpyObj<LayoutMenuLateralService>;
//   let activatedRouteMock: Partial<ActivatedRoute>;

//   beforeEach(() => {
//     selecaoOfertaServiceMock = jasmine.createSpyObj('SelecaoOfertaService', [
//       'buscarSelecaoOferta',
//       'buscarHierarquia',
//     ]);
//     selecaoOfertaServiceMock.abrirModalSelecaoOferta$ =
//       new BehaviorSubject<boolean>(false);
//     menuLateralServiceMock = jasmine.createSpyObj('LayoutMenuLateralService', [
//       'atualizarConteudoMenuLateral',
//     ]);
//     activatedRouteMock = {
//       queryParams: of({
//         seqConfiguracaoEtapa: '123',
//         idioma: 'pt',
//         seqConfiguracaoEtapaPagina: '1',
//         seqGrupoOferta: '456',
//         seqInscricao: '789',
//       }),
//     };

//     TestBed.configureTestingModule({
//       imports: [HttpClientModule], // Adiciona HttpClientModule aos imports
//       providers: [
//         { provide: SelecaoOfertaService, useValue: selecaoOfertaServiceMock },
//         { provide: LayoutMenuLateralService, useValue: menuLateralServiceMock },
//         { provide: ActivatedRoute, useValue: activatedRouteMock },
//       ],
//     }).compileComponents();

//     fixture = TestBed.createComponent(SelecaoOfertaComponent);
//     component = fixture.componentInstance;
//   });

//   it('deve buscar a seleção de oferta e atualizar o menu lateral', () => {
//     // Configura o mock para retornar um Observable com valor simulado
//     const mockResponse = new SelecaoOfertaModel();
//     selecaoOfertaServiceMock.getBuscarSelecaoOferta.and.returnValue(
//       of(mockResponse),
//     );

//     component.buscarSelecaoOferta();

//     expect(selecaoOfertaServiceMock.getBuscarSelecaoOferta).toHaveBeenCalledWith(
//       '/INS/Inscricao/SelecaoOfertaAngular',
//       jasmine.any(FiltroPaginaModel),
//     );

//     expect(
//       menuLateralServiceMock.atualizarConteudoMenuLateral,
//     ).toHaveBeenCalledWith('INS/Front/PartialMenuLateral', mockResponse);

//     expect(component.selecaoOferta).toEqual(mockResponse);
//   });

//   it('deve configurar o filtro corretamente com base nos parâmetros da query string', () => {
//     spyOn(component, 'buscarSelecaoOferta').and.callThrough();

//     component.buscarSelecaoOferta();

//     const filtroEsperado = new FiltroPaginaModel();
//     filtroEsperado.seqConfiguracaoEtapa = '123';
//     filtroEsperado.idioma = 'pt';
//     filtroEsperado.seqConfiguracaoEtapaPagina = '1';
//     filtroEsperado.seqGrupoOferta = '456';
//     filtroEsperado.seqInscricao = '789';

//     expect(selecaoOfertaServiceMock.getBuscarSelecaoOferta).toHaveBeenCalledWith(
//       '/INS/Inscricao/SelecaoOfertaAngular',
//       jasmine.objectContaining(filtroEsperado),
//     );
//   });
// });
