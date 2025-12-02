import { GPILookup } from '../../../../core/models/GpiLookup';
import { TipoCobranca } from './enums/tipo-cobranca.enum';

export class SelecaoOfertaModel {
  alertaOferta: any = null;
  aptoBolsa: boolean = false;
  bolsaExAluno: boolean = false;
  chaveTextoBotaoAnterior: string = '';
  chaveTextoBotaoProximo: string = '';
  cobrancaPorOferta: boolean = false;
  descricaoGrupoOferta: any = null;
  descricaoOfertas: number = 0;
  descricaoProcesso: string = '';
  descricaoSituacaoAtual: string = '';
  exibeTermoOrientacaoPDF: boolean = false;
  exigeJustificativaOferta: boolean = false;
  fluxoPaginas: FluxoPaginas[] = [];
  gestaoEventos: boolean = true;
  gruposTaxa: GrupoTaxa[] = [];
  habilitaCheckin: boolean = true;
  idioma: string = '';
  imagemCabecalho: any = null;
  inscricaoIniciada: boolean = true;
  labelCodigoAutorizacao: any = null;
  labelGrupoOferta: string = '';
  labelOferta: string = '';
  navigationGroup: any = null;
  numeroMaximoConvocacaoPorInscricao: any = null;
  numeroMaximoOfertaPorInscricao: any = null;
  numeroOpcoesDesejadas: number | null = null;
  numeroPaginaAtual: number = 0;
  ofertas: Ofertas[] = [];
  opcoesParaConvocacao: number = 0;
  ordem: number = 0;
  orientacaoAceiteConversaoArquivosPDF: any = null;
  possuiBoletoPago: boolean = false;
  secoes: any;
  seqConfiguracaoEtapa: number = 0;
  seqConfiguracaoEtapaEncrypted: string = '';
  seqConfiguracaoEtapaPagina: number = 0;
  seqConfiguracaoEtapaPaginaAnterior: number = 0;
  seqConfiguracaoEtapaPaginaAnteriorEncrypted: string = '';
  seqConfiguracaoEtapaPaginaEncrypted: string = '';
  seqConfiguracaoEtapaPaginaProxima: number = 0;
  seqConfiguracaoEtapaPaginaProximaEncrypted: string = '';
  seqGrupoOferta: number = 0;
  seqGrupoOfertaEncrypted: string = '';
  seqInscricao: number = 0;
  seqInscricaoEncrypted: string = '';
  seqProcesso: number = 0;
  taxas: Taxa[] = [];
  termoAceiteConversaoArquivosPDF: any = null;
  titulo: string = '0';
  tituloPago: boolean = false;
  token: string = '';
  tokenPaginaAnterior: string = '';
  tokenPaginaAnteriorEncrypted: string = '';
  tokenProximaPagina: string = '';
  tokenProximaPaginaEncrypted: string = '';
  tokenResource: string = '';
  tokenSituacaoAtual: string = '';
  totalGeral: number = 0;
  uidProcesso: string = '';
  urlCss: string = '';
  _seqConfiguracaoEtapaPaginaProxima: number = 0;
  _tokenProximaPagina: string = '';
  exibirArvoreFechada: boolean = false;
  tipoCobrancaPorQuantidadeOferta: boolean = false;
  instrucaoTaxa: any;
  processoPossuiTaxa: boolean = false;
  permiteAlterarBoleto: boolean = false;
}

export class FluxoPaginas {
  SeqPaginaIdioma: number = 0;
  ExibeConfirmacaoInscricao: boolean = false;
  ExibeComprovanteInscricao: boolean = false;
  Alerta: string = '';
  SeqConfiguracaoEtapaPagina: number = 0;
  Ordem: number = 0;
  Token: string = '';
  Titulo: string = '';
  SeqFormularioSGF: string = '';
  SeqVisaoSGF: string = '';
}

export class Ofertas {
  seq: number = 0;
  numeroOpcao: number = 0;
  seqOferta: GPILookup = {} as GPILookup;
  exibirMensagemOferta: boolean = false;
  mensagemOferta: string = '';
  justificativaInscricao: string = '';
  ativo: boolean = true;
  ofertaImpedida: boolean = true;
}

export class Taxa {
  maximoAtingido: boolean = false;
  minimoAtingido: boolean = false;
  quantidadeFaltante: number = 0;
  seqInscricaoBoleto: number | null = null;
  seqTaxa: number = 0;
  descricao: string = '';
  descricaoComplementar: string = '';
  descricaoDisplay: string = '';
  numeroItens: number | null = null;
  numeroMinimo: number | null = null;
  numeroMaximo: number | null = null;
  valorItem: number = 0;
  valorEventoTaxa: number = 0;
  valorTitulo: number | null = null;
  valorTotalTaxa: number = 0;
  cobrarPorOferta: boolean | null = null;
  tituloPago: boolean = false;
  tipoCobranca: TipoCobranca = 0;
  seqOferta: number = 0;
  possuiGrupoTaxas: boolean = false;
}

export class GrupoTaxa {
  seq: number = 0;
  seqProcesso: number = 0;
  descricao: string = '';
  numeroMinimoItens: number = 0;
  numeroMaximoItens: number | null = null;
  itens: GrupoTaxaItem[] = [];
}

export class GrupoTaxaItem {
  seq: number = 0;
  seqGrupoTaxa: number = 0;
  seqTaxa: number = 0;
  seqTipoTaxa: number = 0;
  descTipoTaxa: string = '';
  taxa: Taxa = {} as Taxa;
}

export class SelecaoOfertasSalvar {
  numeroMaximoOfertaPorInscricao: number | null = null;

  exigeJustificativaOferta: boolean = false;

  numeroMaximoConvocacaoPorInscricao: number | null = null;

  numeroOpcoesDesejadas: number | null = null;

  ofertas: Ofertas[] = [];

  taxas: any;
  seqConfiguracaoEtapaPagina: number = 0;
  seqConfiguracaoEtapaEncrypted: string = '';
  seqConfiguracaoEtapaPaginaAnterior: number = 0;
  seqConfiguracaoEtapaPaginaAnteriorEncrypted: string = '';
  seqConfiguracaoEtapaPaginaProxima: number = 0;
  seqConfiguracaoEtapaPaginaProximaEncrypted: string = '';
  seqGrupoOferta: number = 0;
  possuiBoletoPago: boolean = false;

  cobrancaPorOferta: boolean = false;

  bolsaExAluno: boolean = false;

  seqGrupoOfertaEncrypted: string = '';

  seqInscricao: number = 0;

  seqInscricaoEncrypted: string = '';

  seqProcesso: number = 0;
  idioma: string = '';

  token: string = '';

  tokenPaginaAnterior: string = '';

  tokenPaginaAnteriorEncrypted: string = '';

  tokenProximaPagina: string = '';

  tokenProximaPaginaEncrypted: string = '';

  tokenResource: string = '';

  tokenSituacaoAtual: string = '';

  permiteAlterarBoleto: boolean = false;
}
