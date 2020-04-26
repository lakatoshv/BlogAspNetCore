import { PageInfo } from './../../../../core/models/PageInfo';
import { CommentService } from './../../../../core/services/posts-services/comment.service';
import { Component, OnInit, Input } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import { GeneralServiceService } from 'src/app/core';

import { Comment } from 'src/app/core/models/Comment';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { UsersService } from 'src/app/core/services/users-services/users.service';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {
  /**
   * @param postId number
   */
  @Input("post-id") postId: number;

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
   * @param comment Comment
   */
  public comment: Comment;

  /**
   * @param user User
   */
  public user: User;

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param editPostId number
   */
  public editPostId: number;

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
   */
  constructor(
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _commentService: CommentService,
    private _usersService: UsersService,
    private _globalService: GlobalService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
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
    this._commentService.list(this.postId, sortParameters)
      .subscribe((response: any) => {
        this.comments = response.comments;
        this.pageInfo = response.pageInfo;
      },
      error => {
      });
  }

  /**
   * Comments pagination.
   *
   * @param page number
   * @returns void
   */
  public paginate(page: number): void {
    this.pageInfo.pageNumber = page;
  }
}
