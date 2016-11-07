import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/primeng';

@Component({
    selector: 'opengamelist',
    template: `
        <router-outlet></router-outlet>
    `
})
export class AppComponent implements OnInit {
    title = 'OpenGameList';

    constructor(private items: MenuItem[]) {}

    ngOnInit() {
        this.items = [
            { label: 'Home', routerLink: [''] },
            { label: 'About', routerLink: ['about'] },
            { label: 'Login', routerLink: ['login'] },
            { label: 'Add New', routerLink: ['item/edit', 0] }
        ];
    }
}
