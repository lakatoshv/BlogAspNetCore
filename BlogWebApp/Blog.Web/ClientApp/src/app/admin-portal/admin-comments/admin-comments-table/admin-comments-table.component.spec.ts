import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCommentsTableComponent } from './admin-comments-table.component';

describe('AdminCommentsTableComponent', () => {
  let component: AdminCommentsTableComponent;
  let fixture: ComponentFixture<AdminCommentsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCommentsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminCommentsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
