import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { TagForm } from './../../../core/forms/posts/TagForm';
import { User } from './../../../core/models/User';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { TagsService } from './../../../core/services/posts-services/tags.service';
import { Tag } from './../../../core/models/Tag';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-edit-tag',
  templateUrl: './edit-tag.component.html',
  styleUrls: ['./edit-tag.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EditTagComponent implements OnInit {
  /**
   * @param tagForm FormGroup
   */
  public tagForm: FormGroup = new TagForm().tagForm;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param tag Tag
   */
  public tag: Tag | undefined;

  /**
   * @param user User
   */
  public user: User | undefined;

  /**
   * @param _tagId number
   */
  private _tagId: number | undefined;

  /**
   * @param _activatedRoute ActivatedRoute
   * @param _router Router
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    const postIdStr = this._globalService.getRouteParam('post-id', this._activatedRoute)
    if(postIdStr !== null) {
      this._tagId = parseInt(postIdStr, undefined);
    }

    this._activatedRoute.params.subscribe(
      (params: Params) => {
        this._tagId = parseInt(params['post-id'], undefined);
        this._checkIfUserIsLoggedIn();

        this._getTag();
      }
    );

    this._checkIfUserIsLoggedIn();
  }

  /**
   * Add new tag.
   * 
   * @param tag Tag
   */
  public async edit(tag: Tag): Promise<void> {
    if (this.tagForm.valid && this.tag && this._tagId) {
      this.tag.title = tag['title'];
      this._tagsService.edit(this._tagId, this.tag)
        .subscribe({
          next: (response: any) => {
            //this._customToastrService.displaySuccessMessage(Messages.TAG_EDITED_SUCCESSFULLY);
            this._router.navigate(['/admin/tags']);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Set edit form data.
   */
  private _setFormData(): void {
    this.tagForm.get('title')?.setValue(this.tag?.title);
  }

  /**
   * Get tag by id.
   */
  private async _getTag(): Promise<void> {
    if(this._tagId) {
      this._tagsService.getTag(this._tagId)
        .pipe(
          finalize(() => {
            this._changeDetectorRef.markForCheck();
          })
        )
        .subscribe({
          next: (response: any) => {
            this.tag = response;
            this._setFormData();
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Check if user is logged in.
   */
  private _checkIfUserIsLoggedIn(): void {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }
  }
}
