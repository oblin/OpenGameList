System.register(["@angular/core","@angular/http","rxjs/Observable"],function(exports_1,context_1){"use strict";var core_1,http_1,Observable_1,ItemService,__decorate=(context_1&&context_1.id,this&&this.__decorate||function(decorators,target,key,desc){var d,c=arguments.length,r=c<3?target:null===desc?desc=Object.getOwnPropertyDescriptor(target,key):desc;if("object"==typeof Reflect&&"function"==typeof Reflect.decorate)r=Reflect.decorate(decorators,target,key,desc);else for(var i=decorators.length-1;i>=0;i--)(d=decorators[i])&&(r=(c<3?d(r):c>3?d(target,key,r):d(target,key))||r);return c>3&&r&&Object.defineProperty(target,key,r),r}),__metadata=this&&this.__metadata||function(k,v){if("object"==typeof Reflect&&"function"==typeof Reflect.metadata)return Reflect.metadata(k,v)};return{setters:[function(core_1_1){core_1=core_1_1},function(http_1_1){http_1=http_1_1},function(Observable_1_1){Observable_1=Observable_1_1}],execute:function(){ItemService=function(){function ItemService(http){this.http=http,this.baseUrl="api/items/"}return ItemService.prototype.getLastest=function(num){var url=this.baseUrl+"GetLastest/";return null!=num&&(url+=num),this.http.get(url).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.getMostViewed=function(num){var url=this.baseUrl+"GetMostViewed/";return null!=num&&(url+=num),this.http.get(url).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.getRandom=function(num){var url=this.baseUrl+"GetRandom/";return null!=num&&(url+=num),this.http.get(url).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.get=function(id){if(null==id)throw new Error("id is required.");var url=this.baseUrl+id;return this.http.get(url).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.add=function(item){var url=this.baseUrl;return this.http.post(url,JSON.stringify(item),this.getRequestOptions()).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.update=function(item){var url=this.baseUrl+item.Id;return this.http.put(url,JSON.stringify(item),this.getRequestOptions()).map(function(response){return response.json()}).catch(this.handleError)},ItemService.prototype.delete=function(id){var url=this.baseUrl+id;return this.http.delete(url).catch(this.handleError)},ItemService.prototype.getRequestOptions=function(){return new http_1.RequestOptions({headers:new http_1.Headers({"Content-Type":"application/json"})})},ItemService.prototype.handleError=function(error){return console.error(error),Observable_1.Observable.throw(error.json().error||"Server error")},ItemService=__decorate([core_1.Injectable(),__metadata("design:paramtypes",[http_1.Http])],ItemService)}(),exports_1("ItemService",ItemService)}}});
//# sourceMappingURL=item.service.js.map
