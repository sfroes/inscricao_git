import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginaErrosComponent } from './pagina-erros.component';

describe('PaginaErrosComponent', () => {
  let component: PaginaErrosComponent;
  let fixture: ComponentFixture<PaginaErrosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaginaErrosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaginaErrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
