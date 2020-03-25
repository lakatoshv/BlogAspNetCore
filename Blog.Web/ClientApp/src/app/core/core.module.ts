import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GeneralServiceService } from './services/general-service.service';
@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    GeneralServiceService
  ]
})
export class CoreModule { }
