import { CommentsService } from './../../../../core/services/posts-services/comments.service';
import { Component, OnInit, Input, EventEmitter, Output, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CommentForm } from './../../../../core/forms/posts/CommentForm';
import { Comment } from '../../../../core/models/Comment';
import { User } from './../../../../core/models/User';
import { UsersService } from '../../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../../core/services/global-service/global-service.service';
import { CustomToastrService } from './../../../../core/services/custom-toastr.service';
import { Messages } from './../../../../core/data/Mesages';
import { ErrorResponse } from '../../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
   * @param _commentsService Commentservice
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _commentsService: CommentsService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
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
      this._commentsService.edit(this.comment.id, this.comment).subscribe(
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
