import { CommentService } from './../../../../core/services/posts-services/comment.service';
import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CommentForm } from 'src/app/core/forms/posts/CommentForm';

import { Comment } from '../../../../core/models/Comment';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { UsersService } from 'src/app/core/services/users-services/users.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit {
  /**
   * @param postId number
   */
  @Input() postId: number;

  /**
   * @param user User
   */
  @Input() user: User = null;

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
   * @param _commentService CommentService
   * @param _usersService UsersService
   */
  constructor(
    private _globalService: GlobalService,
    private _commentService: CommentService,
    private _usersService: UsersService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    if (this.user) {
      this.commentForm.get('name').setValue(this.user.firstName + ' ' + this.user.lastName);
      this.commentForm.get('email').setValue(this.user.email);
    }
  }

  /**
   * Add Comment
   * @returns void
   */
  addComment(): void {
    if (this._usersService.isLoggedIn) {
      const comment: Comment = new Comment ();
      comment.postId = this.postId;
      comment.commentBody = this.commentForm.get('content').value;
      comment.createdAt = new Date();
      if (this.user) {
        comment.userId = this.user.id;
      } else {
        comment.email = this.commentForm.get('email').value;
        comment.name = this.commentForm.get('name').value;
      }

      this._commentService.add(comment).subscribe(
        (response: any) => {
          this.onAdd.emit(response.json());
        },
        (error) => {}
      );
      this.onAdd.emit(null);
    }
  }
}
