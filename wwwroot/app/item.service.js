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
var Observable_1 = require('rxjs/Observable');
var auth_http_1 = require('./auth.http');
var ItemService = (function () {
    function ItemService(http) {
        this.http = http;
        this.baseUrl = 'api/items/';
    }
    ItemService.prototype.getLastest = function (num) {
        var url = this.baseUrl + 'GetLastest/';
        if (num != null) {
            url += num;
        }
        return this.http.get(url)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ItemService.prototype.getMostViewed = function (num) {
        var url = this.baseUrl + 'GetMostViewed/';
        if (num != null) {
            url += num;
        }
        return this.http.get(url)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    ItemService.prototype.getRandom = function (num) {
        var url = this.baseUrl + 'GetRandom/';
        if (num != null) {
            url += num;
        }
        return this.http.get(url)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    // calls the [GET] /api/items/{id} Web API method to retrieve the item with the given id.
    ItemService.prototype.get = function (id) {
        if (id == null) {
            throw new Error('id is required.');
        }
        var url = this.baseUrl + id;
        return this.http.get(url)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    /**
     * calls the [POST] /api/items/ Web API method to add a new item.
     * @param {Item} item
     * @returns {Observable<Item>} 傳回新增成功的 Item
     */
    ItemService.prototype.add = function (item) {
        var url = this.baseUrl;
        return this.http.post(url, JSON.stringify(item), this.http.RequestOptionJsonType)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    // calls the [PUT] /api/items/ Web API method to update a existing item.
    ItemService.prototype.update = function (item) {
        var url = this.baseUrl + item.Id;
        return this.http.put(url, JSON.stringify(item), this.http.RequestOptionJsonType)
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    /**
     * calls the [DELETE] /api/items/{id} Web API method to delete the item with the given id.
     * @param {number} id
     * @returns
     */
    ItemService.prototype.delete = function (id) {
        var url = this.baseUrl + id;
        return this.http.delete(url)
            .catch(this.handleError);
    };
    /**
     * 處理錯誤訊息，主要在 console.log 中紀錄
     * @param {Response} error
     * @returns Observable.throw error message
     */
    ItemService.prototype.handleError = function (error) {
        console.error(error);
        return Observable_1.Observable.throw(error.json().error || 'Server error');
    };
    ItemService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [auth_http_1.AuthHttp])
    ], ItemService);
    return ItemService;
}());
exports.ItemService = ItemService;

//# sourceMappingURL=item.service.js.map
