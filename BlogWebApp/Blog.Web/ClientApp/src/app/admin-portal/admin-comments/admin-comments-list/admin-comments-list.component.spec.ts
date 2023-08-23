import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCommentsListComponent } from './admin-comments-list.component';

describe('AdminCommentsListComponent', () => {
  let component: AdminCommentsListComponent;
  let fixture: ComponentFixture<AdminCommentsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCommentsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminCommentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
