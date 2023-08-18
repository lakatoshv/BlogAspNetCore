import { Component } from '@angular/core';
import { PageViewDto } from 'src/app/core/Dto/PageViewDto';
import { Messages } from 'src/app/core/data/Messages';
import { Tag } from 'src/app/core/models/Tag';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { TagsService } from 'src/app/core/services/posts-services/tags.service';

@Component({
  selector: 'app-admin-tags-table',
  templateUrl: './admin-tags-table.component.html',
  styleUrls: ['./admin-tags-table.component.scss']
})
export class AdminTagsTableComponent {
  public tags: Tag[] = [];

  /**
   * @param isLoaded boolean
   */
  isLoaded: boolean = false;

  /**
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._getTags();
  }

  /**
   * Get all tags.
   * @param page number
   */
  private _getTags(page: number = 0): void {
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
    this._tagsService.list(model)
    .subscribe(
      (response: any) => {
        this.tags = response.tags;
        this.isLoaded = true;
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
        this.isLoaded = true;
      });;
  }

  /**
   * Delete tag event.
   * @param tag Tag
   * @returns void
   */
  deleteAction(tag: Tag): void {
    //this._customToastrService.displaySuccessMessage(Messages.TAG_DELETED_SUCCESSFULLY);
  }
}
