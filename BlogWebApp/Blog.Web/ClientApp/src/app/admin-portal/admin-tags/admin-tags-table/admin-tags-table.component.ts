import { TagsService } from '../../../core/services/posts-services/tags.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Tag } from '../../../core/models/Tag';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-admin-tags-table',
  templateUrl: './admin-tags-table.component.html',
  styleUrls: ['./admin-tags-table.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminTagsTableComponent implements OnInit {
  public tags: Tag[] = [];

  /**
   * @param isLoaded boolean
   */
  isLoaded: boolean = false;

  /**
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
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
  private async _getTags(page: number = 0): Promise<void> {
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
      .pipe(
            finalize(() => {
              this.isLoaded = true;
              this._changeDetectorRef.markForCheck();
            })
          )
          .subscribe({
            next: (response: any) => {
              this.tags = response.tags;
            },
            error: (error: ErrorResponse) => {
              this._customToastrService.displayErrorMessage(error);
            }
          });
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
