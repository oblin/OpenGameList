import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Item } from './item';
import { ItemService } from './item.service';
import { Message } from 'primeng/primeng';

@Component({
    selector: 'item-detail-view',
    templateUrl: './template/item-detail-view.component.html',
    styleUrls: ['./css/item-detail.component.css']
})
export class ItemDetailViewComponent implements OnInit {
    private messages: Message[];
    private userform: FormGroup;
    item: Item;
    constructor(private itemService: ItemService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder) { }

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
        // Build up form's controls
        this.userform = this.fb.group({
            title: ['', Validators.required],
            // desc: ['', Validators.compose([Validators.required, Validators.minLength(10)])]
            desc: ['', Validators.required]
        });
    }

    onItemDetailEdit(item: Item) {
        console.log('Click Edit tab...');
        this.router.navigate(['item/edit', item.Id]);
    }

    onTabChanged(event) {
        console.log(`event index: ${event.index}`);
        this.messages = [];
        this.messages.push({
            severity: 'warn', summary: 'Tab Clicked!', detail: 'Click Index is ' + event.index
        });
    }
}
