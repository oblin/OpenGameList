"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
/// <reference path="../../typings/index.d.ts" />
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var http_1 = require('@angular/http');
var forms_1 = require('@angular/forms');
var router_1 = require('@angular/router');
require('rxjs/Rx');
var item_service_1 = require('./item.service');
var auth_service_1 = require('./auth.service');
var auth_http_1 = require('./auth.http');
var app_component_1 = require('./app.component');
var item_list_component_1 = require('./item-list.component');
var item_detail_edit_component_1 = require('./item-detail-edit.component');
var item_detail_view_component_1 = require('./item-detail-view.component');
var about_component_1 = require('./about.component');
var home_component_1 = require('./home.component');
var login_component_1 = require('./login.component');
var page_not_found_component_1 = require('./page-not-found.component');
var app_routing_1 = require('./app.routing');
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
                item_list_component_1.ItemListComponent,
                item_detail_edit_component_1.ItemDetailEditComponent,
                item_detail_view_component_1.ItemDetailViewComponent,
                home_component_1.HomeComponent,
                login_component_1.LoginComponent,
                page_not_found_component_1.PageNotFoundComponent,
                about_component_1.AboutComponent
            ],
            imports: [
                platform_browser_1.BrowserModule,
                http_1.HttpModule,
                forms_1.FormsModule, forms_1.ReactiveFormsModule,
                router_1.RouterModule,
                app_routing_1.AppRouting
            ],
            providers: [item_service_1.ItemService, auth_service_1.AuthService, auth_http_1.AuthHttp],
            bootstrap: [app_component_1.AppComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;

//# sourceMappingURL=app.module.js.map
