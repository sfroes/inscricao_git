import { AfterViewInit, Component, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { LayoutProcessoDataService } from '../../layout-processo/service/layout-processo-data.service';
import { PaginaErrosDataService } from './service/pagina-erros-data.service';

@Component({
  selector: 'app-pagina-erros',
  imports: [ButtonModule],
  standalone: true,
  templateUrl: './pagina-erros.component.html',
  styleUrl: './pagina-erros.component.scss',
})
export class PaginaErrosComponent implements AfterViewInit {
  //#region injeção
  paginaErrosDataService = inject(PaginaErrosDataService);
  layoutProcessoDataService = inject(LayoutProcessoDataService);
  //#endregion

  ngAfterViewInit(): void {
    this.paginaErrosDataService.setMensagemErro(
      sessionStorage.getItem('erroMessage')!,
    );
    this.paginaErrosDataService.setUrlRetorno(
      sessionStorage.getItem('urlRetorno')!,
    );
    this.layoutProcessoDataService.setExbirComponenteMenuLateral(false);

    const intervalID = setInterval(() => {
      const userMenuDiv = document.querySelector('.user.user-menu.dropdown');
      const skDescricao = document.getElementById('sk-descricaoprocesso');

      if (skDescricao) {
        skDescricao.remove();
      }
      if (userMenuDiv) {
        let count = 0;
        const divMenu = setInterval(() => {
          const divMeusDados = document.querySelector('.smc-ico-meusdados');
          if (divMeusDados) {
            this.removerMenus();
          }
          count++;
          if (count == 5) {
            clearInterval(divMenu);
          }
        }, 500);
        clearInterval(intervalID);
      }
    }, 100);
  }

  redirecionar() {
    this.paginaErrosDataService.setAcionarBotaoSair(true);
    document.location.href = this.paginaErrosDataService.$urlRetorno();
  }

  adicionarClickNoMenuBotao() {
    const usabilidadeMenuDiv = document.querySelector(
      '.smc-acessibilidade.dropdown',
    );
    const toggleButtonUsabilidade = usabilidadeMenuDiv?.querySelector(
      '.dropdown-toggle.smc-btn-acessibilidade',
    );

    if (toggleButtonUsabilidade && usabilidadeMenuDiv) {
      toggleButtonUsabilidade.addEventListener('click', (event) => {
        event.stopPropagation(); // Evita que o clique no botão remova a classe
        usabilidadeMenuDiv.classList.toggle('open');
        //userMenuDiv?.classList.remove('open');
      });

      // Remove a classe 'open' ao clicar em qualquer outra parte do documento
      document.addEventListener('click', () => {
        if (usabilidadeMenuDiv.classList.contains('open')) {
          usabilidadeMenuDiv.classList.remove('open');
        }
      });
    }
  }

  adicionarEventosContraste() {
    const btnFonteMenor = document.querySelector(
      '[data-behavior="fonteMenor"]',
    );
    const btnFonteNormal = document.querySelector(
      '[data-behavior="fonteNormal"]',
    );
    const btnFonteMaior = document.querySelector(
      '[data-behavior="fonteMaior"]',
    );
    const btnContraste = document.querySelector('[data-behavior="contraste"]');

    const body = document.body;

    if (btnFonteMenor) {
      btnFonteMenor.addEventListener('click', () => {
        body.classList.remove('smc-acessibilidade1');
        body.classList.add('smc-acessibilidade-1');
      });
    }

    if (btnFonteNormal) {
      btnFonteNormal.addEventListener('click', () => {
        body.classList.remove('smc-acessibilidade-1', 'smc-acessibilidade1');
      });
    }

    if (btnFonteMaior) {
      btnFonteMaior.addEventListener('click', () => {
        body.classList.remove('smc-acessibilidade-1');
        body.classList.add('smc-acessibilidade1');
      });
    }

    if (btnContraste) {
      btnContraste.addEventListener('click', () => {
        body.classList.toggle('smc-acessibilidade-contraste');
      });
    }
  }

  removerMenus() {
    const divMeusDados = document.querySelector('.smc-ico-meusdados');
    if (divMeusDados) {
      const listItem = divMeusDados.parentElement;
      listItem?.remove();
    }

    const divUser = document.querySelector('.user-menu');
    if (divUser) {
      const listItem = divUser.parentElement;
      listItem?.remove();
    }
    this.adicionarClickNoMenuBotao();
    this.adicionarEventosContraste();
  }
}
