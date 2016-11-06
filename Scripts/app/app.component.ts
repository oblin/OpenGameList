import { Component } from '@angular/core';
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
export class AppComponent {
    title = 'OpenGameList';

    constructor(private router: Router, private authService: AuthService) { }

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
        if (this.authService.logout()) {
            this.router.navigate(['']);
        }
        return false;
    }
}
