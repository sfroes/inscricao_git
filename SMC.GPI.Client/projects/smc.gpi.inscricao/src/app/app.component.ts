import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule],
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'SMC.GPI.Inscricao';

  constructor(private router: Router) {
    const url = sessionStorage.getItem('urlRedirect');
    if (url) {
      window.location.href = url;
      sessionStorage.removeItem('urlRedirect');
    }
  }
}
