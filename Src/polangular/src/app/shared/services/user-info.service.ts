import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {UserInfo} from '../model/user-info.model';
import {UserInfoResource} from '../resources/user-info.resource';


@Injectable()
export class UserInfoService {

    constructor(
        private userInfoResource: UserInfoResource,
    ) {
    }

    getUserInfo(): Observable<UserInfo> {
        return this.userInfoResource.getUserInfo();
    }
}
