import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalLoadingIndicatorComponent } from './global-loading-indicator.component';

describe('GlobalLoadingIndicatorComponent', () => {
  let component: GlobalLoadingIndicatorComponent;
  let fixture: ComponentFixture<GlobalLoadingIndicatorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GlobalLoadingIndicatorComponent]
    });
    fixture = TestBed.createComponent(GlobalLoadingIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
