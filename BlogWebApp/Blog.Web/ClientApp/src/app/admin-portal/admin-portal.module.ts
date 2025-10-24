import { DefaultPagesModule } from './default-pages/default-pages.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPortalRoutingModule } from './admin-portal-routing.module';
import { LayoutComponentComponent } from './layout-component/layout-component.component';
import { ErrorsModule } from '../shared/errors/errors.module';
import { PostsService } from '../core/services/posts-services/posts.service';
import { HttpClientService } from '../core/services/global-service/http-client-services/http-client.service';

@NgModule({
  imports: [
    CommonModule,
    AdminPortalRoutingModule,
    DefaultPagesModule,
    ErrorsModule,
  ],
  declarations: [LayoutComponentComponent],
  providers: [PostsService, HttpClientService]
})
export class AdminPortalModule { }
