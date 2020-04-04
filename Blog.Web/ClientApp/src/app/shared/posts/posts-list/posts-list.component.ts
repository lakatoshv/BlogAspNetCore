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
   */
  constructor(
    private _globalService: GlobalService,
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _usersService: UsersService,
    private _postService: PostService
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
   * Like post event.
   * @param id number
   * @returns void
   */
  public like(id: number): void {
    this.posts[id].likes += 1;
  }

  /**
   * Dislike post event.
   * @param id number
   * @returns void
   */
  public dislike(id: number): void {
    this.posts[id].dislikes += 1;
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
      (error: any) => {
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
      error => {
      }
    );

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
        (error: any) => {
          this.isLoaded = true;
        });
  }
}
