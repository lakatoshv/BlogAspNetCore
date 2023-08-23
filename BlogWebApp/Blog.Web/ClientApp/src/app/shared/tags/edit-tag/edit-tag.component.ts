import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TagForm } from 'src/app/core/forms/posts/TagForm';
import { Tag } from 'src/app/core/models/Tag';
import { User } from 'src/app/core/models/User';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { TagsService } from 'src/app/core/services/posts-services/tags.service';
import { UsersService } from 'src/app/core/services/users-services/users.service';

@Component({
  selector: 'app-edit-tag',
  templateUrl: './edit-tag.component.html',
  styleUrls: ['./edit-tag.component.scss']
})
export class EditTagComponent {
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
   */
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService
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
  public edit(tag: Tag): void {
    if (this.tagForm.valid && this.tag && this._tagId) {
      this.tag.title = tag['title'];
      this._tagsService.edit(this._tagId, this.tag)
        .subscribe(
          () => {
            //this._customToastrService.displaySuccessMessage(Messages.TAG_EDITED_SUCCESSFULLY);
            this._router.navigate(['/admin/tags']);
          },
          (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
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
  private _getTag(): void {
    if(this._tagId) {
      this._tagsService.getTag(this._tagId)
        .subscribe(
          (response: any) => {
            this.tag = response;
            this._setFormData();
          },
          (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
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
