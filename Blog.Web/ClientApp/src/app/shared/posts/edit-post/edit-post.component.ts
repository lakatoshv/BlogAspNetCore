import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { PostForm } from '../../../core/forms/posts/PostForm';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { Post } from 'src/app/core/models/Post';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { UsersService } from 'src/app/core/services/users-services/users.service';

@Component({
  selector: 'app-edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']
})
export class EditPostComponent implements OnInit {
  /**
   * @param postForm FormGroup
   */
  postForm: FormGroup = new PostForm().postForm;

  /**
   * @param post Post
   */
  post: Post;

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
  selectedTag: object = {
    value: '',
    id: null
  };

  /**
   * @param user User
   */
  user: User;

  /**
   * @param _postId number
   */
  private _postId: number;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _activatedRoute ActivatedRoute
   * @param _router Router
   * @param _postService PostService
   * @param _usersService UsersService
   * @param _globalService GlobalService
   */
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _postService: PostService,
    private _usersService: UsersService,
    private _globalService: GlobalService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._postId = parseInt(this._globalService.getRouteParam('post-id', this._activatedRoute), null);

    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    } else {
      this._router.navigateByUrl('/authorization');
    }
    this._getPost();
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
    if (this.isCurrentUserPost) {
      post.id = this._postId;
      post.tags = this.post.tagsList.join(', ');
      post.authorId = this.user.id;
      this._postService.edit(this._postId, post).subscribe(
        () => {
          this._router.navigate(['/blog/post/' + this._postId]);
        },
        (error) => {}
      );
    }
  }

  /**
   * Delete post action.
   */
  deleteAction(): void {
    if (this.isCurrentUserPost) {
      this._postService.delete(this._postId, this._globalService._currentUser.id).subscribe(
        () => {
          this._router.navigate(['/blog']);
        },
        () => {}
      );
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
  editTag(tag: string): void {
    this.selectedTag['value'] = tag;
    this.selectedTag['id'] = this.post.tagsList.indexOf(tag);
    this.action = 'edit';
    this.tagLabel = 'Редагувати тег';
  }

  /**
   * Add tag event
   * @param tag string
   * @returns void
   */
  onAddTagAction(tag: string): void {
    this.post.tagsList.unshift(tag);
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
      this.post.tagsList[index] = tag;
      this.clearFormData();
    }
  }

  /**
   * Delete tag event
   * @param tag any
   */
  onDeleteTagAction(tag: any): void {
    const index = this.post.tagsList.indexOf(tag);
    if (index > -1) {
      this.post.tagsList.splice(index, 1);
    }
  }

  /**
   * Get post.
   * @returns void
   */
  private _getPost(): void {
    this._postService.showPost(this._postId).subscribe(
      (response: any) => {
        this.post = response.post;
        if(! this.isLoggedIn || this.user.id !== this.post.authorId){
          this._router.navigateByUrl('/');
        }
        if(this.user.id === this.post.authorId) {
          this.isCurrentUserPost = true;
        }

        this.post.tagsList = this.post.tags.split(', ');
        this._setFormData();
      },
      () => {}
    );
  }

  /**
   * Set edit form data.
   * @returns void
   */
  private _setFormData(): void {
    // this.postForm.get('id').setValue(this.post.Id)
    this.postForm.get('title').setValue(this.post.title);
    this.postForm.get('description').setValue(this.post.description);
    this.postForm.get('content').setValue(this.post.content);
    this.postForm.get('imageUrl').setValue(this.post.imageUrl);
    this.isLoadEdit = true;
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
  }
}
