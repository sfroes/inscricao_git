import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HierarquiaModel } from '../model/herarquia.model';
import { SelecaoOfertaHierarquiaComponent } from './selecao-oferta-hierarquia.component';

// describe('SelecaoOfertaHierarquiaComponent', () => {
//   let component: SelecaoOfertaHierarquiaComponent;
//   let fixture: ComponentFixture<SelecaoOfertaHierarquiaComponent>;

//   beforeEach(async () => {
//     await TestBed.configureTestingModule({
//       imports: [SelecaoOfertaHierarquiaComponent], // Importa o componente standalone
//     }).compileComponents();
//   });

//   beforeEach(() => {
//     fixture = TestBed.createComponent(SelecaoOfertaHierarquiaComponent);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('deveria criar', () => {
//     expect(component).toBeTruthy();
//   });

//   it('deve definir os arquivos corretamente quando dataArray é definido com dados válidos', () => {
//     const data: HierarquiaModel[] = [
//       {
//         seq: 1,
//         seqPai: 0,
//         descricao: 'Raiz 1',
//         descricaoComplementar: 'Descrição Raiz 1',
//         isLeaf: false,
//       },
//       {
//         seq: 2,
//         seqPai: 1,
//         descricao: 'Filho 1',
//         descricaoComplementar: 'Descrição Filho 1',
//         isLeaf: true,
//       },
//       {
//         seq: 3,
//         seqPai: 0,
//         descricao: 'Raiz 2',
//         descricaoComplementar: 'Descrição Raiz 2',
//         isLeaf: false,
//       },
//       {
//         seq: 4,
//         seqPai: 3,
//         descricao: 'Filho 2',
//         descricaoComplementar: 'Descrição Filho 2',
//         isLeaf: true,
//       },
//     ];

//     component.dadosHierarquia = data;

//     expect(component.files.length).toBe(2);
//     expect(component.files[0].label).toBe('Raiz 1');
//     expect(component.files[0].children?.length).toBe(1);
//     expect(component.files[0].children?.[0].label).toBe('Filho 1');
//     expect(component.files[1].label).toBe('Raiz 2');
//     expect(component.files[1].children?.length).toBe(1);
//     expect(component.files[1].children?.[0].label).toBe('Filho 2');
//   });
// });
