import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthApiWrapperService } from './auth-api-wrapper.service';
import { TokenResponse } from '../shared/token-response';

@Component({
  templateUrl: './login.component.html'
})
export class LoginComponent {
  tokenResponse: any; // Angular doesn't support unions in templates during aot. Should be: TokenResponse | HttpErrorResponse;
  form = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(private formBuilder: FormBuilder, private authApiWrapperService: AuthApiWrapperService, private router: Router) { }

  onSubmit() {
    if (this.form.valid === false) {
      return;
    }
    this.authApiWrapperService.logIn(
      this.form.controls.email.value,
      this.form.controls.password.value
    ).subscribe(e => {
      this.tokenResponse = e;
      if ((<HttpErrorResponse>e).error == null) {
        this.router.navigateByUrl('/');
      }
    });
  }
}
