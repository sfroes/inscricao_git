import { Injectable, signal } from '@angular/core';
import { HierarquiaModel } from '../model/herarquia.model';
import { ListaOfertaModel } from '../model/lista-oferta.model';
import {
  Ofertas,
  SelecaoOfertaModel,
  Taxa,
} from '../model/selecao-oferta.model';

@Injectable({
  providedIn: 'root',
})
export class SelecaoOfertaDataService {
  totalGeralInDB: number = 0;
  private _ofertaSelecionadas = signal<Ofertas[]>([]);
  $ofertasSelecionadas = this._ofertaSelecionadas.asReadonly();
  setOfertasSelecionadas(ofertas: Ofertas[]) {
    this._ofertaSelecionadas.set(ofertas);
  }
  addOfertaSelecioanda(oferta: Ofertas) {
    this._ofertaSelecionadas.update((ofertas) => [...ofertas, oferta]);
  }
  private _selecaoOferta = signal<SelecaoOfertaModel>({} as SelecaoOfertaModel);
  $selecaoOferta = this._selecaoOferta.asReadonly();
  setSelecaoOferta(selecaoOferta: SelecaoOfertaModel) {
    this._selecaoOferta.set(selecaoOferta);
  }
  updateOfertasSelecaoOferta(data: Ofertas[]) {
    this._selecaoOferta.update((u) => ({
      // Retorna um novo objeto
      ...u,
      ofertas: data,
    }));
  }
  updateTotalGeralSelecaoOferta(totalGeral: number) {
    this._selecaoOferta.update((u) => {
      return {
        ...u,
        totalGeral: totalGeral,
      };
    });
  }

  public updateItemTaxaNaSelecaoOferta(
    seqTaxaAlvo: number,
    seqOferta: number,
    novoNumeroItens: number | null,
  ): void {
    this._selecaoOferta.update((currentModel) => {
      // Se o modelo atual ou o array de taxas não existir, retorna o modelo atual sem alteração
      // ou um novo modelo vazio, dependendo da sua lógica de estado inicial.
      if (!currentModel || !Array.isArray(currentModel.taxas)) {
        console.warn(
          'Tentativa de atualizar item de taxa, mas SelecaoOfertaModel ou sua propriedade .taxas é inválida ou não existe.',
          currentModel,
        );
        return currentModel; // Ou return { ...currentModel, taxas: [] } se preferir garantir que 'taxas' seja um array
      }

      // Cria um novo array 'taxas' mapeando sobre o array antigo
      const novoArrayDeTaxas = currentModel.taxas.map((taxaItem: Taxa) => {
        if (
          taxaItem.seqTaxa === seqTaxaAlvo &&
          taxaItem.seqOferta === seqOferta
        ) {
          // Encontrou a taxa a ser atualizada. Retorna um NOVO objeto Taxa com os valores modificados.
          return {
            ...taxaItem, // Copia todas as propriedades existentes da taxaItem
            numeroItens: novoNumeroItens,
            valorTotalTaxa: (novoNumeroItens ?? 0) * taxaItem.valorItem,
            // Certifique-se de que o tipo 'Taxa' realmente tem 'numeroItens' e 'valorTotalTaxa' como propriedades atualizáveis.
          };
        }
        // Se não for a taxa a ser atualizada, retorna o objeto taxaItem original (sem modificação)
        return taxaItem;
      });

      // Retorna um NOVO objeto SelecaoOfertaModel, contendo todas as propriedades do modelo original,
      // mas com o array 'taxas' substituído pelo 'novoArrayDeTaxas'.
      return {
        ...currentModel,
        taxas: novoArrayDeTaxas,
      };
    });
  }

  public updateTaxasSelecaoOferta(taxa: Taxa[]) {
    this._selecaoOferta.update((u) => {
      u.taxas.push(...taxa);
      return u;
    });
  }

  private _taxasPorGrupo = signal<Taxa[]>([]);
  $taxasPorGrupo = this._taxasPorGrupo.asReadonly();

  updateTaxasPorGrupo(data: Taxa[]) {
    this._taxasPorGrupo.update((taxa) => {
      taxa = data;
      return taxa;
    });
  }

  private _botaoCancelarHierarquia = signal<boolean>(false);
  $botaoCancelarHierarquia = this._botaoCancelarHierarquia.asReadonly();
  setBotaoCancelarHierarquia(ind: boolean) {
    this._botaoCancelarHierarquia.set(ind);
  }
  private _listaOfertas = signal<ListaOfertaModel[]>([]);
  $listaOfertas = this._listaOfertas.asReadonly();
  setListaOfertas(lista: ListaOfertaModel[]) {
    this._listaOfertas.set(lista);
  }
  updateListaOfertas(lista: ListaOfertaModel[]) {
    this._listaOfertas.update(() => [...lista]);
  }
  private _carregarListaOferta = signal<boolean>(false);
  $carregarListaOferta = this._carregarListaOferta.asReadonly();
  setCarregarListaOferta(ind: boolean) {
    this._carregarListaOferta.set(ind);
  }

  private _numeroNoFolha = signal<number>(0);
  $numeroNoFolha = this._numeroNoFolha.asReadonly();
  setNumeroNoFolha(ind: number) {
    this._numeroNoFolha.set(ind);
  }

  private _ativarBotaoSelecionaModalHierarquia = signal<boolean>(true);
  $ativarBotaoSelecionaModalHierarquia =
    this._ativarBotaoSelecionaModalHierarquia.asReadonly();
  setAtivarBotaoSelecionaModalHierarquia(ind: boolean) {
    this._ativarBotaoSelecionaModalHierarquia.set(ind);
  }

  private _dispositivoEhMobile = signal<boolean>(false);
  $dispositivoEhMobile = this._dispositivoEhMobile.asReadonly();
  setDispositivoEhMobile(ind: boolean) {
    this._dispositivoEhMobile.set(ind);
  }

  private _ordernarLista = signal<boolean>(false);
  $ordernarLista = this._ordernarLista.asReadonly();
  setOrdernarLista(ind: boolean) {
    this._ordernarLista.set(ind);
  }

  private _desativarBotaoSelecionarOfertas = signal<boolean>(false);
  $desativarBotaoSelecionarOfertas =
    this._desativarBotaoSelecionarOfertas.asReadonly();
  setDesativarBotaoSelecionaModalHierarquia(ind: boolean) {
    this._desativarBotaoSelecionarOfertas.set(ind);
  }

  private _abrirModalSelecaoOferta = signal<boolean>(false);
  $abrirModalSelecaoOferta = this._abrirModalSelecaoOferta.asReadonly();
  setAbrirModalSelecaoOferta(data: boolean) {
    this._abrirModalSelecaoOferta.set(data);
  }
  private _exibirMensagemBoletoPago = signal<boolean>(false);

  $exibirMensagemBoletoPago = this._exibirMensagemBoletoPago.asReadonly();
  setExibirMensagemBoletoPago(data: boolean) {
    this._exibirMensagemBoletoPago.set(data);
  }

  private _desabilitarBotaoAlterarHierarquia = signal(false);
  $desabilitarBotaoAlterarHierarquia =
    this._desabilitarBotaoAlterarHierarquia.asReadonly();
  setDeabilitarBotaoAlterarHieraquia(visivel: boolean) {
    this._desabilitarBotaoAlterarHierarquia.set(visivel);
  }

  private _hierarquia = signal<HierarquiaModel[]>([]);
  $hierarquia = this._hierarquia.asReadonly();
  setHierarquia(data: HierarquiaModel[]) {
    this._hierarquia.set(data);
  }

  botaoSelecionarOfertaAcionado: boolean = false;
}
