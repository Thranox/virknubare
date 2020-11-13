import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccessHTMLElementService {

    constructor() { }

    getById(elementId) {
        return document.getElementById(elementId)
    }
}
