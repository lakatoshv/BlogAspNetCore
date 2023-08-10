import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutComponentComponent } from './layout-component.component';

describe('LayoutComponentComponent', () => {
  let component: LayoutComponentComponent;
  let fixture: ComponentFixture<LayoutComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LayoutComponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LayoutComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
