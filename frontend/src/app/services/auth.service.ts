import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

import { RegisterModel } from '../models/register.model';
import { LoginModel } from '../models/login.model';
import { AuthResponseModel } from '../models/auth-response.model';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

}