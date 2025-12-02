import { TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';
import { Skeleton } from 'primeng/skeleton';
import { of } from 'rxjs';
import { LayoutProcessoService } from '../../service/layout-processo.service';
import { LayoutFooterComponent } from './layout-footer.component';

// describe('LayoutFooterComponent', () => {
//   let component: LayoutFooterComponent;
//   let layoutProcessoServiceMock: jasmine.SpyObj<LayoutProcessoService>;
//   let sanitizerMock: jasmine.SpyObj<DomSanitizer>;

//   beforeEach(() => {
//     layoutProcessoServiceMock = jasmine.createSpyObj('LayoutProcessoService', [
//       'carregarFooter',
//     ]);
//     sanitizerMock = jasmine.createSpyObj('DomSanitizer', [
//       'bypassSecurityTrustHtml',
//     ]);

//     TestBed.configureTestingModule({
//       imports: [Skeleton],
//       providers: [
//         { provide: LayoutProcessoService, useValue: layoutProcessoServiceMock },
//         { provide: DomSanitizer, useValue: sanitizerMock },
//       ],
//     }).compileComponents();

//     const fixture = TestBed.createComponent(LayoutFooterComponent);
//     component = fixture.componentInstance;
//   });

//   it('deve carregar o conteúdo do footer ao inicializar', () => {
//     const mockHtmlContent = '<p>Footer Content</p>';
//     const sanitizedContent = 'Sanitized Content';

//     layoutProcessoServiceMock.carregarFooter.and.returnValue(
//       of(mockHtmlContent)
//     );
//     sanitizerMock.bypassSecurityTrustHtml.and.returnValue(sanitizedContent);

//     component.ngOnInit();

//     expect(layoutProcessoServiceMock.carregarFooter).toHaveBeenCalledWith(
//       'INS/Front/PartialFooter'
//     );
//     expect(sanitizerMock.bypassSecurityTrustHtml).toHaveBeenCalledWith(
//       mockHtmlContent
//     );
//     expect(component.conteudo).toBe(sanitizedContent);
//     expect(component.isLoading()).toBeFalse();
//   });

//   it('deve definir isLoading como true inicialmente', () => {
//     expect(component.isLoading()).toBeTrue();
//   });
// });
