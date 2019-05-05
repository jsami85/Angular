import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserService {

    constructor(private http: HttpClient) {
    }

    getUsers() {

        return this.http.get<any>(`http://localhost:63100/api/Users/`);
        // return this.http.get('/users');
    }

    getUser(id: any) {
        return null; // this.http.get(`/users/${id}`);
    }
}
