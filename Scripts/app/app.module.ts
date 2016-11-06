/// <reference path="../../typings/index.d.ts" />
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import 'rxjs/Rx';

import { ItemService } from './item.service';
import { AuthService } from './auth.service';
import { AuthHttp } from './auth.http';

import { AppComponent } from './app.component';
import { ItemListComponent } from './item-list.component';
import { ItemDetailEditComponent } from './item-detail-edit.component';
import { ItemDetailViewComponent } from './item-detail-view.component';
import { AboutComponent } from './about.component';
import { HomeComponent } from './home.component';
import { LoginComponent } from './login.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { AppRouting } from './app.routing';

@NgModule({
    declarations: [
        AppComponent,
        ItemListComponent,
        ItemDetailEditComponent,
        ItemDetailViewComponent,
        HomeComponent,
        LoginComponent,
        PageNotFoundComponent,
        AboutComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule, ReactiveFormsModule,
        RouterModule,
        AppRouting
    ],
    providers: [ItemService, AuthService, AuthHttp],
    bootstrap: [AppComponent]
})
export class AppModule { }
