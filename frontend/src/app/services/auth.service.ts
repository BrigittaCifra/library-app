import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

//Modeller
import { RegisterModel } from '../models/register.model';
import { LoginModel } from '../models/login.model';
import { AuthResponseModel } from '../models/auth-response.model';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private http = inject(HttpClient);
    private url = environment.apiUrl + '/user';

    Register(registerRequest: RegisterModel) {
        return this.http.post<AuthResponseModel>(`${this.url}/register`, registerRequest);
    }

    Login(loginRequest: LoginModel) {
        return this.http.post<AuthResponseModel>(`${this.url}/login`, loginRequest);
    }

}