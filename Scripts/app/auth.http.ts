import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';

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
}
