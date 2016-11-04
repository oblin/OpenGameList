import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';

@Component({
    selector: 'opengamelist',
    template: `
        <h1>{{ title }}</h1>
        <div class="menu">
            <a class="home" [routerLink]="['']">Home</a>
            | <a class="about" [routerLink]="['about']">About</a>
            | <a class="login" [routerLink]="['login']">Login</a>
            | <a class="add" [routerLink]="['item/edit', 0]">Add New</a>
        </div>
        <p-menubar [model]="items"></p-menubar>

        <router-outlet></router-outlet>
    `
})
export class AppComponent implements OnInit {
    title = 'OpenGameList';
    private items: MenuItem[];

    ngOnInit() {
        this.items = [
            { label: 'Home', routerLink: [''] },
            { label: 'About', routerLink: ['about'] },
            { label: '登入', routerLink: ['login'] }
        ];
    }
}
