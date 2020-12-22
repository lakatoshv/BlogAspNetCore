import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularTagsComponent } from './popular-tags.component';

describe('PopularTagsComponent', () => {
  let component: PopularTagsComponent;
  let fixture: ComponentFixture<PopularTagsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PopularTagsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopularTagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
