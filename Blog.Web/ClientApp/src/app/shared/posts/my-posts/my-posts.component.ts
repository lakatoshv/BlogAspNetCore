import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit } from '@angular/core';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { SearchForm } from 'src/app/core/forms/SearchForm';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from 'src/app/core/models/User';
import { Post } from 'src/app/core/models/Post';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { PageInfo } from 'src/app/core/models/PageInfo';

@Component({
  selector: 'app-my-posts',
  templateUrl: './my-posts.component.html',
  styleUrls: ['./my-posts.component.css']
})
export class MyPostsComponent implements OnInit {
  /**
   * @param posts Post[]
   */
  public posts: Post[] = [];

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
   * @param _userId number
   */
  private _userId: string;

  /**
   * @param _globalService GlobalService
   * @param _router Router
   * @param _activatedRoute ActivatedRoute
   * @param _usersService UsersService
   */
  constructor(
    private _globalService: GlobalService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _postService: PostService,
    private _usersService: UsersService
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this.isLoggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }

    if (this._router.url.includes('/my-posts')) {
      this._userId = this.user.id;
      this.isCurrentUserPosts = true;
    } else {
      this._userId = this._globalService.getRouteParam('user-id', this._activatedRoute);
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
  public deleteAction(postId: number): void {
    const post = this.posts.find(p =>  p.id === postId);
    if (this.isLoggedIn && this.posts[postId].author.id === this.user.id) {
      this._postService.delete(postId, this._globalService._currentUser.id).subscribe(
        (response: any) => {
          this._onDeleteCommentAction(response.id);
        },
        (errorMessage) => {
        }
      );
    }
  }

  /**
   * Like post by id.
   * @param id number
   * @returns void
   */
  public like(id: number): void {
    if (this.isLoggedIn) {
      this._postService.like(id).subscribe(
        (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
        },
        (error) => {
        }
      );
    }
  }

  /**
   * Dislike post by id.
   * @param id number
   * @returns void
   */
  public dislike(id: number): void {
    if (this.isLoggedIn) {
      this._postService.dislike(id).subscribe(
        (response: any) => {
          const ind = this.posts.findIndex(post =>  post.id === id);
          this.posts[ind] = response;
          this.posts = this.posts;
        },
        (error) => {
        }
      );
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
  search(search: string): void {
    this.isLoaded = false;
    const model = {
      search: search,
      sortParameters: null,
    };
    this._postService.userPosts(this._userId, model).subscribe(
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
  sort() {
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
    this._postService.userPosts(this._userId, model).subscribe(
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
      displayType: null,
    };
    const model = {
      search: null,
      sortParameters: sortParameters,
    };
    this._postService.userPosts(this._userId, model).subscribe(
      (response: any) => {
        this.posts = response.posts;
        this.pageInfo = response.pageInfo;
        this.isLoaded = true;
      },
      (error) => {}
    );

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
