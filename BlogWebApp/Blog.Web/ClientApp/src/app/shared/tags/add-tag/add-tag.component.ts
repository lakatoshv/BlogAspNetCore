import { TagsService } from './../../../core/services/posts-services/tags.service';
import { Tag } from './../../../core/models/Tag';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { TagForm } from './../../../core/forms/posts/TagForm';
import { User } from './../../../core/models/User';
import { Router } from '@angular/router';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-add-tag',
  templateUrl: './add-tag.component.html',
  styleUrls: ['./add-tag.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddTagComponent implements OnInit {
  /**
   * @param tagForm FormGroup
   */
  public tagForm: FormGroup = new TagForm().tagForm;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param user User
   */
  public user: User | undefined;

  /**
   * @param _router Router
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
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
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }
  }

  /**
   * Add new tag.
   * 
   * @param tag Tag
   */
  public async add(tag: Tag): Promise<void> {
    if (this.tagForm.valid) {
      this._tagsService.add(...this.tagForm.value)
        .subscribe({
          next: (response: any) => {
            //this._customToastrService.displaySuccessMessage(Messages.TAG_CREATED_SUCCESSFULLY);
            this._router.navigate(['/admin/tags']);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });    
    }
  }
}
