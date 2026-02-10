import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { User } from '../../../core/models/User';
import { Comment } from '../../../core/models/Comment';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { PageInfo } from '../../..//core/models/PageInfo';
import { CommentsService } from '../../../core/services/posts-services/comments.service';
import { Messages } from '../../../core/data/Mesages';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-admin-comments-list',
  templateUrl: './admin-comments-list.component.html',
  styleUrls: ['./admin-comments-list.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminCommentsListComponent implements OnInit {
  /**
   * @param comments Comment[]
   */
  comments: Comment[] = [];

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param user User
   */
  public user: User | undefined;

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param isLoaded boolean
   */
  isLoaded: boolean = false;

  /**
   * @param postId number
   */
  public postId: number | undefined;

  /**
   * @param _commentService CommentService
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _commentsService: CommentsService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._getComments();

    /*this._commentService.commentChanged.subscribe(
      () => {
        if(this.postId !== undefined){
          this._getComments(this.postId);
        }
      }
    );*/
  }

  /**
   * Delete comment event
   * @param comment Comment
   * @returns void
   */
  deleteAction(comment: Comment): void {
    this._commentsService.delete(comment.id);
    this._customToastrService.displaySuccessMessage(Messages.COMMENT_DELETED_SUCCESSFULLY);
  }

  /**
   * Get all comments.
   * @param page number
   * @returns void
   */
  private async _getComments(page: number = 0): Promise<void> {
    const sortParameters = {
      sortBy: null,
      orderBy: null,
      currentPage: page,
      pageSize: 10,
      displayType: null
    };
    const model = {
      search: null,
      tag: null,
      sortParameters: sortParameters
    };
    
    this._commentsService.list(null, model)
      .pipe(
        finalize(() => {
          this.isLoaded = true;
          this._changeDetectorRef.markForCheck();
        })
      )
    .subscribe({
      next: (response: any) => {
        this.comments = [...response.comments];
      },
      error: (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      }
    });
    /*!isNaN(postId)
      ? this._commentService.getCommentsByPostId(postId)
      : this._commentService.list(nu);*/
  }

}
