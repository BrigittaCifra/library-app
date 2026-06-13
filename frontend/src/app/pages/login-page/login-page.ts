//Angular imports
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

import { AuthStore } from '../../stores/auth.store';
import { LoginModel } from '../../models/login.model';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule],
  templateUrl: `./login-page.html`,
  styles: ``,
})
export class LoginPage {

  private readonly fb = inject(FormBuilder);
  private readonly authStore = inject(AuthStore);

  loginForm = this.fb.group({
    email: [''],
    password: ['']
  })

  login() {
    const loginModel: LoginModel = {
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    }
    this.authStore.loginUser(loginModel);
  }

  onReset() {
    console.log("Reset");
  }

}
