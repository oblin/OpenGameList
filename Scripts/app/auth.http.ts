import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

@Injectable()
export class AuthHttp {
    private readonly authKey = 'auth';       // key for local storage
    constructor(private http: Http) {}

    get(url, opts = {}) {
        this.configureAuth(opts);
        return this.http.get(url, opts);
    }

   post(url, data, opts = {}) {
        this.configureAuth(opts);
        return this.http.post(url, data, opts);
    }

    put(url, data, opts = {}) {
        this.configureAuth(opts);
        return this.http.put(url, data, opts);
    }

    delete(url, opts = {}) {
        this.configureAuth(opts);
        return this.http.delete(url, opts);
    }

    /**
     * 設定 WebApi 傳回的 Token，僅限定於 JWT 時候使用
     * 
     * @param {*} opts 可以傳入已經設定的 Headers，會再加上 Bearer token，如果沒有請使用 {} object
     */
    configureAuth(opts: any) {
        let token = localStorage.getItem(this.authKey);
        if (token != null) {
            let auth = JSON.parse(token);
            console.log(auth);
            if (auth.access_token != null) {
                if (opts.headers == null) {
                    opts.headers = new Headers();
                }
                opts.headers.set('Authorization', `Bearer ${auth.access_token}`);
            }
        }
    }

    /**
     * Generate request headers
     * @returns Headers: application/json
     */
    get RequestOptionJsonType() {
        return new RequestOptions({
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });
    }
}
