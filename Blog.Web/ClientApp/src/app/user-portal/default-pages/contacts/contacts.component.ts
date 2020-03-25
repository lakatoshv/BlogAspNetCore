import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MessageForm } from '../../../core/forms/Message';

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

  constructor() { }

  ngOnInit() {
  }

}
