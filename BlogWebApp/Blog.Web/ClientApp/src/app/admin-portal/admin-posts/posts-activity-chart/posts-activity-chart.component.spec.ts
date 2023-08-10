import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostsActivityChartComponent } from './posts-activity-chart.component';

describe('PostsActivityChartComponent', () => {
  let component: PostsActivityChartComponent;
  let fixture: ComponentFixture<PostsActivityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostsActivityChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostsActivityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
