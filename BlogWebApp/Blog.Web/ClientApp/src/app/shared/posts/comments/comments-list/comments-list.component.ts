import { CommentsService } from './../../../../core/services/posts-services/comments.service';
import { Component, OnInit, Input, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { Comment } from './../../../../core/models/Comment';
import { UsersService } from '../../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../../core/services/global-service/global-service.service';
import { User } from './../../../../core/models/User';
import { Messages } from './../../../../core/data/Mesages';
import { CustomToastrService } from './../../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../../core/responses/ErrorResponse';
import { PageInfo } from '../../../../core/models/PageInfo';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentsListComponent implements OnInit {
   /**
   * @param postId number | undefined
   */
  @Input("post-id") postId: number | undefined;

  /**
   * @param comments Comment[]
   */
  @Input("comments") comments:  Comment[] = [];

  /**
   * @param pageInfo Object
   */
  @Input('pageInfo') pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0,
  };

  /**
   * @param comment Comment | undefined
   */
  public comment: Comment | undefined;

  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param editPostId number | undefined
   */
  public editPostId: number | undefined;

  /**
   * @param isLoadEdit boolean
   */
  isLoadEdit = false;
  isLoaded = false;

  /**
   * @param _generalService GeneralServiceService
   * @param _activatedRoute ActivatedRoute
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
  async ngOnInit(): Promise<void> {
    // this._getCommentsForCurrentPost();

    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      // this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
    this.isLoaded = true;
  }

  list(page = 1) {
    const sortParameters = {
      sortBy: null,
      orderBy: null,
      currentPage: page,
      pageSize: 10,
      displayType: null
    };

    if(this.postId) {
      this._commentsService.list(this.postId, sortParameters)
        .pipe(
          finalize(() => {
            this._changeDetectorRef.markForCheck();
          })
        )
        .subscribe({
          next: (response: any) => {
            this.comments = response.comments;
            this.pageInfo = response.pageInfo;
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Comments pagination.
   *
   * @param page number
   * @returns void
   */
  public paginate(page: number): void {
    this.pageInfo.pageNumber = page;
    this.list(page);
  }

  /**
   * Add comment event
   * @param comment Comment
   * @returns void
   */
  onAddAction(comment: Comment): void {
    /*this.comments.unshift(comment);
    this.pageInfo.totalItems += 1;*/
    this.list();
  }

  /**
   * Edit comment event
   * @param comment Comment
   * @returns void
   */
  onEditAction(comment: Comment): void {
    const index = this.comments?.findIndex(x => x.id === comment.id);
    if (this.comments && index && index > -1) {
      this.comments[index] = comment;
      this.isLoadEdit = false;
    }
  }

  /**
   * Edit comment event
   * @param comment Comment
   * @returns void
   */
  editAction(comment: Comment): void {
    if (this.loggedIn && this.user?.id === comment.userId) {
      this.editPostId = comment.id;
      this.comment = comment;
      this.isLoadEdit = true;
    }
  }

  /**
   * Delete comment event
   * @param comment Comment
   * @returns void
   */
  async deleteAction(comment: Comment): Promise<void> {
    this._commentsService.delete(comment.id)
      .subscribe({
        next: (response: any) => {
          this.onDeleteCommentAction(response.id);
          this._customToastrService.displaySuccessMessage(Messages.COMMENT_DELETED_SUCCESSFULLY);
        },
        error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        }
      });
  }

  /**
   * On comment deleted event.
   * @param commentId number
   * @returns void
   */
  onDeleteCommentAction(commentId: number): void {
    const index = this.comments?.findIndex(x => x.id === commentId);
    if (this.comments && index && index > -1) {
      this.comments.splice(index, 1);
    }

    this.pageInfo.totalItems -= 1;
  }

  /**
   * Load edit component event
   * @returns void
   */
  private _onLoadEditAction(): void {
    this.isLoadEdit = true;
  }
}
