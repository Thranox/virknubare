import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../core/services/auth.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
    user: Observable<any>;

    constructor(private authService: AuthService) {
    }

    ngOnInit(): void {
        this.user = this.authService.getUser();
    }

}
