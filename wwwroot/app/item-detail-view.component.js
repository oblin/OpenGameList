"use strict";var __decorate=this&&this.__decorate||function(decorators,target,key,desc){var d,c=arguments.length,r=c<3?target:null===desc?desc=Object.getOwnPropertyDescriptor(target,key):desc;if("object"==typeof Reflect&&"function"==typeof Reflect.decorate)r=Reflect.decorate(decorators,target,key,desc);else for(var i=decorators.length-1;i>=0;i--)(d=decorators[i])&&(r=(c<3?d(r):c>3?d(target,key,r):d(target,key))||r);return c>3&&r&&Object.defineProperty(target,key,r),r},__metadata=this&&this.__metadata||function(k,v){if("object"==typeof Reflect&&"function"==typeof Reflect.metadata)return Reflect.metadata(k,v)},core_1=require("@angular/core"),router_1=require("@angular/router"),item_service_1=require("./item.service"),ItemDetailViewComponent=function(){function ItemDetailViewComponent(itemService,router,activatedRoute){this.itemService=itemService,this.router=router,this.activatedRoute=activatedRoute}return ItemDetailViewComponent.prototype.ngOnInit=function(){var _this=this,id=+this.activatedRoute.snapshot.params.id;id?this.itemService.get(id).subscribe(function(item){return _this.item=item}):0===id?(console.log("id is 0: adding a new item..."),this.router.navigate(["item/edit",0])):(console.log("Invalid id: routing back to home..."),this.router.navigate([""]))},ItemDetailViewComponent.prototype.onItemDetailEdit=function(item){return this.router.navigate(["item/edit",item.Id]),!1},ItemDetailViewComponent.prototype.onBack=function(){this.router.navigate([""])},ItemDetailViewComponent=__decorate([core_1.Component({selector:"item-detail-view",templateUrl:"./template/item-detail-view.component.html",styleUrls:["./css/item-detail.component.css"]}),__metadata("design:paramtypes",[item_service_1.ItemService,router_1.Router,router_1.ActivatedRoute])],ItemDetailViewComponent)}();exports.ItemDetailViewComponent=ItemDetailViewComponent;
//# sourceMappingURL=item-detail-view.component.js.map
