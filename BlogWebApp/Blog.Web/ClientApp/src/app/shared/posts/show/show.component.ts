import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';
import { GeneralServiceService } from 'src/app/core';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { Post } from 'src/app/core/models/Post';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { PageInfo } from 'src/app/core/models/PageInfo';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';

@Component({
  selector: 'app-show',
  templateUrl: './show.component.html',
  styleUrls: ['./show.component.css']
})
export class ShowComponent implements OnInit {
  /**
   * @param tagInput ElementRef | undefined
   */
  @ViewChild('tag') tagInput: ElementRef | undefined;

  /**
   * @param post Post | undefined
   */
  public post: Post | undefined;

  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param isLoaded boolean
   */
  public isLoaded = false;

  /**
   * @param postId number | undefined
   */
  public postId: number | undefined;

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param _generalService GeneralServiceService
   * @param _activatedRoute ActivatedRoute
   * @param _postService PostService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _router Router
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _postService: PostService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _router: Router,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.postId = parseInt(this._generalService.getRouteParam('post-id', this._activatedRoute) ?? '', undefined);
    this._getPost();
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
  }

  /**
   * Like post by id.
   * @param id number
   * @returns void
   */
  public like(id: number): void {
    if(this.postId) {
      this._postService.like(this.postId).subscribe(
        (response: any) => {
          this.post = response;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Dislike post by id.
   * @param id number
   * @returns void
   */
  public dislike(id: number): void {
    if(this.postId) {
      this._postService.dislike(this.postId).subscribe(
        (response: any) => {
          this.post = response;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Delete post.
   */
  public deleteAction() {
    if (this.loggedIn && this.post && this.post?.authorId === this.user?.id) {
      this._postService.delete(this.post.id, this.user.id).subscribe(
        () => {
          this._router.navigateByUrl('/blog');
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Back button.
   */
  backButton(): void {
    this._router.navigateByUrl('/blog');
  }

  /**
   * Get post by id.
   */
  private _getPost() {
    if(this.postId) {
      this._postService.showPost(this.postId).subscribe(
        (response: any) => {
          this.post = response.post;
          if(this.post) {
            this.post.tags = response.tags;
            this.post.comments = response.comments.comments;
          }
          this.pageInfo = response.comments.pageInfo;
          this.isLoaded = true;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

}
