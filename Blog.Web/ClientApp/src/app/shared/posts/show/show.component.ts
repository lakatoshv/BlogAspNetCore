import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';
import { GeneralServiceService } from 'src/app/core';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { Post } from 'src/app/core/models/Post';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { PageInfo } from 'src/app/core/models/PageInfo';

@Component({
  selector: 'app-show',
  templateUrl: './show.component.html',
  styleUrls: ['./show.component.css']
})
export class ShowComponent implements OnInit {
  /**
   * @param post Post
   */
  public post: Post;

  /**
   * @param user User
   */
  public user: User;

  /**
   * @param loggedIn boolean
   */
  public loggedIn = false;

  /**
   * @param postId number
   */
  public postId: number;

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
   */
  constructor(
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
    private _postService: PostService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _router: Router
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.postId = parseInt(this._generalService.getRoutePeram('post-id', this._activatedRoute), null);
    this._getPost();
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
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
    this._postService.showPost(this.postId).subscribe(
      (response: any) => {
        this.post = response.post;
        this.post.comments = response.comments;
        this.post.tagsList = response.post.tags.split(', ');
        this.pageInfo = response.comments.pageInfo;
      },
      () => {}
    );
  }

}
