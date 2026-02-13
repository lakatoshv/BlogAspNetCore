import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-right-sidebar',
  templateUrl: './right-sidebar.component.html',
  styleUrls: ['./right-sidebar.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RightSidebarComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
