import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { MessageService } from 'primeng/api'; // Importa MessageService
import { RouterTestingModule } from '@angular/router/testing';
import { ToastModule } from 'primeng/toast';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, ToastModule, AppComponent], // Importa AppComponent e ToastModule
      providers: [MessageService], // Adiciona MessageService aos providers
    }).compileComponents();
  });

  it('deveria criar o componente', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('deveria ter o título correto', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('SMC.GPI.Inscricao');
  });
});
