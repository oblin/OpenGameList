import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IntervalObservable } from 'rxjs/Observable/IntervalObservable';

@Injectable()
export class AuthService {
    private readonly authKey = 'auth';       // key for local storage
    private refreshSubscription: any;
    private notRefreshToken = true;

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

        return this.postToAuthServer(data, this.scheduleRefresh);
    }

    logout(): boolean {
        this.setAuth(null);
        this.unscheduleRefresh();
        return false;
    }

    /**
     * Converts a JSON object to urlencoded format
     * @param {*} data
     * @returns {string}
     */
    toUrlEncodedString(data: any): string {
        let body = '';
        // console.log('toUrlEncodedString data: ' + JSON.stringify(data));
        for (let key in data) {
            if (data.hasOwnProperty(key)) {
                if (body.length) {
                    body += '&';
                }
                body += key + '=';
                body += encodeURIComponent(data[key]);
            }
        }
        // console.log('toUrlEncodedString body: ' + body);
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

    /**
     * 當瀏覽器啟動時候，需要進行第一次的呼叫，讓瀏覽器可以開始設定到
     * IntervalObservable 中
     */
    startupTokenRefresh() {
        this.refreshTokenNow();
        this.scheduleRefresh();
    }

    /**
     * 排程下一次自動 refresh token 的時間 
     * 當從 server 取回 token 後（包含第一次 login & 之後的 refresh token）
     * @param {number} expiresIn 預計幾秒後就失效
     */
    scheduleRefresh() {
        let auth = this.getAuth();
        if (auth && this.notRefreshToken) {
            this.notRefreshToken = false;
            this.refreshSubscription = IntervalObservable.create(auth.expires_in * 1000);   // ms to second
            this.refreshSubscription.subscribe(() => {
                    console.log('Auto refresh token begin...');
                    this.refreshTokenNow();
                });
        }
    }

    unscheduleRefresh() {
        // Unsubscribe fromt the refresh
        if (this.refreshSubscription) {
            console.log('refreshSubscription.unsubscribe');
            this.refreshSubscription.unsubscribe();
        }
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

    /**
     * refreshToken with subscribe 讓他立刻執行 
     * @private
     */
    private refreshTokenNow(): void {
        this.refreshToken()
            .subscribe(token => {
                this.notRefreshToken = true;
                console.log('refresh result: ' + JSON.stringify(token))}
            );
    }

    private postToAuthServer(data: any, scheduleRefresh?: any): Observable<any> {
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
                // console.log(`The following auth JSON object received: ${auth}`);
                this.setAuth(auth);
                return auth;
            })
            .do((d) => { if (scheduleRefresh) { scheduleRefresh(); }});
    }
}
