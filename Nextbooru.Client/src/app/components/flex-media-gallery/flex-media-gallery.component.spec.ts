import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlexMediaGalleryComponent } from './flex-media-gallery.component';

describe('FlexMediaGalleryComponent', () => {
  let component: FlexMediaGalleryComponent;
  let fixture: ComponentFixture<FlexMediaGalleryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FlexMediaGalleryComponent]
    });
    fixture = TestBed.createComponent(FlexMediaGalleryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
