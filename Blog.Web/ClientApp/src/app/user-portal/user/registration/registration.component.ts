import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { RegistrationForm } from '../../../core/forms/user/RegistrationForm';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  /**
   * @param registrationForm FormGroup
   */
  registrationForm: FormGroup = new RegistrationForm().registrationForm;

  /**
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _router Router
   */
  constructor(
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _router: Router
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
  register() {
    if (this.registrationForm.value.password === this.registrationForm.value.confirmPassword) {
      const roles: string[] = [];
      roles.push('User');
      this.registrationForm.value.roles = roles;
      this._usersService.registration(this.registrationForm.value)
        .subscribe(
          (response) => {
            // this._logIn(registerModel);
            // this.isLoginRequestSend = false;
          },
          (errorMessage) => {});
    }
  }

}
