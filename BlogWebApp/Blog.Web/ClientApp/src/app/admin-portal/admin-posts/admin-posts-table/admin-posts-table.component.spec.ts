import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPostsTableComponent } from './admin-posts-table.component';

describe('AdminPostsTableComponent', () => {
  let component: AdminPostsTableComponent;
  let fixture: ComponentFixture<AdminPostsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminPostsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPostsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
