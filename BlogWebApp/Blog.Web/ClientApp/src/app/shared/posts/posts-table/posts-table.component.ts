import { Component, OnInit, Input, Output, output, ChangeDetectionStrategy, ChangeDetectorRef } from "@angular/core";
import { PageViewDto } from "../../../core/Dto/PageViewDto";
import { PageInfo } from "../../../core/models/PageInfo";
import { Post } from "../../../core/models/Post";
import { ErrorResponse } from "../../../core/responses/ErrorResponse";
import { CustomToastrService } from "../../../core/services/custom-toastr.service";
import { PostsService } from "../../../core/services/posts-services/posts.service";
import { finalize } from "rxjs";

@Component({
  selector: 'app-posts-table',
  templateUrl: './posts-table.component.html',
  styleUrls: ['./posts-table.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostsTableComponent implements OnInit {
  /**
   * @param userId string | undefineds
   */
  @Input() userId: string | undefined;

  /**
   * @param postsCount EventEmitter<number>
   */
  readonly postsCount = output<number>();

  /**
   * @param posts Post[]
   */
  public posts: Post[] = [];

  /**
   * @param isLoaded boolean
   */
  public isLoaded = false;

  /**
   * @param pageInfo PageInfo
   */
  public pageInfo: PageInfo = {
    pageSize: 10,
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
    private _changeDetectorRef: ChangeDetectorRef) { }

  /** @inheritdoc */
  ngOnInit() {
    this._getPosts();
  }

  /**
   * Get all posts
   * @param page number
   * @returns void
   */
  private async _getPosts(page: number = 1): Promise<void> {
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
    if (this.userId != null) {
      this._postsService.userPosts(this.userId, model)
        .pipe(
          finalize(() => {
            this.isLoaded = true;
            this._changeDetectorRef.markForCheck();
          })
        )
        .subscribe({
          next: (response: any) => {
            this.posts = response.posts;
            this.pageInfo = response.pageInfo;
            this.postsCount.emit(this.pageInfo.totalItems);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    } else {
      this._postsService.list(model)
        .pipe(
          finalize(() => {
            this.isLoaded = true;
            this._changeDetectorRef.markForCheck();
          })
        )
        .subscribe({
          next: (response: any) => {
            this.posts = response.posts;
            this.pageInfo = response.pageInfo;
            this.postsCount.emit(this.pageInfo.totalItems);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }

    this.pageInfo.totalItems = this.posts.length;
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
