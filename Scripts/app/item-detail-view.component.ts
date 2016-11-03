import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Item } from './item';
import { ItemService } from './item.service';

@Component({
    selector: 'item-detail-view',
    templateUrl: './app/item-detail-view.component.html',
    styleUrls: ['./app/item-detail.component.css']
})
export class ItemDetailViewComponent implements OnInit {
    item: Item;
    constructor(private itemService: ItemService,
        private router: Router,
        private activatedRoute: ActivatedRoute) { }

    ngOnInit() {
        let id = +this.activatedRoute.snapshot.params['id'];
        if (id) {
            this.itemService.get(id).subscribe(item => this.item = item);
        } else if (id === 0) {
            console.log('id is 0: adding a new item...');
            this.router.navigate(['item/edit', 0]);
        } else {
            console.log('Invalid id: routing back to home...');
            this.router.navigate(['']);
        }
    }

    onItemDetailEdit(item: Item) {
        this.router.navigate(['item/edit', item.Id]);
    }
}
