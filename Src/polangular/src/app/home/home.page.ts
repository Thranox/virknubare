import { Component, OnInit } from '@angular/core';
import { MenuController } from '@ionic/angular';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class HomePage implements OnInit {
    
    toolbarOptions =
        [
            {
                tab: 'Rejseafgifter',
                route: 'travel-expenses',
                icon: 'person'
            },
            {
                tab: 'Indberetninger',
                route: 'reports',
                icon: 'newspaper-outline'
            },
            {
                tab: 'Arkiver',
                route: 'archives',
                icon: 'folder-open-outline'
            },
            {
                tab: 'Hjælp',
                route: 'help',
                icon: 'help-circle-outline'
            },
        ]
    constructor() {
    }

    ngOnInit() {
    
    }
}
