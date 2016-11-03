import { Component } from '@angular/core';

@Component({
    selector: 'home',
    template: `
        <h2>Angular2 & WebAip 範例！</h2>
        <div class="col-md-4">
            <item-list class="latest"></item-list>
        </div>
        <div class="col-md-4">
            <item-list class="most-viewed"></item-list>
        </div>
        <div class="col-md-4">
            <item-list class="random"></item-list>
        </div>
    `,
    styles: []
})
export class HomeComponent {
}
