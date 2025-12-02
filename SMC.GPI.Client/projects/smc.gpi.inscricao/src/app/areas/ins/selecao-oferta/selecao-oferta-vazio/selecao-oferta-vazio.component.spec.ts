import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SelecaoOfertaVazioComponent } from './selecao-oferta-vazio.component';
import { BehaviorSubject } from 'rxjs';
import { SelecaoOfertaService } from '../service/selecao-oferta.service';

// describe('SelecaoOfertaVazioComponent', () => {
//   let component: SelecaoOfertaVazioComponent;
//   let fixture: ComponentFixture<SelecaoOfertaVazioComponent>;
//   let selecaoVazioService: jasmine.SpyObj<SelecaoOfertaVazioService>;
//   let selecionarOfertaService: jasmine.SpyObj<SelecaoOfertaService>;
//   let desabilitarBotaoSelecionar$: BehaviorSubject<boolean>;

//   beforeEach(async () => {
//     desabilitarBotaoSelecionar$ = new BehaviorSubject<boolean>(false);
//     const selecaoVazioServiceSpy = jasmine.createSpyObj(
//       'SelecaoOfertaVazioService',
//       [],
//       {
//         desabilitarBotaoSelecionar$: desabilitarBotaoSelecionar$,
//       },
//     );
//     const abrirModalSelecaoOferta$ = new BehaviorSubject<boolean>(false);
//     const selecionarOfertaServiceSpy = jasmine.createSpyObj(
//       'SelecaoOfertaService',
//       ['abrirModalSelecaoOferta$'],
//     );
//     selecionarOfertaServiceSpy.abrirModalSelecaoOferta$ =
//       abrirModalSelecaoOferta$;

//     await TestBed.configureTestingModule({
//       imports: [SelecaoOfertaVazioComponent],
//       providers: [
//         {
//           provide: SelecaoOfertaVazioService,
//           useValue: selecaoVazioServiceSpy,
//         },
//         { provide: SelecaoOfertaService, useValue: selecionarOfertaServiceSpy },
//       ],
//     }).compileComponents();

//     fixture = TestBed.createComponent(SelecaoOfertaVazioComponent);
//     component = fixture.componentInstance;
//     selecaoVazioService = TestBed.inject(
//       SelecaoOfertaVazioService,
//     ) as jasmine.SpyObj<SelecaoOfertaVazioService>;
//     selecionarOfertaService = TestBed.inject(
//       SelecaoOfertaService,
//     ) as jasmine.SpyObj<SelecaoOfertaService>;

//     fixture.detectChanges();
//   });

//   it('deve criar o componente', () => {
//     expect(component).toBeTruthy();
//   });

//   it('deve desabilitar o botão quando desabilitarBotaoSelecionar$ emitir true', () => {
//     desabilitarBotaoSelecionar$.next(true);
//     fixture.detectChanges();
//     expect(component.desabilitarBotaoSelecionar()).toBeTrue();
//   });

//   it('deve habilitar o botão quando desabilitarBotaoSelecionar$ emitir false', () => {
//     desabilitarBotaoSelecionar$.next(false);
//     fixture.detectChanges();
//     expect(component.desabilitarBotaoSelecionar()).toBeFalse();
//   });

//   it('deve chamar abrirModalSelecaoOferta$ quando selecionarHierarquia for chamado', () => {
//     spyOn(selecionarOfertaService.abrirModalSelecaoOferta$, 'next');
//     component.selecionarHierarquia();
//     expect(
//       selecionarOfertaService.abrirModalSelecaoOferta$.next,
//     ).toHaveBeenCalledWith(true);
//   });

//   it('deve desinscrever de todas as assinaturas ao destruir o componente', () => {
//     const unsubscribeSpy = spyOn(component['subs'], 'unsubscribe');
//     component.ngOnDestroy();
//     expect(unsubscribeSpy).toHaveBeenCalled();
//   });

//   it('deve definir desabilitarBotaoSelecionar como true quando desabilitarBotaoSelecionar$ emitir true', () => {
//     desabilitarBotaoSelecionar$.next(true);
//     fixture.detectChanges();
//     expect(component.desabilitarBotaoSelecionar()).toBeTrue();
//   });

//   it('deve definir desabilitarBotaoSelecionar como false quando desabilitarBotaoSelecionar$ emitir false', () => {
//     desabilitarBotaoSelecionar$.next(false);
//     fixture.detectChanges();
//     expect(component.desabilitarBotaoSelecionar()).toBeFalse();
//   });
// });
