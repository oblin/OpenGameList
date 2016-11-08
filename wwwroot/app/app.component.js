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
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var auth_service_1 = require('./auth.service');
var AppComponent = (function () {
    function AppComponent(router, authService) {
        this.router = router;
        this.authService = authService;
        this.title = 'OpenGameList';
    }
    /**
     * 判斷傳入的資料組合是否是目前的 url
     * @param {any[]} ex: ['item/edit', 0], ['login']
     */
    AppComponent.prototype.isActive = function (link) {
        // 利用 router.isActive(url) 判斷是否是目前的 url 
        return this.router.isActive(
        // 使用 ['item/edit', 0] 組合成目前的 url
        this.router.createUrlTree(link), true);
    };
    AppComponent.prototype.logout = function () {
        if (this.authService.logout()) {
            this.router.navigate(['']);
        }
        return false;
    };
    AppComponent.prototype.ngOnInit = function () {
        console.log('App Component is start....');
        if (this.authService.getAuth()) {
            this.authService.startupTokenRefresh();
        }
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: 'opengamelist',
            templateUrl: './template/app.component.html'
        }), 
        __metadata('design:paramtypes', [router_1.Router, auth_service_1.AuthService])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;

//# sourceMappingURL=app.component.js.map
