import { CommentService } from './../../../../core/services/posts-services/comment.service';
import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CommentForm } from 'src/app/core/forms/posts/CommentForm';

import { Comment } from '../../../../core/models/Comment';
import { User } from 'src/app/core/models/User';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Messages } from 'src/app/core/data/Messages';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';

@Component({
  selector: 'app-edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.css']
})
export class EditCommentComponent implements OnInit {
  /**
   * @param comment Comment | undefined
   */
  @Input() comment: Comment | undefined;

  /**
   * @param onEdit EventEmitter<any>
   */
  @Output() onEdit: EventEmitter<any> = new EventEmitter<any>();

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param commentForm FormGroup
   */
  commentForm: FormGroup = new CommentForm().commentForm;

  /**
   * @param _commentService CommentService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _commentService: CommentService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.loggedIn = this._usersService.isLoggedIn();

    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
    if (this.user?.id === this.comment?.userId) {
      this.setFormValue();
    }
  }

  /**
   * Sets form value
   * @returns void
   */
  public setFormValue(): void {
    this.commentForm.get('name')?.setValue(this.comment?.user?.firstName + ' ' + this.comment?.user?.lastName);
    this.commentForm.get('email')?.setValue(this.comment?.user?.email);
    this.commentForm.get('content')?.setValue(this.comment?.commentBody);
  }

  /**
   * Edit comment
   * @param comment Comment
   * @returns void
   */
  public edit(): void {
    if (this.user?.id === this.comment?.userId
      && this.commentForm.valid
      && this.comment) {
      this.comment.commentBody = this.commentForm.get('content')?.value;
      this._commentService.edit(this.comment.id, this.comment).subscribe(
        (response: any) => {
          this.onEdit.emit(response);
          this._customToastrService.displaySuccessMessage(Messages.COMMENT_EDITED_SUCCESSFULLY);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }
}
