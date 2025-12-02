import { Component, effect, inject, OnInit } from '@angular/core';
import { Skeleton } from 'primeng/skeleton';
import { LayoutHeaderService } from './service/layout-header.service';

import { SelecaoOfertaDataService } from '../../../../areas/ins/selecao-oferta/service/selecao-oferta-data.service';
import { LayoutProcessoDataService } from '../../service/layout-processo-data.service';
import { LayoutNavegacaoDataService } from '../layout-navegacao/service/layout-navegacao-data.service';

@Component({
  selector: 'app-layout-header',
  imports: [Skeleton],
  templateUrl: './layout-header.component.html',
})
export class LayoutHeaderComponent implements OnInit {
  //#region injeção de dependencia

  LayoutHeaderService = inject(LayoutHeaderService);
  dataServiceNavegacao = inject(LayoutNavegacaoDataService);
  //serviceSelecaoVazio = inject(SelecaoOfertaVazioService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  layoutProcessoDataService = inject(LayoutProcessoDataService);

  //#endregion
  /**
   * Carrega o conteúdo do header dinamicamente.
   * Faz uso do serviço LayoutProcessoService para obter o conteúdo HTML do header.
   * Através do RXJS, o conteúdo é processado e a flag isLoading é setada para false,
   * liberando a visualização do componente.
   * Adiciona também evento de click no menu do botão e eventos de acessibilidade.
   */
  ngOnInit(): void {
    this.LayoutHeaderService.carregarHeader();
  }

  constructor() {
    effect(() => {
      const rodarScript = this.LayoutHeaderService.rodarScripts$();
      if (rodarScript) {
        setTimeout(() => {
          this.adicionarClickNoMenuBotao();
          this.adicionarEventosContraste();
          this.adicionarEventoMeusDados();
          this.adicionarEventoPerfilTrocaSenha();
        });
      }
    });
  }

  /**
   * Adiciona eventos de click nos botões do menu de Usuário e Acessibilidade.
   * O evento de click no botão do menu de Usuário:
   * - adiciona a classe 'open' ao elemento pai do botão;
   * - remove a classe 'open' do elemento pai do botão de Acessibilidade.
   * O evento de click no botão do menu de Acessibilidade:
   * - adiciona a classe 'open' ao elemento pai do botão;
   * - remove a classe 'open' do elemento pai do botão de Usuário.
   * Além disso, remove a classe 'open' de todos os elementos no documento quando
   * o usuário clica em qualquer lugar fora do menu.
   */
  adicionarClickNoMenuBotao() {
    const userMenuDiv = document.querySelector('.user.user-menu.dropdown');
    const toggleButtonProfile = userMenuDiv?.querySelector(
      '.dropdown-toggle.smc-btn-usuario',
    );
    const usabilidadeMenuDiv = document.querySelector(
      '.smc-acessibilidade.dropdown',
    );
    const toggleButtonUsabilidade = usabilidadeMenuDiv?.querySelector(
      '.dropdown-toggle.smc-btn-acessibilidade',
    );

    if (toggleButtonProfile && userMenuDiv) {
      toggleButtonProfile.addEventListener('click', (event) => {
        event.stopPropagation(); // Evita que o clique no botão remova a classe
        userMenuDiv.classList.toggle('open');
        usabilidadeMenuDiv?.classList.remove('open');
      });

      // Remove a classe 'open' ao clicar em qualquer outra parte do documento
      document.addEventListener('click', () => {
        if (userMenuDiv.classList.contains('open')) {
          userMenuDiv.classList.remove('open');
        }
      });
    }

    if (toggleButtonUsabilidade && usabilidadeMenuDiv) {
      toggleButtonUsabilidade.addEventListener('click', (event) => {
        event.stopPropagation(); // Evita que o clique no botão remova a classe
        usabilidadeMenuDiv.classList.toggle('open');
        userMenuDiv?.classList.remove('open');
      });

      // Remove a classe 'open' ao clicar em qualquer outra parte do documento
      document.addEventListener('click', () => {
        if (usabilidadeMenuDiv.classList.contains('open')) {
          usabilidadeMenuDiv.classList.remove('open');
        }
      });
    }
  }

  /**
   * Adiciona eventos de click nos botões de Acessibilidade.
   * Aumenta ou diminui o tamanho da fonte do conteúdo da página,
   * alterna entre o tema de cores original e o tema de cores de alto contraste.
   * Os eventos de click nos botões de Acessibilidade:
   * - adicionam ou removem a classe 'smc-acessibilidade-1' ou 'smc-acessibilidade1'
   *   para alterar o tamanho da fonte;
   * - adicionam ou removem a classe 'smc-acessibilidade-contraste' para alterar o tema de cores.
   */
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

  adicionarEventoMeusDados() {
    const btnMeusDados = document.querySelector(
      '.smc-btn-custom.smc-btn-meudados',
    );
    if (btnMeusDados) {
      btnMeusDados.addEventListener('click', () => {
        //this.dataServiceNavegacao.desativarTodosBotoes$.next(true);
        this.layoutProcessoDataService.setDesativarTodosBotoes(true);
        //this.serviceSelecaoVazio.desabilitarBotaoSelecionar$.next(true);
        this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
          true,
        );
      });
    }
  }

  adicionarEventoPerfilTrocaSenha() {
    const btnMenuPerfil = document.querySelector(
      '[data-behavior="show-profile"]',
    );
    const btnTrocarSenha = document.querySelector(
      '[data-behavior="change-password"',
    );

    if (btnMenuPerfil) {
      btnMenuPerfil.addEventListener('click', () => {
        //this.dataServiceNavegacao.desativarTodosBotoes$.next(true);
        this.layoutProcessoDataService.setDesativarTodosBotoes(true);
        this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
          true,
        );
      });
    }

    if (btnTrocarSenha) {
      btnTrocarSenha.addEventListener('click', () => {
        //this.dataServiceNavegacao.desativarTodosBotoes$.next(true);
        this.layoutProcessoDataService.setDesativarTodosBotoes(true);
        //this.serviceSelecaoVazio.desabilitarBotaoSelecionar$.next(true);
        this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
          true,
        );
      });
    }
  }
}
