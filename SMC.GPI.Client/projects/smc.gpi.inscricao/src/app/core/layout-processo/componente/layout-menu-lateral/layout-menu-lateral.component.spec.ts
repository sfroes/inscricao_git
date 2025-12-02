import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';
import { LayoutMenuLateralComponent } from './layout-menu-lateral.component';
import { LayoutMenuLateralService } from './service/layout-menu-lateral.service';

// describe('LayoutMenuLateralComponent', () => {
//   let component: LayoutMenuLateralComponent;
//   let fixture: ComponentFixture<LayoutMenuLateralComponent>;
//   let layoutMenuLateralServiceMock: jasmine.SpyObj<LayoutMenuLateralService>;
//   let sanitizerMock: jasmine.SpyObj<DomSanitizer>;

//   beforeEach(() => {
//     layoutMenuLateralServiceMock = jasmine.createSpyObj(
//       'LayoutMenuLateralService',
//       ['menuLateral$']
//     );
//     sanitizerMock = jasmine.createSpyObj('DomSanitizer', [
//       'bypassSecurityTrustHtml',
//     ]);

//     TestBed.configureTestingModule({
//       imports: [LayoutMenuLateralComponent], // Importa o componente standalone
//       providers: [
//         {
//           provide: LayoutMenuLateralService,
//           useValue: layoutMenuLateralServiceMock,
//         },
//         { provide: DomSanitizer, useValue: sanitizerMock },
//       ],
//     }).compileComponents();

//     fixture = TestBed.createComponent(LayoutMenuLateralComponent);
//     component = fixture.componentInstance;
//   });

//   it('should create the component', () => {
//     expect(component).toBeTruthy();
//   });

//   it('should call menuLateral$ and update conteudo and isLoading when data is received', () => {
//     const menuLateralSubject = new BehaviorSubject<string | null>(
//       '<div>Mock Menu Lateral</div>'
//     );
//     layoutMenuLateralServiceMock.menuLateral$ =
//       menuLateralSubject.asObservable();
//     sanitizerMock.bypassSecurityTrustHtml.and.returnValue('Safe HTML');
//     fixture.detectChanges(); // Dispara a execução do ngOnInit

//     expect(sanitizerMock.bypassSecurityTrustHtml).toHaveBeenCalledWith(
//       '<div>Mock Menu Lateral</div>'
//     );
//     expect(component.conteudo).toBe('Safe HTML');
//     expect(component.isLoading()).toBe(false);
//   });

//   it('should execute scripts when content is loaded', (done) => {
//     const mockResponse =
//       '<div><script>console.log("test script")</script></div>';
//     const menuLateralSubject = new BehaviorSubject<string | null>(mockResponse);
//     layoutMenuLateralServiceMock.menuLateral$ =
//       menuLateralSubject.asObservable();
//     sanitizerMock.bypassSecurityTrustHtml.and.returnValue('Safe HTML');

//     // Espiando o método getElementById
//     spyOn(document, 'getElementById').and.returnValue({
//       getElementsByTagName: () => [
//         { textContent: 'console.log("test script")' },
//       ],
//     } as any);

//     // Espiando a função appendChild
//     const appendChildSpy = spyOn(
//       document.body,
//       'appendChild'
//     ).and.callThrough();

//     fixture.detectChanges(); // Dispara o ngOnInit

//     setTimeout(() => {
//       expect(appendChildSpy).toHaveBeenCalled(); // Verifica se appendChild foi chamado
//       done(); // Chama done para finalizar o teste assíncrono
//     }, 0);
//   });

//   it('should not change isLoading if menuLateral$ emits null or undefined', () => {
//     const menuLateralSubject = new BehaviorSubject<string | null>(null);
//     layoutMenuLateralServiceMock.menuLateral$ =
//       menuLateralSubject.asObservable();
//     fixture.detectChanges();
//     expect(component.isLoading()).toBe(true);

//     menuLateralSubject.next(null); // Emite undefined
//     fixture.detectChanges();
//     expect(component.isLoading()).toBe(true);
//   });
// });
