import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

@Component({
    selector: 'opengamelist',
    templateUrl: './template/app.component.html'
    // 改用 templateUrl
    // template: `
    //     <h1>{{ title }}</h1>
    //     <div class="menu">
    //         <a class="home" [routerLink]="['']">Home</a>
    //         | <a class="about" [routerLink]="['about']">About</a>
    //         | <a class="login" [routerLink]="['login']">Login</a>
    //         | <a class="add" [routerLink]="['item/edit', 0]">Add New</a>
    //     </div>
    //     <router-outlet></router-outlet>
    // `
})
export class AppComponent implements OnInit {
    title = 'OpenGameList';

    constructor(private router: Router,
        private authService: AuthService,
        public zone: NgZone) {
        if (!(<any>window).externalProviderLogin) {
            let self = this;
            (<any>window).externalProviderLogin = function (auth) {
                self.zone.run(() => {
                    self.externalProviderLogin(auth);
                });
            }
        }
    }

    /**
     * 判斷傳入的資料組合是否是目前的 url
     * @param {any[]} ex: ['item/edit', 0], ['login'] 
     */
    isActive(link: any[]): boolean {
        // 利用 router.isActive(url) 判斷是否是目前的 url 
        return this.router.isActive(
            // 使用 ['item/edit', 0] 組合成目前的 url
            this.router.createUrlTree(link), true);
    }

    logout(): boolean {
        // logout current user, then redirect to welcome view
        this.authService.logout().subscribe(result => {
            if (result) {
                this.router.navigate(['']);
            }
        });
        return false;
    }

    externalProviderLogin(auth: any) {
        this.authService.setAuth(auth);
        console.log(`External Login successful! Provider: ${this.authService.getAuth().providerName}`);
        this.router.navigate(['']);
    }

    ngOnInit() {
        console.log('App Component is start....');
        if (this.authService.getAuth()) {
            this.authService.startupTokenRefresh();
        }
    }
}
