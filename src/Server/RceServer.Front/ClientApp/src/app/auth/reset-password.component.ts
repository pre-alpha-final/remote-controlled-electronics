import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ValidationErrors } from '@angular/forms';
import { ErrorType } from '../shared/error-type';
import { AuthApiWrapperService } from './auth-api-wrapper.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordResponse: any; // Angular doesn't support unions in templates during aot. Should be: Object | ErrorType;
  private userId: string;
  private code: string;
  form = this.formBuilder.group({
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&,.\\/\\-+=()])[ A-Za-z\\d@$!%*?&,.\\/\\-+=()]{1,}$')
    ]],
    password2: ['']
  }, { validator: this.passwordMatchValidator });

  constructor(private formBuilder: FormBuilder, private authApiWrapperService: AuthApiWrapperService,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.code = params['code'];
    });
  }

  passwordMatchValidator(group: FormGroup): ValidationErrors {
    const password = group.controls.password.value;
    const password2 = group.controls.password2.value;

    return password === password2
      ? null
      : { passwordsMatch: false };
  }

  onSubmit() {
    if (this.form.valid === false) {
      return;
    }
    this.authApiWrapperService.resetPassword(
      this.userId,
      this.code,
      this.form.controls.password.value,
      this.form.controls.password2.value
    ).subscribe(e => this.resetPasswordResponse = e || {});
  }
}
