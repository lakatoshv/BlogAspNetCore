import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-posts-list',
  templateUrl: './admin-posts-list.component.html',
  styleUrls: ['./admin-posts-list.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminPostsListComponent implements OnInit {
  /**
   * @inheritdoc
   */
  constructor() { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
  }
}
