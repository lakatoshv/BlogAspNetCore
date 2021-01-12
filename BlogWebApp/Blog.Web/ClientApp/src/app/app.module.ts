import { ErrorsModule } from './shared/errors/errors.module';
import { CustomToastrService } from './core/services/custom-toastr.service';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { GlobalService } from './core/services/global-service/global-service.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    HttpModule,
    ToastrModule.forRoot(),
    ErrorsModule
  ],
  bootstrap: [AppComponent],
  providers: [
    CustomToastrService,
    GlobalService
  ]
})
export class AppModule { }

