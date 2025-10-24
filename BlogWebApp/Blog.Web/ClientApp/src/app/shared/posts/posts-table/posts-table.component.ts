import { Component, OnInit, Input, Output, output } from "@angular/core";
import EventEmitter from "events";
import { PageViewDto } from "../../../core/Dto/PageViewDto";
import { PageInfo } from "../../../core/models/PageInfo";
import { Post } from "../../../core/models/Post";
import { ErrorResponse } from "../../../core/responses/ErrorResponse";
import { CustomToastrService } from "../../../core/services/custom-toastr.service";
import { PostsService } from "../../../core/services/posts-services/posts.service";

@Component({
  selector: 'app-posts-table',
  templateUrl: './posts-table.component.html',
  styleUrls: ['./posts-table.component.css'],
  standalone: false
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
   */
  constructor(
    private _postsService: PostsService,
    private _customToastrService: CustomToastrService) { }

  /** @inheritdoc */
  ngOnInit() {
    this._getPosts();
  }

  /**
   * Get all posts
   * @param page number
   * @returns void
   */
  private _getPosts(page: number = 1): void {
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
      this._postsService.userPosts(this.userId, model).subscribe(
        (response: any) => {
          this.posts = response.posts;
          this.pageInfo = response.pageInfo;
          this.postsCount.emit(this.pageInfo.totalItems);
          this.isLoaded = true;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          this.isLoaded = true;
        });
    } else {
      this._postsService.list(model)
        .subscribe(
          (response: PageViewDto) => {
            this.posts = response.posts;
            this.pageInfo = response.pageInfo;
            this.postsCount.emit(this.pageInfo.totalItems);
            this.isLoaded = true;
          },
          (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
            this.isLoaded = true;
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
