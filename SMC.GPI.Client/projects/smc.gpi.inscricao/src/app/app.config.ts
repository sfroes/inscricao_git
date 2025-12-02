import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { APP_BASE_HREF } from '@angular/common';
import {
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import {
  BrowserAnimationsModule,
  provideAnimations,
} from '@angular/platform-browser/animations';
import Aura from '@primeng/themes/aura';
import { MessageService } from 'primeng/api';
import { providePrimeNG } from 'primeng/config';
import { routes } from './app.routes';
import { environment } from './core/environments/environment';
import { errorInterceptor } from './core/inteceptors/ErrosInterceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: APP_BASE_HREF, useValue: environment.base_href },
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          prefix: 'p',
          darkModeSelector: false,
          cssLayer: false,
        },
      },
    }),
    provideHttpClient(withInterceptors([errorInterceptor])),
    { provide: MessageService, useClass: MessageService },
    provideAnimations(),
    importProvidersFrom(BrowserModule, BrowserAnimationsModule),
  ],
};
