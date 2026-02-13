import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { RegistrationForm } from '../../../core/forms/user/RegistrationForm';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { Router } from '@angular/router';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegistrationComponent implements OnInit {
  /**
   * @param registrationForm FormGroup
   */
  registrationForm: FormGroup = new RegistrationForm().registrationForm;

  /**
   * @param _usersService UsersService
   * @param _router Router
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _usersService: UsersService,
    private _router: Router,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    if (this._usersService.isLoggedIn()) {
      this._router.navigate(['/']);
    }
  }

  /**
   * Register user.
   */
  async register(): Promise<void> {
    if (
      this.registrationForm.valid &&
      this.registrationForm.value.password === this.registrationForm.value.confirmPassword) {
      const roles: string[] = [];
      roles.push('User');
      this.registrationForm.value.roles = roles;
      this._usersService.registration(this.registrationForm.value)
        .subscribe({
          next: () => {
            // this._logIn(registerModel);
            // this.isLoginRequestSend = false;
            this._customToastrService.displaySuccessMessage(Messages.REGISTERED_SUCCESSFULLY);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }
}
