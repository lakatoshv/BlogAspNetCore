import { PostService } from './../../../core/services/posts-services/post.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Location } from '@angular/common';
import { PostForm } from '../../../core/forms/posts/PostForm';
import { Post } from '../../../core/models/Post';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { User } from 'src/app/core/models/User';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { UsersService } from 'src/app/core/services/users-services/users.service';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css']
})
export class AddPostComponent implements OnInit {
  /**
   * @param postForm FormGroup
   */
  postForm: FormGroup = new PostForm().postForm;

  /**
   * @param tagsList string[]
   */
  tagsList: string[] = [];

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
   */
  constructor(
    private _router: Router,
    private _postService: PostService,
    private _usersService: UsersService,
    private _globalService: GlobalService
  ) { }

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
  editTag(tag: string): void {
    this.selectedTag['value'] = tag;
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
    this.tagsList.unshift(tag);
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
      this.tagsList[index] = tag;
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
    this.postForm.value.id = 0;
    this.postForm.value.tags = this.tagsList.join(', ');
    this.postForm.value.authorId = this.user.id;
    this._postService.add({
      ...this.postForm.value
    }).subscribe(
      () => {
        this._router.navigate(['/']);
      },
      (errorMessage) => {
      });
  }

  /**
   * Back button.
   */
  backButton(): void {
    this._router.navigate(['/']);
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
