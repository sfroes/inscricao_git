import { HttpClientModule } from '@angular/common/http'; // Importa HttpClientModule
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LayoutNavegacaoComponent } from './layout-navegacao.component';

// describe('LayoutNavegacaoComponent', () => {
//   let component: LayoutNavegacaoComponent;
//   let fixture: ComponentFixture<LayoutNavegacaoComponent>;

//   beforeEach(async () => {
//     await TestBed.configureTestingModule({
//       imports: [HttpClientModule, LayoutNavegacaoComponent], // Importa o componente standalone e HttpClientModule
//     }).compileComponents();

//     fixture = TestBed.createComponent(LayoutNavegacaoComponent);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('deve criar o componente', () => {
//     expect(component).toBeTruthy();
//   });

//   it('deve desabilitar todos os botões quando bloquearTodosBotoes for chamado com true', () => {
//     component.desativarTodosBotoes(true);
//     expect(component.desabilitarBotaoAnterior()).toBeTrue();
//     expect(component.desabilitarBotaoCancelar()).toBeTrue();
//     expect(component.desabilitarBotaoProximo()).toBeTrue();
//   });

//   it('deve habilitar todos os botões quando bloquearTodosBotoes for chamado com false', () => {
//     component.desativarTodosBotoes(false);
//     expect(component.desabilitarBotaoAnterior()).toBeFalse();
//     expect(component.desabilitarBotaoCancelar()).toBeFalse();
//     expect(component.desabilitarBotaoProximo()).toBeFalse();
//   });
// });
