import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MessageForm } from '../../../core/forms/MessageForm';
import { Messages } from '../../../core/data/Mesages';
import { MessageType } from '../../../core/enums/MessageType';
import { User } from '../../../core/models/User';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { GlobalService } from '../../../core/services/global-service/global-service.service';
import { MessagesService } from '../../../core/services/messages-service';
import { UsersService } from '../../../core/services/users-services/users-service.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss'],
  standalone: false
})
export class ContactsComponent implements OnInit {
  /**
   * @param messageForm FormGroup
   */
  public messageForm: FormGroup = new MessageForm().messageForm;

  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _messagesService MessagesService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _messagesService: MessagesService,
    private _customToastrService: CustomToastrService) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
  }

  /**
   * Send message.
   * @param message any
   * @returns void
   */
  public sendMessage(message: any): void {
    message['messageType'] = MessageType.MessageForAdmins;
    this._messagesService.sendMessage(message).subscribe(
      () => {
        this._customToastrService.displaySuccessMessage(Messages.MESSAGE_SENDED_SUCCESSFULLY);
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }
}
