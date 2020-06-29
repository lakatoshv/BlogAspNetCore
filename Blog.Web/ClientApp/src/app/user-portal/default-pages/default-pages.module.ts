import { MessagesService } from 'src/app/core/services/messages-service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DefaultPagesRoutingModule } from './default-pages-routing.module';
import { AboutComponent } from './about/about.component';
import { ContactsComponent } from './contacts/contacts.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    DefaultPagesRoutingModule
  ],
  providers: [MessagesService]
})
export class DefaultPagesModule { }
