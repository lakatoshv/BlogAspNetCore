import { Component, OnInit } from '@angular/core';
import { Tag } from 'src/app/core/models/Tag';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { TagsService } from 'src/app/core/services/posts-services/tags.service';

@Component({
  selector: 'app-popular-tags',
  templateUrl: './popular-tags.component.html',
  styleUrls: ['./popular-tags.component.css']
})
export class PopularTagsComponent implements OnInit {
  public tags: Tag[] = [];
  constructor(
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService) {
  }

  ngOnInit() {
    this._getTags();
  }

  /**
   * Get all tags.
   * @returns void
   */
  private _getTags(page = 1): void {
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
      .subscribe(
        (response: any) => {
          this.tags = response.tags;
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
  }
}
