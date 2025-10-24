import { PostsService } from './../../../core/services/posts-services/posts.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { PostForm } from '../../../core/forms/posts/PostForm';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { User } from './../../../core/models/User';
import { Post } from './../../../core/models/Post';
import { TinyMCEOptionsObject } from './../../../core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from './../../../core/data/TinyMCEOptions';
import { Tag } from './../../../core/models/Tag';
import { TagsService } from './../../../core/services/posts-services/tags.service';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { Messages } from './../../../core/data/Mesages';
import { SelectedTag } from '../../../core/models/SelectedTag';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css'],
  standalone: false
})
export class EditPostComponent implements OnInit {
  /**
   * @param tagInput ElementRef | undefined
   */
  @ViewChild('tag') tagInput: ElementRef | undefined;

  /**
   * @param postForm FormGroup
   */
  postForm: FormGroup = new PostForm().postForm;

  /**
   * @param post Post
   */
  post: Post | undefined;

  /**
   * @param availableTags Tag[]
   */
  public availableTags: Tag[] = [];

  /**
   * @param isLoggedIn boolean
   */
  isLoggedIn = false;

  /**
   * @param isCurrentUserPost boolean
   */
  isCurrentUserPost = false;

  /**
   * @param isLoadEdit boolean
   */
  isLoadEdit = false;

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
  selectedTag: SelectedTag = {
    value: '',
    id: undefined
  };

  /**
   * @param user User
   */
  user: User | undefined;

  /**
   * @param _postId number
   */
  private _postId: number | undefined;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _activatedRoute ActivatedRoute
   * @param _router Router
   * @param _postsService PostsService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   * @param _tagsService TagsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _postsService: PostsService,
    private _usersService: UsersService,
    private _globalService: GlobalService,
    private _tagsService: TagsService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._postId = parseInt(this._globalService.getRouteParam('post-id', this._activatedRoute) ?? '', undefined);

    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }
    this._getPost();
    this._getTags();
  }

  /**
   * Check if param is string.
   * @param x any
   * @returns boolean
   */
  public isString(x: any): boolean {
    return Object.prototype.toString.call(x) === '[object String]';
  }

  /**
   * Add/Edit tag action.
   *
   * @param tag string
   * @param action string
   * @returns void
   */
  tagAction(tag: string, action: string): void {
    if (action === 'add') { this.onAddTagAction(tag); }
    if (action === 'edit') { this.onEditTagAction(tag); }
  }

  /**
   * Edit post event.
   *
   * @param post Post
   * @returns void
   */
  edit(post: Post): void {
    if (this.isCurrentUserPost 
      && this.postForm.valid
      && this._postId
      && this.user) {
      post.id = this._postId;
      post.tags = this.post?.tags;
      post.authorId = this.user?.id;
      this._postsService.edit(this._postId, post).subscribe(
        () => {
          this._customToastrService.displaySuccessMessage(Messages.POST_EDITED_SUCCESSFULLY);
          this._router.navigate(['/blog/post/' + this._postId]);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Delete post action.
   */
  deleteAction(): void {
    if (this.isCurrentUserPost && this._postId && this._globalService._currentUser) {
      this._postsService.delete(this._postId, this._globalService._currentUser.id).subscribe(
        () => {
          this._customToastrService.displaySuccessMessage(Messages.POST_DELETED_SUCCESSFULLY);
          this._router.navigate(['/blog']);
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
    this._router.navigate(['/blog/post/' + this._postId]);
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
    this.selectedTag['id'] = this.post?.tags?.indexOf(tag);
    this.action = 'edit';
    this.tagLabel = 'Редагувати тег';
  }

  /**
   * Add tag event
   * @param tag string
   * @returns void
   */
  onAddTagAction(tag: string): void {
    if (tag !== '' && this.availableTags.findIndex(x => x.title === tag) !== -1) {
      const index = this.availableTags.findIndex(x => x.title === tag);
      if (this.post?.tags && index > -1) {
        this.post.tags = this.post?.tags === null ? [] : this.post?.tags;
        this.post.tags.unshift(this.availableTags[index]);
        this._removeFromAvailableTags(this.availableTags[index]);
      }
    } else {
      this.post?.tags?.unshift(new Tag(0, tag));
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
    if (this.post?.tags && index && index > -1) {
      this.post.tags[index].title = tag;
      this.clearFormData();
    }
  }

  /**
   * Delete tag event
   * @param tag any
   */
  onDeleteTagAction(tag: any): void {
    const index = this.post?.tags?.indexOf(tag);
    if (index && index > -1) {
      this.post?.tags?.splice(index, 1);
    }
  }

  /**
   * Get post.
   * @returns void
   */
  private _getPost(): void {
    if(this._postId) {
      this._postsService.showPost(this._postId).subscribe(
        (response: any) => {
          this.post = response.post;
          if(this.post) {
            this.post.tags = response.tags;
            if (! this.isLoggedIn || this.user?.id !== this.post.authorId) {
              this._router.navigateByUrl('/');
            }
            if (this.user?.id === this.post.authorId) {
              this.isCurrentUserPost = true;
            }
            this._setFormData();
          }
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
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
   * Set edit form data.
   * @returns void
   */
  private _setFormData(): void {
    // this.postForm.get('id').setValue(this.post.Id)
    this.postForm.get('title')?.setValue(this.post?.title);
    this.postForm.get('description')?.setValue(this.post?.description);
    this.postForm.get('content')?.setValue(this.post?.content);
    this.postForm.get('imageUrl')?.setValue(this.post?.imageUrl);
    this.isLoadEdit = true;
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
