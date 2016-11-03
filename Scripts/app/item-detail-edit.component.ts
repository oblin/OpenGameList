import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Item } from './item';
import { ItemService } from './item.service';

@Component({
    selector: 'item-detail-edit',
    templateUrl: './template/item-detail-edit.component.html',
    styleUrls: ['./css/item-detail.component.css']
})
export class ItemDetailEditComponent implements OnInit {
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
            this.item = new Item(0, 'New Item', null);
        } else {
            console.log('Invalid id: routing back to home...');
            this.router.navigate(['']);
        }
    }

    onInsert(item: Item) {
        this.itemService.add(item).subscribe(
            (data) => {
                this.item = data;
                console.log(`Item ${this.item.Id} has been added`);
                this.router.navigate(['']);
            },
            (error) => console.log(error)
        );
    }

    onUpdate(item: Item) {
        this.itemService.update(item).subscribe(
            (data) => {
                this.item = data;
                console.log(`Item ${this.item.Id} has been updated`);
                this.router.navigate(['item/view', this.item.Id]);
            },
            (error) => console.log(error)
        );
    }

    onDelete(item: Item) {
        let id = item.Id;
        this.itemService.delete(id).subscribe(
            (data) => {
                console.log(`Item ${this.item.Id} has been updated`);
                this.router.navigate(['']);
            },
            (error) => console.log(error)
        );
    }

    onBack() {
        this.router.navigate(['']);
    }

    onItemDetailView(item: Item) {
        this.router.navigate(['item/view', item.Id]);
        // return false;
    }
}
