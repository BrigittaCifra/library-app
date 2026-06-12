import { Component, inject } from '@angular/core';

import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule],
  templateUrl: `./login-page.html`,
  styles: ``,
})
export class LoginPage {

  private readonly fb = inject(FormBuilder);

  loginForm = this.fb.group({
    email: [''],
    password: ['']
  })

  onReset() {
    console.log("Reset");
  }

  //När formuläret skickas
  onSubmit() {
    console.log("Lyckad inlogg");
  }

}
