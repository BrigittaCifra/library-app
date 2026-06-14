import { HttpErrorResponse, HttpHeaderResponse } from "@angular/common/http";
import { inject, Injectable, signal, computed } from "@angular/core";
import { Router } from '@angular/router';

//JWT relaterade import
import { AuthService } from "../services/auth.service";
import { RegisterModel } from "../models/register.model";
import { LoginModel } from "../models/login.model";
import { AuthResponseModel } from "../models/auth-response.model";

import { jwtDecode } from "jwt-decode";

export interface AuthState {
    token: string | null,
    loading: boolean,
    error: HttpErrorResponse | null
}

const defaultState: AuthState = {
    token: null,
    loading: false,
    error: null
}

@Injectable({ providedIn: 'root' })
export class AuthStore {

    private readonly authService = inject(AuthService);
    private router = inject(Router);
    private state = signal(defaultState);
    private readonly tokenKey: string = "auth_token";

    token = computed(() => this.state().token);
    loading = computed(() => this.state().loading);
    error = computed(() => this.state().error);

    //state uppdateringar
    //loading
    private setLoading() {
        //spread operatorn kopierar alla gamla state och sätter loading till true
        this.state.update(() => ({ ...this.state(), loading: true }));
    }

    //error
    private setError(error: HttpErrorResponse) {
        this.state.update(() => ({ ...this.state(), loading: false, error }));
    }

    //state metoder
    //Hämtar token från localStorage
    private getToken(): string | null {
        return localStorage.getItem(this.tokenKey) || null;
    }

    //kollar tokenen är giltig
    private isTokenValid(token: string): boolean {
        //Tokenen skickas in
        if (token == null) return false;

        const decodedToken = jwtDecode(token);

        if (Date.now() > decodedToken['exp']! * 1000) {
            this.logOut();
            return false;
        }

        return true;
    }

    //Registrerar en ny användare 
    registerUser(credentials: RegisterModel) {
        //sätter loadingen till true
        this.setLoading();
        this.authService.Register(credentials).subscribe({
            next: (response: AuthResponseModel) => {
                //Sparar token i local storage
                localStorage.setItem(this.tokenKey, response.token);
                //Uppdaterar token staten och sätter loadingen till false
                this.state.update(() => ({ ...this.state(), token: response.token, loading: false }));
                //Navigerar till landningssidan
                this.router.navigate(['/']);
            },
            error: (error) => {
                //loggar ut felmeddelandet till konsolen
                console.log(`Registreringen misslyckades ${error}`);
                this.setError(error);
            }
        });
    }

    //Login metoder
    loginUser(credentials: LoginModel) {
        //sätter loadingen till true
        this.setLoading();

        this.authService.Login(credentials)
            .subscribe({
                next: (response: AuthResponseModel) => {
                    //Sparar token i local storage
                    localStorage.setItem(this.tokenKey, response.token);
                    //Uppdaterar token staten och sätter loadingen till false
                    this.state.update(() => ({ ...this.state(), token: response.token, loading: false }));
                    //Navigerar till landningssidan
                    this.router.navigate(['/']);
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(`Inloggningen misslyckades ${error}`);
                    this.setError(error);
                }
            });
    }

    isLoggedIn(): boolean {
        //Hämtar tokenen från lokal storage
        const token = this.getToken();

        //Kollar om värdet är null eller om det finns ett värde
        if (token == null) return false;

        //Kollar om tokenen är giltig. isTokenValid() returnerar också en boolean
        return this.isTokenValid(token);
    }

    //Loggar ut användaren 
    logOut(): void {
        //Tar bort tokenen från localstorage
        localStorage.removeItem(this.tokenKey);

        //Ändrar tillbaka staten till default staten
        this.state.update(() => ({ ...defaultState }));

        //Navigerar användaren till inlogg sidan
        this.router.navigate(['/user/login']);
    }

    //Sätter en header men tokenen som request kan använda för att komma åt skidade endpoints
    getHeaders() {
        return { Authorization: `Bearer ${this.getToken()}` };
    }

    //Körs när appen startar. kollar om token redan finns i localStorage
    constructor() {
        //Hämtar tokenen
        const token = this.getToken();

        //Om det finns ett token uppdateras token staten
        if (token) {
            this.state.update(() => ({ ...this.state(), token }));
        }
    }

}