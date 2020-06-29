import { MessageType } from './../../../core/enums/MessageType';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MessageForm } from '../../../core/forms/Message';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { User } from 'src/app/core/models/User';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { MessagesService } from 'src/app/core/services/messages-service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {
  /**
   * @param messageForm FormGroup
   */
  public messageForm: FormGroup = new MessageForm().messageForm;

  /**
   * @param user User
   */
  public user: User = null;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _messagesService MessagesService
   */
  constructor(
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _messagesService: MessagesService) {
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
      (result: any) => {},
      (error) => {}
    );
  }
}
