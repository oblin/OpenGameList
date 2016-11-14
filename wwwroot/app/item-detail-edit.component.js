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
var item_1 = require('./item');
var item_service_1 = require('./item.service');
var auth_service_1 = require('./auth.service');
var ItemDetailEditComponent = (function () {
    function ItemDetailEditComponent(itemService, authService, router, activatedRoute) {
        this.itemService = itemService;
        this.authService = authService;
        this.router = router;
        this.activatedRoute = activatedRoute;
    }
    ItemDetailEditComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['']);
        }
        var id = +this.activatedRoute.snapshot.params['id'];
        if (id) {
            this.itemService.get(id).subscribe(function (item) { return _this.item = item; });
        }
        else if (id === 0) {
            console.log('id is 0: adding a new item...');
            this.item = new item_1.Item(0, 'New Item', null);
        }
        else {
            console.log('Invalid id: routing back to home...');
            this.router.navigate(['']);
        }
    };
    ItemDetailEditComponent.prototype.onInsert = function (item) {
        var _this = this;
        this.itemService.add(item).subscribe(function (data) {
            _this.item = data;
            console.log("Item " + _this.item.Id + " has been added");
            _this.router.navigate(['']);
        }, function (error) { return console.log(error); });
    };
    ItemDetailEditComponent.prototype.onUpdate = function (item) {
        var _this = this;
        this.itemService.update(item).subscribe(function (data) {
            _this.item = data;
            console.log("Item " + _this.item.Id + " has been updated");
            _this.router.navigate(['item/view', _this.item.Id]);
        }, function (error) { return console.log(error); });
    };
    ItemDetailEditComponent.prototype.onDelete = function (item) {
        var _this = this;
        var id = item.Id;
        this.itemService.delete(id).subscribe(function (data) {
            console.log("Item " + _this.item.Id + " has been updated");
            _this.router.navigate(['']);
        }, function (error) { return console.log(error); });
    };
    /**
     * 處理 Token 逾期的時候，可以透過 refresh token 再次取得（不需要重新登入）
     *
     */
    ItemDetailEditComponent.prototype.onRefreshToken = function () {
        this.authService.refreshToken()
            .subscribe(function (token) { return console.log('refresh result: ' + JSON.stringify(token)); });
    };
    ItemDetailEditComponent.prototype.onBack = function () {
        this.router.navigate(['']);
    };
    ItemDetailEditComponent.prototype.onItemDetailView = function (item) {
        this.router.navigate(['item/view', item.Id]);
        // return false;
    };
    ItemDetailEditComponent.prototype.onGetUserInfo = function () {
        this.authService.get().subscribe(function (user) {
            alert('current user is ' + JSON.stringify(user));
        });
    };
    ItemDetailEditComponent = __decorate([
        core_1.Component({
            selector: 'item-detail-edit',
            templateUrl: './template/item-detail-edit.component.html',
            styleUrls: ['./css/item-detail.component.css']
        }), 
        __metadata('design:paramtypes', [item_service_1.ItemService, auth_service_1.AuthService, router_1.Router, router_1.ActivatedRoute])
    ], ItemDetailEditComponent);
    return ItemDetailEditComponent;
}());
exports.ItemDetailEditComponent = ItemDetailEditComponent;

//# sourceMappingURL=item-detail-edit.component.js.map
