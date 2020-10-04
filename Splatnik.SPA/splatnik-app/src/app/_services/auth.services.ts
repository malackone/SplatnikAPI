import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { registerLocaleData } from '@angular/common';

const AUTH_API = environment.apiUrl;

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
};

@Injectable({
    providedIn: 'root'
})

export class AuthService {
    constructor(private http: HttpClient){}

    login(credentials): Observable<any> {
        return this.http.post(AUTH_API + 'identity/login', {
            email: credentials.email,
            password: credentials.password
        }, httpOptions);
    }

    register(user): Observable<any> {
        return this.http.post(AUTH_API + 'identity/register', {
            email: user.email,
            password: user.password
        }, httpOptions);
    }

    

}

