import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToolsService {
  /**
   * Decodifica uma string HTML usando um elemento textarea para analisar
   * a string e retornar o valor da string decodificada.
   * @param encodedStr a string HTML a ser decodificada
   * @returns a string decodificada
   */
  decodeHtml(encodedStr: string): string {
    const textarea = document.createElement('textarea');
    textarea.innerHTML = encodedStr;
    return textarea.value;
  }

  /**
   * Verifica se o GUID fornecido é válido, não está vazio e não é uma sequência de zeros.
   * @param guid - A string que representa o GUID a ser validado.
   * @returns Verdadeiro se o GUID for válido, não estiver vazio e não for uma sequência de zeros, caso contrário, falso.
   */
  isGuidValido(guid: string): boolean {
    // Expressão regular para um GUID no formato XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
    const guidRegex =
      /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;

    // GUID que representa a sequência de zeros
    const emptyGuid = '00000000-0000-0000-0000-000000000000';

    // Verifica se o GUID está no formato correto e não é uma sequência de zeros
    return guidRegex.test(guid) && guid !== emptyGuid;
  }

  currencyMask(value: number | null | undefined): string {
    // Added null/undefined handling for robustness
    if (value === null || value === undefined) {
      return 'R$ 0,00'; // Or 'R$ -' as per your display preference for invalid/missing values
    }

    // Create a number formatter for Brazilian Real (BRL)
    const formatter = new Intl.NumberFormat('pt-BR', {
      style: 'currency', // Specifies currency formatting
      currency: 'BRL', // Sets the currency to Brazilian Real
      minimumFractionDigits: 2, // Ensures at least two decimal places
      maximumFractionDigits: 2, // Ensures exactly two decimal places
    });

    return formatter.format(value);
  }
}
