import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Tag } from './../../../core/models/Tag';
import { TagsService } from './../../../core/services/posts-services/tags.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { CustomToastrService } from '../../../core/services/custom-toastr.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-popular-tags',
  templateUrl: './popular-tags.component.html',
  styleUrls: ['./popular-tags.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PopularTagsComponent implements OnInit {
  public tags: Tag[] = [];

  constructor(
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef) {
  }

  ngOnInit() {
    this._getTags();
  }

  /**
   * Get all tags.
   * @returns void
   */
  private async _getTags(page = 1): Promise<void> {
    const sortParameters = {
      sortBy: null,
      orderBy: null,
      currentPage: page,
      pageSize: 5,
      displayType: null
    };
    const model = {
      search: null,
      sortParameters: sortParameters
    };

    this._tagsService.list(model)
      .pipe(
        finalize(() => {
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
}
