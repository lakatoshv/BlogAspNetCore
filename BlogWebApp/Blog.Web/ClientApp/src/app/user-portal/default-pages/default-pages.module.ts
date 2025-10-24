import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultPagesRoutingModule } from './default-pages-routing.module';
import { AboutComponent } from './about/about.component';
import { ContactsComponent } from './contacts/contacts.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    DefaultPagesRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  declarations: [AboutComponent, ContactsComponent]
})
export class DefaultPagesModule { }
