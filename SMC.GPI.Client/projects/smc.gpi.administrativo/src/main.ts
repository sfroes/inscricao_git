import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { APP_BASE_HREF } from '@angular/common';

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
