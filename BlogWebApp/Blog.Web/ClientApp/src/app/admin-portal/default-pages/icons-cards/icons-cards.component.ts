import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-icons-cards',
  templateUrl: './icons-cards.component.html',
  styleUrls: ['./icons-cards.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IconsCardsComponent implements OnInit {
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
