import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthService {
    private readonly authKey = 'auth';       // key for local storage

    constructor(private http: Http) { }

    login(username: string, password: string): Observable<any> {
        let data = {
            username: username,
            password: password,
            client_id: 'OpenGameList',
            // required when signing up with username/password
            grant_type: 'password',
            // space-separated list of scopes for which the token is issued
            scope: 'offline_access profile email'
        };

        return this.postToAuthServer(data);

        // return this.http
        //     .post(
        //     this.tokenUrl,
        //     this.toUrlEncodedString(data),
        //     new RequestOptions({
        //         headers: new Headers({
        //             'Content-Type': 'application/x-www-form-urlencoded'
        //         })
        //     }))
        //     .map(response => {
        //         let auth = response.json();
        //         console.log(`The following auth JSON object received: ${auth}`);
        //         this.setAuth(auth);
        //         return auth;
        //     });
    }

    logout(): boolean {
        this.setAuth(null);
        return false;
    }

    /**
     * Converts a JSON object to urlencoded format
     * @param {*} data
     * @returns {string}
     */
    toUrlEncodedString(data: any): string {
        let body = '';
        console.log('toUrlEncodedString data: ' + JSON.stringify(data));
        for (let key in data) {
            if (data.hasOwnProperty(key)) {
                if (body.length) {
                    body += '&';
                }
                body += key + '=';
                body += encodeURIComponent(data[key]);
            }
        }
        console.log('toUrlEncodedString body: ' + body);
        return body;
    }

    /**
     * Persist auth into local storage or removes it if a null argument is given
     * @param {*} auth
     * @returns {boolean}
     */
    setAuth(auth: any): boolean {
        if (auth) {
            localStorage.setItem(this.authKey, JSON.stringify(auth));
        } else {
            localStorage.removeItem(this.authKey);
        }
        return true;
    }

    getAuth(): any {
        let auth = localStorage.getItem(this.authKey);
        if (auth) {
            return JSON.parse(auth);
        } else {
            return null;
        }
    }

    /**
     * Return TRUE if the user is logged in
     * @returns {boolean}
     */
    isLoggedIn(): boolean {
        return localStorage.getItem(this.authKey) != null;
    }

    refreshToken(): Observable<any> {
        let auth = this.getAuth();
        console.log('Invoke refreshToken, token expires_in: ' + auth.expires_in);
        let data = {
            client_id: 'OpenGameList',
            refresh_token: auth.refresh_token,
            grant_type: 'refresh_token',
            scope: 'offline_access profile email'
        };

        return this.postToAuthServer(data);
    }

    private postToAuthServer(data: any): Observable<any> {
        let tokenUrl = 'api/connect/token';  // JwtProvider's Login path

        return this.http
            .post(
            tokenUrl,
            this.toUrlEncodedString(data),
            new RequestOptions({
                headers: new Headers({
                    'Content-Type': 'application/x-www-form-urlencoded'
                })
            }))
            .map(response => {
                let auth = response.json();
                console.log(`The following auth JSON object received: ${auth}`);
                this.setAuth(auth);
                return auth;
            });
    }
}
