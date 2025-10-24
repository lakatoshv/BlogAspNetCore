import { CommentsService } from './../../../../core/services/posts-services/comments.service';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CommentForm } from './../../../../core/forms/posts/CommentForm';
import { Comment } from '../../../../core/models/Comment';
import { User } from './../../../../core/models/User';
import { CustomToastrService } from './../../../../core/services/custom-toastr.service';
import { Messages } from './../../../../core/data/Mesages';
import { ErrorResponse } from '../../../../core/responses/ErrorResponse';
import { UsersService } from '../../../../core/services/users-services/users-service.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.scss'],
  standalone: false
})
export class AddCommentComponent implements OnInit {
  /**
   * @param postId number | undefined
   */
  @Input() postId: number | undefined;

  /**
   * @param user User | undefined
   */
  @Input() user: User | undefined;

  /**
   * @param onAdd EventEmitter<any>
   */
  @Output() onAdd: EventEmitter<any> = new EventEmitter<any>();

  /**
   * @param commentForm FormGroup
   */
  commentForm: FormGroup = new CommentForm().commentForm;

  /**
   * @param _globalService GlobalService
   * @param _commentsService CommentsService
   * @param _usersService UsersService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _commentsService: CommentsService,
    private _usersService: UsersService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    if (this.user) {
      this.commentForm.get('name')?.setValue(this.user.firstName + ' ' + this.user.lastName);
      this.commentForm.get('email')?.setValue(this.user.email);
    }
  }

  /**
   * Add Comment
   * @returns void
   */
  addComment(): void {
    if (this._usersService.isLoggedIn() && this.commentForm.valid) {
      const comment: Comment = new Comment ();
      comment.postId = this.postId;
      comment.commentBody = this.commentForm.get('content')?.value;
      comment.createdAt = new Date();
      if (this.user) {
        comment.userId = this.user.id;
      } else {
        comment.email = this.commentForm.get('email')?.value;
        comment.name = this.commentForm.get('name')?.value;
      }

      this._commentsService.add(comment).subscribe(
        (response: any) => {
          this.onAdd.emit(response.json());
          this._customToastrService.displaySuccessMessage(Messages.COMMENT_CREATED_SUCCESSFULLY);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
      this.onAdd.emit(null);
    }
  }
}
