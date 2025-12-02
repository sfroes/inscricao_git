import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LayoutHeaderComponent } from './layout-header.component';
import { DomSanitizer } from '@angular/platform-browser';
import { of } from 'rxjs';
import { LayoutProcessoService } from '../../service/layout-processo.service';
import { LayoutNavegacaoDataService } from './../layout-navegacao/service/layout-navegacao-data.service';

// describe('LayoutHeaderComponent', () => {
//   let component: LayoutHeaderComponent;
//   let fixture: ComponentFixture<LayoutHeaderComponent>;
//   let mockLayoutProcessoService: jasmine.SpyObj<LayoutProcessoService>;

//   beforeEach(async () => {
//     mockLayoutProcessoService = jasmine.createSpyObj('LayoutProcessoService', [
//       'carregarHeader',
//     ]);
//     await TestBed.configureTestingModule({
//       imports: [LayoutHeaderComponent],
//       providers: [
//         { provide: LayoutProcessoService, useValue: mockLayoutProcessoService },
//         LayoutNavegacaoDataService,
//         SelecaoOfertaVazioService,
//         DomSanitizer,
//       ],
//     }).compileComponents();

//     fixture = TestBed.createComponent(LayoutHeaderComponent);
//     component = fixture.componentInstance;
//   });

//   it('deve fechar os menus ao clicar fora deles', () => {
//     const mockHtmlContent = `
//       <div class="user user-menu dropdown">
//         <button class="dropdown-toggle smc-btn-usuario"></button>
//       </div>
//       <div class="smc-acessibilidade dropdown">
//         <button class="dropdown-toggle smc-btn-acessibilidade"></button>
//       </div>`;
//     mockLayoutProcessoService.carregarHeader.and.returnValue(
//       of(mockHtmlContent),
//     );

//     fixture.detectChanges(); // Dispara o ciclo de inicialização do componente.

//     // Recupera os elementos necessários do DOM.
//     const userMenuDiv = document.querySelector('.user.user-menu.dropdown')!;
//     const toggleButtonProfile = userMenuDiv.querySelector(
//       '.dropdown-toggle.smc-btn-usuario',
//     )!;
//     const usabilidadeMenuDiv = document.querySelector(
//       '.smc-acessibilidade.dropdown',
//     )!;
//     const toggleButtonUsabilidade = usabilidadeMenuDiv.querySelector(
//       '.dropdown-toggle.smc-btn-acessibilidade',
//     )!;

//     // Simula clique no botão de perfil.
//     toggleButtonProfile.dispatchEvent(new Event('click'));
//     expect(userMenuDiv.classList.contains('open')).toBeTrue();

//     // Simula clique fora do menu.
//     document.body.click();
//     expect(userMenuDiv.classList.contains('open')).toBeFalse();

//     // Simula clique no botão de acessibilidade.
//     toggleButtonUsabilidade.dispatchEvent(new Event('click'));
//     expect(usabilidadeMenuDiv.classList.contains('open')).toBeTrue();

//     // Simula clique fora do menu.
//     document.body.click();
//     expect(usabilidadeMenuDiv.classList.contains('open')).toBeFalse();
//   });
// });
