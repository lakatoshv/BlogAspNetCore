import { Component } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Messages } from 'src/app/core/data/Messages';
import { User } from 'src/app/core/models/User';
import { Comment } from 'src/app/core/models/Comment';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { GeneralServiceService } from 'src/app/core/services/general-service.service';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { CommentService } from 'src/app/core/services/posts-services/comment.service';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { PageInfo } from 'src/app/core/models/PageInfo';

@Component({
  selector: 'app-admin-comments-table',
  templateUrl: './admin-comments-table.component.html',
  styleUrls: ['./admin-comments-table.component.scss']
})
export class AdminCommentsTableComponent {
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
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _commentService: CommentService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _customToastrService: CustomToastrService
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
    this._commentService.delete(comment.id);
    this._customToastrService.displaySuccessMessage(Messages.COMMENT_DELETED_SUCCESSFULLY);
  }

  /**
   * Get all comments.
   * @param page number
   * @returns void
   */
  private _getComments(page: number = 0): void {
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
    this._commentService.list(null, model).subscribe(
      (response: any) => {
        this.comments = response.comments;
        this.isLoaded = true;
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
        this.isLoaded = true;
      });
    /*!isNaN(postId)
      ? this._commentService.getCommentsByPostId(postId)
      : this._commentService.list(nu);*/
  }

  /**
   * Check if user is logged in.
   * @returns void
   */
  private _checkIfUserIsLoggedIn(): void {
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
  }
}
