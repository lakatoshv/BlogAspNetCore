import { TagsService } from './../../../core/services/posts-services/tags.service';
import { PostsService } from './../../../core/services/posts-services/posts.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { PostForm } from '../../../core/forms/posts/PostForm';
import { Router } from '@angular/router';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { User } from './../../../core/models/User';
import { TinyMCEOptionsObject } from './../../../core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from './../../../core/data/TinyMCEOptions';
import { Tag } from './../../../core/models/Tag';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css'],
  standalone: false
})
export class AddPostComponent implements OnInit {
  /**
   * @param tagInput ElementRef
   */
  @ViewChild('tag') tagInput: ElementRef | undefined;

  /**
   * @param postForm FormGroup
   */
  public postForm: FormGroup = new PostForm().postForm;

  /**
   * @param tagsList string[]
   */
  public tagsList: Tag[] = [];

  /**
   * @param availableTags Tag[]
   */
  public availableTags: Tag[] = [];

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param tagLabel string
   */
  public tagLabel = 'Додати новий тег';

  /**
   * @param action string
   */
  public action = 'add';

  /**
   * @param selectedTag object
   */
  public selectedTag: any = {
    value: '',
    id: null
  };

  /**
   * @param user User
   */
  public user: User | undefined;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _router Router
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _postsService PostsService,
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   */
  public constructor(
    private _router: Router,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _postsService: PostsService,
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  public ngOnInit() {
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
   *
   * @param tag string
   * @param action string
   */
  public tagAction(tag: string, action: string): void {
    if (action === 'add') { this.onAddTagAction(tag); }
    if (action === 'edit') { this.onEditTagAction(tag); }
  }

  /**
   * Add tag event.
   */
  public addTagLabel(): void {
    this.clearFormData();
  }
  /**
   * Edit tag event.
   * 
   * @param tag string
   */
  public editTag(tag: Tag): void {
    this.selectedTag['value'] = tag.title;
    this.selectedTag['id'] = this.tagsList.indexOf(tag);
    this.action = 'edit';
    this.tagLabel = 'Редагувати тег';
  }

  /**
   * Add tag event.
   * 
   * @param tag string
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
   * Edit tag event.
   * 
   * @param tag string
   */
  onEditTagAction(tag: any): void {
    const index = this.selectedTag['id'];
    if (index && index > -1) {
      this.tagsList[index].title = tag;
      this.clearFormData();
    }
  }

  /**
   * Delete tag event.
   * 
   * @param tag any
   */
  onDeleteTagAction(tag: any): void {
    const index = this.tagsList.indexOf(tag);
    if (index > -1) {
      this.tagsList.splice(index, 1);
    }
  }

  /**
   * Add new post.
   * 
   * @param post Post
   */
  add() {
    if (this.postForm.valid) {
      this.postForm.value.id = 0;
      this.postForm.value.tags = this.tagsList;
      this.postForm.value.authorId = this.user?.id;
      this._postsService.add({
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
    this.selectedTag['id'] = undefined;
    if(this.tagInput) {
      this.tagInput.nativeElement.value = '';
    }
  }
}
