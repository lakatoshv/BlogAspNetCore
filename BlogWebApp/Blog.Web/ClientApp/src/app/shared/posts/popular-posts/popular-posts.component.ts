import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Post } from './../../../core/models/Post';
import { PostsService } from './../../../core/services/posts-services/posts.service';
import { PageViewDto } from '../../../core/Dto/PageViewDto';
import { PageInfo } from '../../../core/models/PageInfo';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _postsService: PostsService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
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
  private async _getPosts(page = 1): Promise<void> {
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
      .pipe(
        finalize(() => {
          this._changeDetectorRef.markForCheck();
        })
      )
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
