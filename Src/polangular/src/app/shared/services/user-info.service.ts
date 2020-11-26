import {Injectable} from '@angular/core';


@Injectable()
export class UserInfoService {

    homeAdress: string
    workAdress: string



    constructor(
        
    ) {
        this.homeAdress = 'skanderborgvej 159'
        this.workAdress = 'work adress 4'
    }

    getHomeAdress() {
        return this.homeAdress
    }
    getWorkAdress() {
        return this.workAdress
    }
}
