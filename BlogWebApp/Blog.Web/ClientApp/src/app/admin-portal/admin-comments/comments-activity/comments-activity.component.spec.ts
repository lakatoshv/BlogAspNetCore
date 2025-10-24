import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentsActivityComponent } from './comments-activity.component';

describe('CommentsActivityComponent', () => {
  let component: CommentsActivityComponent;
  let fixture: ComponentFixture<CommentsActivityComponent>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [ CommentsActivityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentsActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
