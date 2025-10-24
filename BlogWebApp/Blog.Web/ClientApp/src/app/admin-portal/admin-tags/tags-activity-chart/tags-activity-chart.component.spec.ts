import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TagsActivityChartComponent } from './tags-activity-chart.component';

describe('TagsActivityChartComponent', () => {
  let component: TagsActivityChartComponent;
  let fixture: ComponentFixture<TagsActivityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TagsActivityChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TagsActivityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
