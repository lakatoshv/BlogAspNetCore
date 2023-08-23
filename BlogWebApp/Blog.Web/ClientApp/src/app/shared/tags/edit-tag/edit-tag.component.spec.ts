import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTagComponent } from './edit-tag.component';

describe('EditTagComponent', () => {
  let component: EditTagComponent;
  let fixture: ComponentFixture<EditTagComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditTagComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
