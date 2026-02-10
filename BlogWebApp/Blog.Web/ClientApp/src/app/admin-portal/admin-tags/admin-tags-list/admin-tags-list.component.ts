import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-tags-list',
  templateUrl: './admin-tags-list.component.html',
  styleUrls: ['./admin-tags-list.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminTagsListComponent implements OnInit {
  /**
   * @inheritdoc
   */
  ngOnInit(): void {
  }

  /**
   * @inheritdoc
   */
  constructor() {
  }
}
