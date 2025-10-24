import { Component, OnInit } from "@angular/core";
import { PageInfo } from "../../../core/models/PageInfo";
import { Post } from "../../../core/models/Post";
import { CustomToastrService } from "../../../core/services/custom-toastr.service";
import { PageViewDto } from "../../../core/Dto/PageViewDto";
import { ErrorResponse } from "../../../core/responses/ErrorResponse";
import { PostsService } from "../../../core/services/posts-services/posts.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-admin-posts-table',
  templateUrl: './admin-posts-table.component.html',
  styleUrls: ['./admin-posts-table.component.css'],
  standalone: false
})
export class AdminPostsTableComponent implements OnInit {
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
   * @param _postsService: PostsService
   */
  constructor(
    private _router: Router,
    private _customToastrService: CustomToastrService,
    private _postsService: PostsService
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
   * Post pagination.
   * @param page number
   * @returns void
   */
  public paginate(page: number): void {
    this.pageInfo.pageNumber = page;
  }
}
