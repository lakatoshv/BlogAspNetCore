import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PageViewDto } from 'src/app/core/Dto/PageViewDto';
import { PageInfo } from 'src/app/core/models/PageInfo';
import { Post } from 'src/app/core/models/Post';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { PostService } from 'src/app/core/services/posts-services/post.service';

@Component({
  selector: 'app-admin-posts-table',
  templateUrl: './admin-posts-table.component.html',
  styleUrls: ['./admin-posts-table.component.scss']
})
export class AdminPostsTableComponent {
  /**
   * @param posts Post[]
   */
  posts: Post[] = [];

  postsUrl = "";

  /**
   * @param isLoaded boolean
   */
  isLoaded: boolean = false;

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param _router Router
   * @param _customToastrService CustomToastrService
   * @param _postService: PostService
   */
  constructor(
    private _router: Router,
    private _customToastrService: CustomToastrService,
    private _postService: PostService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.postsUrl = this._router.url.includes('posts') ? "/admin/posts" : "posts";
    this._getPosts();
  }

  /**
   * Change post status action.
   * @param postId number
   * @param status string
   */
  onChangeStatusAction(postId: number, status: string) {}

  /**
   * Delete event
   * @param postId number
   * @returns void
   */
  public deleteAction(postId: number): void {
    /*const index = this.posts.findIndex(x => x.Id === postId);
    if (index > -1) {
      this.posts.splice(index, 1);
      const comments = Comments.filter(comment => comment.PostId === postId).forEach(comment => {
        Comments.splice(comment.Id, 1);
      });
      this._customToastrService.displaySuccessMessage(Messages.POST_DELETED_SUCCESSFULLY);
      this.posts = this.posts;
    }*/
  }

  /**
   * Get all posts
   */
  private _getPosts(page = 1): void {
    debugger
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
   * Post pagination.
   * @param page number
   * @returns void
   */
  public paginate(page: number): void {
    this.pageInfo.pageNumber = page;
  }
}
