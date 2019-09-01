import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ErrorType } from '../shared/error-type';
import { AuthApiWrapperService } from './auth-api-wrapper.service';

@Component({
  templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent {
  forgotPasswordResponse: any; // Angular doesn't support unions in templates during aot. Should be: Object | ErrorType;
  form = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]]
  });

  constructor(private formBuilder: FormBuilder, private authApiWrapperService: AuthApiWrapperService) { }

  onSubmit() {
    if (this.form.valid === false) {
      return;
    }
    this.authApiWrapperService.forgotPassword(this.form.controls.email.value).subscribe(
      e => this.forgotPasswordResponse = e || {});
  }
}
