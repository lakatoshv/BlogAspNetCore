import { PostsService } from './../../../core/services/posts-services/posts.service';
import { Component, OnInit } from '@angular/core';
import { GeneralServiceService } from './../../../core';
import { ActivatedRoute, Params } from '@angular/router';
import { Post } from './../../../core/models/Post';
import { User } from './../../../core/models/User';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { FormGroup } from '@angular/forms';
import { SearchForm } from './../../../core/forms/SearchForm';
import { PageInfo } from './../../../core/models/PageInfo';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { Messages } from './../../../core/data/Mesages';
import { PageViewDto } from '../../../core/Dto/PageViewDto';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.scss'],
  standalone: false
})
export class PostsListComponent implements OnInit {
  /**
   * @param posts Post[]
   */
  public posts: Post[] = [];

  /**
   * @param searchForm FormGroup
   */
  public searchForm: FormGroup = new SearchForm().searchForm;

  /**
   * @param sortBy string
   */
  public sortBy = 'title';

  /**
   * @param orderBy string
   */
  public orderBy = 'asc';

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
   * @param _postId number | undefined
   */
  private _postId: number | undefined;

  /**
   * @param users User[]
   */
  private users: User[] = [];

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param isLoaded boolean
   */
  public isLoaded = false;

  /**
   * @param _searchFilter any
   */
  private _searchFilter: any;

  /**
   * @param _globalService GlobalService
   * @param _generalService GeneralServiceService
   * @param _activatedRoute ActivatedRoute
   * @param _usersService UsersService
   * @param _postsService: PostsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _globalService: GlobalService,
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _usersService: UsersService,
    private _postsService: PostsService,
    private _customToastrService: CustomToastrService
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._searchFilter = this._generalService.getRouteParam('search-filter', this._activatedRoute);

    this._postId = parseInt(this._generalService.getRouteParam('post', this._activatedRoute) ?? '', undefined);
    this._getPosts();
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this.user = this._globalService._currentUser;
    }
  }

  /**
   * Delete event
   * @param postId number
   * @returns void
   */
  deleteAction(postId: number): void {
    const postItem = this.posts.find(post =>  post.id === postId);
    if (this.loggedIn && this._globalService._currentUser && postItem?.authorId === this._globalService._currentUser?.id) {
      this._postsService.delete(postId, this._globalService._currentUser.id).subscribe(
        (response: any) => {
          this._customToastrService.displaySuccessMessage(Messages.POST_DELETED_SUCCESSFULLY);
          this._onDeleteCommentAction(response.id);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Like post by id.
   * @param id number
   * @returns void
   */
  public like(id: number): void {
    if (this.loggedIn) {
      this._postsService.like(id).subscribe(
        (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
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
    if (this.loggedIn) {
      this._postsService.dislike(id).subscribe(
        (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
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
    this._getPosts(page);
  }

  /**
   * Search post by Title
   * @param search string
   * @returns void
   */
  public search(search: string): void {
    this.isLoaded = false;
    const model = {
      search: search,
      tag: this._searchFilter,
      sortParameters: null,
    };
    this._postsService.list(model).subscribe(
      (response: any) => {
        this.posts = response.posts;
        this.pageInfo = this.pageInfo;
        this.isLoaded = true;
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
        this.isLoaded = true;
      });
  }

  /**
   * Sort posts by parameter.
   * @returns void
   */
  public sort(): void {
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
    this._postsService.list(model).subscribe(
      (response: any) => {
        this.posts = response.posts;
        this.pageInfo = response.pageInfo;

      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });

  }

  /**
   * Get all posts.
   * @returns void
   */
  private _getPosts(page = 1): void {
    const sortParameters = {
      sortBy: null,
      orderBy: null,
      currentPage: page,
      pageSize: 10,
      displayType: null
    };
    const model = {
      search: null,
      tag: this._searchFilter,
      sortParameters: sortParameters
    };

    this._postsService.list(model)
      .subscribe(
        (response: PageViewDto) => {
          this.posts = response.posts;
          this.pageInfo = response.pageInfo;
          this.isLoaded = true;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          this.isLoaded = true;
        });
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
    }
    this.posts = this.posts;
    this.pageInfo.totalItems -= 1;
  }
}
