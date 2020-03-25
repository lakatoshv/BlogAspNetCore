import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-layout-component',
  templateUrl: './layout-component.component.html',
  styleUrls: ['./layout-component.component.css']
})
export class LayoutComponentComponent implements OnInit {
  /**
   * @param navbarOpen boolean
   */
  navbarOpen = false;

  /**
   * @param loggedIn boolean
   */
  loggedIn = false;

  constructor(
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
  }

  /**
   * Toggle navbar menu
   */
  toggleNavbar() {
    this.navbarOpen = !this.navbarOpen;
  }
}
