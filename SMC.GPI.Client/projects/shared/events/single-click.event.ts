import { Directive, HostListener, ElementRef, Renderer2, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[pInputNumberSingleClick]',
  standalone: true,
})
export class SingleClickEvent implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    // Acessa os botões após o componente ser renderizado
    const buttons = this.el.nativeElement.querySelectorAll('.p-inputnumber-button');
    buttons.forEach((button: HTMLElement) => {
      this.renderer.listen(button, 'mousedown', this.onPress.bind(this));
    //  this.renderer.listen(button, 'touchstart', this.onPress.bind(this));
    });
  }

  private onPress(ev: Event) {
    // 1. Impede que o evento padrão de "pressão contínua" do PrimeNG seja acionado.
    ev.preventDefault();
    ev.stopImmediatePropagation();

    const targetButton = ev.currentTarget as HTMLElement;

    // 2. Dispara um clique programático para executar a ação de incremento/decremento uma vez.
    targetButton.click();

    /* 3. Despacha um evento 'mouseup' ou 'touchend' para garantir que o componente
          entenda que a ação de "pressionar" terminou. */
    queueMicrotask(() => {
      const eventType = ev.type === 'mousedown' ? 'mouseup' : 'touchend';
      targetButton.dispatchEvent(
        new MouseEvent(eventType, {
          bubbles: true,
          cancelable: true,
          view: window,
        })
      );
    });
  }
}