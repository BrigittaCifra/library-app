//Angular imports
import { Component, inject } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

import { AuthStore } from '../../stores/auth.store';
import { LoginModel } from '../../models/login.model';
import { RegisterModel } from '../../models/register.model';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: `./auth-form.html`,
  styles: ``,
})
export class AuthForm {

  private readonly fb = inject(FormBuilder);
  private readonly authStore = inject(AuthStore);
  private readonly router = inject(Router);

  loginForm = this.fb.group({
    username: [''],
    email: [''],
    password: ['']
  })

  //Skapar en boolean som är true om url:en är /user/login
  isLoginPage: boolean = this.router.url === '/user/login';
  error = this.authStore.error;

  onReset() {
    console.log("Reset");
  }

  login() {
    const loginModel: LoginModel = {
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    }
    this.authStore.loginUser(loginModel);
  }

  register() {
    const registerModel: RegisterModel = {
      username: this.loginForm.value.username ?? '',
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    }
    this.authStore.registerUser(registerModel);
  }

  onSubmit() {
    if (this.isLoginPage) {
      this.login();
    } else {
      this.register();
    }
    this.onReset();
  }

}
