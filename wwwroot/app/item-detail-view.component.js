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
var item_service_1 = require('./item.service');
var auth_service_1 = require('./auth.service');
var ItemDetailViewComponent = (function () {
    function ItemDetailViewComponent(itemService, authService, router, activatedRoute) {
        this.itemService = itemService;
        this.authService = authService;
        this.router = router;
        this.activatedRoute = activatedRoute;
    }
    ItemDetailViewComponent.prototype.ngOnInit = function () {
        var _this = this;
        var id = +this.activatedRoute.snapshot.params['id'];
        if (id) {
            this.itemService.get(id).subscribe(function (item) { return _this.item = item; });
        }
        else if (id === 0) {
            console.log('id is 0: adding a new item...');
            this.router.navigate(['item/edit', 0]);
        }
        else {
            console.log('Invalid id: routing back to home...');
            this.router.navigate(['']);
        }
    };
    ItemDetailViewComponent.prototype.onItemDetailEdit = function (item) {
        this.router.navigate(['item/edit', item.Id]);
        return false;
    };
    ItemDetailViewComponent.prototype.onBack = function () {
        this.router.navigate(['']);
    };
    ItemDetailViewComponent = __decorate([
        core_1.Component({
            selector: 'item-detail-view',
            templateUrl: './template/item-detail-view.component.html',
            styleUrls: ['./css/item-detail.component.css']
        }), 
        __metadata('design:paramtypes', [item_service_1.ItemService, auth_service_1.AuthService, router_1.Router, router_1.ActivatedRoute])
    ], ItemDetailViewComponent);
    return ItemDetailViewComponent;
}());
exports.ItemDetailViewComponent = ItemDetailViewComponent;

//# sourceMappingURL=item-detail-view.component.js.map
