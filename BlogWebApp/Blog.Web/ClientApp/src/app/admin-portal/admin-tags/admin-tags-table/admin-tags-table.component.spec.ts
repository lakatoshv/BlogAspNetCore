import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTagsTableComponent } from './admin-tags-table.component';

describe('AdminTagsTableComponent', () => {
  let component: AdminTagsTableComponent;
  let fixture: ComponentFixture<AdminTagsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTagsTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTagsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
