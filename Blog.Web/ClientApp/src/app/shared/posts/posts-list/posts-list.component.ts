import { Component, OnInit } from '@angular/core';
import { GeneralServiceService } from 'src/app/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { FormGroup } from '@angular/forms';
import { SearchForm } from 'src/app/core/forms/SearchForm';
import {debounceTime} from 'rxjs/operators';
import { PageInfo } from 'src/app/core/models/PageInfo';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { PostService } from 'src/app/core/services/posts-services/post.service';
import { PageViewDto } from 'src/app/core/Dto/PageViewDto';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { Messages } from 'src/app/core/data/Messages';

@Component({
  selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.css']
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
   * @param user User
   */
  public user: User;

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param _postId number
   */
  private _postId: number;

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
   * @param _globalService GlobalService
   * @param _generalService GeneralServiceService
   * @param _activatedRoute ActivatedRoute
   * @param _usersService UsersService
   * @param _postService: PostService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _globalService: GlobalService,
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _usersService: UsersService,
    private _postService: PostService,
    private _customToastrService: CustomToastrService
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._postId = parseInt(this._generalService.getRoutePeram('post', this._activatedRoute), null);
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
    if (this.loggedIn && postItem.authorId === this._globalService._currentUser.id) {
      this._postService.delete(postId, this._globalService._currentUser.id).subscribe(
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
      this._postService.like(id).subscribe(
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
      this._postService.dislike(id).subscribe(
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
      sortParameters: null,
    };
    this._postService.list(model).subscribe(
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
      sortParameters: sortParameters,
    };
    this._postService.list(model).subscribe(
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
      sortParameters: sortParameters
    };

    this._postService.list(model)
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
