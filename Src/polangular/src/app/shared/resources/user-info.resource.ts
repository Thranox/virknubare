import {Injectable} from '@angular/core';
import {map} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable, of} from 'rxjs';
import {UserInfo} from '../model/user-info.model';

@Injectable()
export class UserInfoResource {

    private baseUrl: string;

    constructor(private httpClient: HttpClient) {

        this.baseUrl = environment.apiUrl;
    }

    getUserInfo(): Observable<UserInfo> {
        return this.httpClient.get<UserInfo>(this.baseUrl + 'userinfo').pipe(
            map(response => {
                return response;
            })
        );
    }
}
