import { Messages } from './../../../core/data/Messages';
import { ErrorResponse } from './../../../core/responses/ErrorResponse';
import { TagsService } from './../../../core/services/posts-services/tags.service';
import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { PostForm } from '../../../core/forms/posts/PostForm';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Tag } from 'src/app/core/models/Tag';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css']
})
export class AddPostComponent implements OnInit {
  /**
   * @param tagInput ElementRef
   */
  @ViewChild('tag') tagInput: ElementRef;

  /**
   * @param postForm FormGroup
   */
  postForm: FormGroup = new PostForm().postForm;

  /**
   * @param tagsList string[]
   */
  tagsList: Tag[] = [];

  /**
   * @param availableTags Tag[]
   */
  public availableTags: Tag[] = [];

  /**
   * @param isLoggedIn boolean
   */
  isLoggedIn = false;

  /**
   * @param tagLabel string
   */
  tagLabel = 'Додати новий тег';

  /**
   * @param action string
   */
  action = 'add';

  /**
   * @param selectedTag object
   */
  selectedTag: object = {
    value: '',
    id: null
  };

  /**
   * @param user User
   */
  user: User;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _router Router
   * @param _postService: PostService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _router: Router,
    private _postService: PostService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this.isLoggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }

    this._getTags();
  }

  /**
   * Add/Edit tag action.
   * @param tag string
   * @param action string
   * @returns void
   */
  tagAction(tag: string, action: string): void {
    if (action === 'add') { this.onAddTagAction(tag); }
    if (action === 'edit') { this.onEditTagAction(tag); }
  }

  /**
   * Add tag event
   * @returns void
   */
  addTagLabel(): void {
    this.clearFormData();
  }

  /**
   * Edit tag event
   * @param tag string
   * @returns void
   */
  editTag(tag: Tag): void {
    this.selectedTag['value'] = tag.title;
    this.selectedTag['id'] = this.tagsList.indexOf(tag);
    this.action = 'edit';
    this.tagLabel = 'Редагувати тег';
  }

  /**
   * Add tag event
   * @param tag string
   * @returns void
   */
  onAddTagAction(tag: string): void {
    if (tag !== '' && this.tagsList.findIndex(x => x.title === tag) === -1) {
      const index = this.availableTags.findIndex(x => x.title === tag);
      if (index > -1) {
        this.tagsList.unshift(this.availableTags[index]);
        this._removeFromAvailableTags(this.availableTags[index]);
      } else {
        this.tagsList.unshift(new Tag(0, tag));
      }
    }
    this.clearFormData();
  }

  /**
   * Edit tag event
   * @param tag any
   * @returns void
   */
  onEditTagAction(tag: any): void {
    const index = this.selectedTag['id'];
    if (index > -1) {
      this.tagsList[index].title = tag;
      this.clearFormData();
    }
  }

  /**
   * Delete tag event
   * @param tag any
   */
  onDeleteTagAction(tag: any): void {
    const index = this.tagsList.indexOf(tag);
    if (index > -1) {
      this.tagsList.splice(index, 1);
    }
  }

  /**
   * Add new post
   * @returns void
   */
  add() {
    if (this.postForm.valid) {
      this.postForm.value.id = 0;
      this.postForm.value.tags = this.tagsList;
      this.postForm.value.authorId = this.user.id;
      this._postService.add({
        ...this.postForm.value
      }).subscribe(
        () => {
          this._customToastrService.displaySuccessMessage(Messages.POST_CREATED_SUCCESSFULLY);
          this._router.navigate(['/']);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Back button.
   */
  backButton(): void {
    this._router.navigate(['/']);
  }

  /**
   * Get available tags.
   * @returns void
   */
  private _getTags(): void {
    this._tagsService.list().subscribe(
      (response: Tag[]) => {
        this.availableTags = response;
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }

  /**
   * Remove selected tag from available tags.
   * @param tag Tag
   * @returns void
   */
  private _removeFromAvailableTags(tag: Tag): void {
    const index = this.availableTags.indexOf(tag);
    if (index > -1) {
      this.availableTags.splice(index, 1);
    }
  }

  /**
   * Clear form data.
   * @returns void
   */
  private clearFormData(): void {
    this.tagLabel = 'Додати новий тег';
    this.action = 'add';
    this.selectedTag['value'] = '';
    this.selectedTag['id'] = null;
    this.tagInput.nativeElement.value = '';
  }
}
