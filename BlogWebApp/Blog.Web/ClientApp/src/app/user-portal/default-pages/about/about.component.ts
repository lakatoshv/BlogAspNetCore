import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AboutComponent implements OnInit {
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
