import { Component, OnInit } from '@angular/core';
import { Post } from './../../../core/models/Post';
import { PostsService } from './../../../core/services/posts-services/posts.service';
import { PageViewDto } from '../../../core/Dto/PageViewDto';
import { PageInfo } from '../../../core/models/PageInfo';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css'],
  standalone: false
})
export class PopularPostsComponent implements OnInit {
  /**
   * @param posts Post[]
   */
  public posts: Post[] = [];

  /**
   * @param sortBy string
   */
  public sortBy = 'Likes';

  /**
   * @param orderBy string
   */
  public orderBy = 'asc';

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 5,
    pageNumber: 0,
    totalItems: 0
  };

  /**
   * @param _postsService PostsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _postsService: PostsService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._getPosts();
  }

  /**
   * Get all posts.
   * @returns void
   */
  private _getPosts(page = 1): void {
    const sortParameters = {
      sortBy: this.sortBy,
      orderBy: this.orderBy,
      currentPage: page,
      pageSize: 5,
      displayType: null
    };
    const model = {
      search: null,
      sortParameters: sortParameters
    };

    this._postsService.list(model)
      .subscribe(
        (response: PageViewDto) => {
          this.posts = response.posts;
          this.pageInfo = response.pageInfo;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
  }
}
