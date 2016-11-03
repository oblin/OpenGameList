import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { Item } from './item';

@Injectable()
export class ItemService {
    private baseUrl = 'api/items/';

    constructor(private http: Http) { }

    getLastest(num?: number): Observable<Item[]> {
        let url = this.baseUrl + 'GetLastest/';
        if (num != null) { url += num; }
        return this.http.get(url)
                   .map(response => <Item[]>response.json())
                   .catch(this.handleError);
    }

    getMostViewed(num?: number): Observable<Item[]> {
        let url = this.baseUrl + 'GetMostViewed/';
        if (num != null) { url += num; }
        return this.http.get(url)
                   .map(response => response.json())
                   .catch(this.handleError);
    }

    getRandom(num?: number): Observable<Item[]> {
        let url = this.baseUrl + 'GetRandom/';
        if (num != null) { url += num; }
        return this.http.get(url)
                   .map(response => response.json())
                   .catch(this.handleError);
    }

    // calls the [GET] /api/items/{id} Web API method to retrieve the item with the given id.
    get(id: number): Observable<Item> {
        if (id == null) { throw new Error('id is required.'); }
        let url = this.baseUrl + id;
        return this.http.get(url)
                   .map(response => <Item>response.json())
                   .catch(this.handleError);
    }

    /**
     * calls the [POST] /api/items/ Web API method to add a new item.
     * @param {Item} item
     * @returns {Observable<Item>} 傳回新增成功的 Item
     */
    add(item: Item): Observable<Item> {
        let url = this.baseUrl;
        return this.http.post(url, JSON.stringify(item),
                this.getRequestOptions())
            .map(response => <Item>response.json())
            .catch(this.handleError);
    }

    // calls the [PUT] /api/items/ Web API method to update a existing item.
    update(item: Item) {
        let url = this.baseUrl + item.Id;
        return this.http.put(url, JSON.stringify(item),
                this.getRequestOptions())
            .map(response => response.json())
            .catch(this.handleError);
    }

    /**
     * calls the [DELETE] /api/items/{id} Web API method to delete the item with the given id.
     * @param {number} id
     * @returns
     */
    delete(id: number) {
        let url = this.baseUrl + id;
        return this.http.delete(url)
            .catch(this.handleError);
    }

    /**
     * Generate request headers
     * @returns Headers: application/json
     */
    private getRequestOptions() {
        return new RequestOptions({
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });
    }

    /**
     * 處理錯誤訊息，主要在 console.log 中紀錄
     * @param {Response} error
     * @returns Observable.throw error message
     */
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}
