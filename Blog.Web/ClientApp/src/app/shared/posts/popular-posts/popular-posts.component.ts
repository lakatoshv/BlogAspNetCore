import { Component, OnInit } from '@angular/core';
import { PageViewDto } from 'src/app/core/Dto/PageViewDto';
import { PageInfo } from 'src/app/core/models/PageInfo';
import { Post } from 'src/app/core/models/Post';
import { PostService } from 'src/app/core/services/posts-services/post.service';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css']
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
   * @param _postsService PostService
   */
  constructor(
    private _postsService: PostService
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
        (error: any) => {
        });
  }
}
