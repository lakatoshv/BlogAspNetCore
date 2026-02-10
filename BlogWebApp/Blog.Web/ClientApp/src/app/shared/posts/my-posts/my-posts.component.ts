import { GeneralServiceService } from './../../../core/services/general-service.service';
import { PostsService } from './../../../core/services/posts-services/posts.service';
import { PageInfo } from './../../../core/models/PageInfo';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { SearchForm } from './../../../core/forms/SearchForm';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { User } from './../../../core/models/User';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { Post } from './../../../core/models/Post';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-my-posts',
  templateUrl: './my-posts.component.html',
  styleUrls: ['./my-posts.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyPostsComponent implements OnInit {
  /**
   * @param posts Post[]
   */
  public posts: Post[] = [];

  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param isLoaded boolean
   */
  public isLoaded = false;

  /**
   * @param isCurrentUserPosts boolean
   */
  public isCurrentUserPosts = false;

  /**
   * @param displayType string
   */
  public displayType = 'list';

  /**
   * @param sortBy string
   */
  public sortBy = 'title';

  /**
   * @param orderBy string
   */
  public orderBy = 'asc';

  /**
   * @param searchForm FormGroup
   */
  public searchForm: FormGroup = new SearchForm().searchForm;

  /**
   * @param _userId string | undefined
   */
  private _userId: string | undefined;

  /**
   * @param _searchFilter any
   */
  private _searchFilter: any;

  /**
   * @param _globalService GlobalService
   * @param _router Router
   * @param _activatedRoute ActivatedRoute
   * @param _usersService UsersService
   * @param _customToastrService CustomToastrService
   * @param _generalService GeneralServiceService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _globalService: GlobalService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _postsService: PostsService,
    private _usersService: UsersService,
    private _customToastrService: CustomToastrService,
    private _generalService: GeneralServiceService,
    private _changeDetectorRef: ChangeDetectorRef
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._searchFilter = this._generalService.getRouteParam('search-filter', this._activatedRoute);

    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this.isLoggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }

    if (this._router.url.includes('/my-posts')) {
      this._userId = this.user?.id;
      this.isCurrentUserPosts = true;
    } else {
      this._userId = this._globalService.getRouteParam('user-id', this._activatedRoute) ?? undefined;
      this.isCurrentUserPosts = false;
    }
    this._getPosts();
  }

  display(displayType: string): void {
    this.displayType = displayType;
  }

  /**
   * Delete post event.
   * @param postId number
   * @returns void
   */
  public async deleteAction(postId: number): Promise<void> {
    const post = this.posts.find(p =>  p.id === postId);
    if (this.isLoggedIn && this.posts[postId].author.id === this.user?.id && this._globalService._currentUser) {
      this._postsService.delete(postId, this._globalService._currentUser.id)
        .subscribe({
          next: (response: any) => {
          this._customToastrService.displaySuccessMessage(Messages.POST_DELETED_SUCCESSFULLY);
          this._onDeleteCommentAction(response.id);
        },
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Like post by id.
   * @param id number
   * @returns void
   */
  public async like(id: number): Promise<void> {
    if (this.isLoggedIn) {
      this._postsService.like(id)
        .subscribe({
          next: (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
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
    if (this.isLoggedIn) {
      this._postsService.dislike(id)
        .subscribe({
          next: (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
        },
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Post pagination.
   * @param page number
   * @returns void
   */
  public paginate(page: number): void {
    this.pageInfo.pageNumber = page;
  }

  /**
   * Search post by Title
   * @param search string
   * @returns void
   */
  async search(search: string): Promise<void> {
    this.isLoaded = false;
    const model = {
      search: search,
      tag: this._searchFilter,
      sortParameters: null,
    };
    if(this._userId) {
      this._postsService.userPosts(this._userId, model)
        .subscribe({
          next: (response: any) => {
          this.posts = response.posts;
          this.pageInfo = this.pageInfo;
        },
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Sort posts by parameter.
   * @returns void
   */
  async sort(): Promise<void> {
    const sortParameters = {
      sortBy: this.sortBy,
      orderBy: this.orderBy,
      currentPage: null,
      pageSize: null,
      displayType: null,
    };
    const model = {
      search: null,
      tag: this._searchFilter,
      sortParameters: sortParameters,
    };
    if(this._userId) {
      this._postsService.userPosts(this._userId, model)

        .subscribe({
          next: (response: any) => {
          this.posts = response.posts;
          this.pageInfo = response.pageInfo;
        },
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Get all posts.
   * @returns void
   */
  private async _getPosts(page = 1): Promise<void> {
    const sortParameters = {
      sortBy: null,
      orderBy: null,
      currentPage: page,
      pageSize: 10,
      displayType: null,
    };
    const model = {
      search: null,
      tag: this._searchFilter,
      sortParameters: sortParameters,
    };

    if(this._userId) {
      this._postsService.userPosts(this._userId, model)

        .subscribe({
          next: (response: any) => {
          this.posts = response.posts;
          this.pageInfo = response.pageInfo;
        },
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }

    this.pageInfo.totalItems = this.posts.length;
  }

  /**
   * Delete post event.
   * @param postId number
   * @returns void
   */
  private _onDeleteCommentAction(postId: number): void {
    const index = this.posts.findIndex(x => x.id === postId);
    if (index > -1) {
      this.posts.splice(index, 1);
      this.pageInfo.totalItems -= 1;
    }
    this.posts = this.posts;
  }
}
