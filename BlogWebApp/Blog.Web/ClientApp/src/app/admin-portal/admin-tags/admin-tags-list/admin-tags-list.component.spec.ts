import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTagsListComponent } from './admin-tags-list.component';

describe('AdminTagsListComponent', () => {
  let component: AdminTagsListComponent;
  let fixture: ComponentFixture<AdminTagsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTagsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminTagsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
