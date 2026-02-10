import { PostsService } from './../../../core/services/posts-services/posts.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GeneralServiceService } from './../../../core';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { User } from './../../../core/models/User';
import { Post } from './../../../core/models/Post';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { PageInfo } from '../../../core/models/PageInfo';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-show',
  templateUrl: './show.component.html',
  styleUrls: ['./show.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
   * @param _postsService PostsService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _router Router
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _postsService: PostsService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _router: Router,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
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
  public async like(id: number): Promise<void> {
    if(this.postId) {
      this._postsService.like(this.postId)
        .subscribe({
          next: (response: any) => {
            this.post = response;
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Dislike post by id.
   * @param id number
   * @returns void
   */
  public async dislike(id: number): Promise<void> {
    if(this.postId) {
      this._postsService.dislike(this.postId)
        .subscribe({
          next: (response: any) => {
            this.post = response;
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Delete post.
   */
  public async deleteAction(): Promise<void> {
    if (this.loggedIn && this.post && this.post?.authorId === this.user?.id) {
      this._postsService.delete(this.post.id, this.user.id)
        .subscribe({
          next: (response: any) => {
            this._router.navigateByUrl('/blog');
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
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
  private async _getPost(): Promise<void> {
    if(this.postId) {
      this._postsService.showPost(this.postId)
        .pipe(
          finalize(() => {
            this.isLoaded = true;
            this._changeDetectorRef.markForCheck();
          })
        )
        .subscribe({
          next: (response: any) => {
            this.post = response.post;
            if(this.post) {
              this.post.tags = response.tags;
              this.post.comments = response.comments.comments;
            }
            this.pageInfo = response.comments.pageInfo;
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }
}
