import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimpleMediaGalleryComponent } from './simple-media-gallery.component';

describe('SimpleMediaGalleryComponent', () => {
  let component: SimpleMediaGalleryComponent;
  let fixture: ComponentFixture<SimpleMediaGalleryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [SimpleMediaGalleryComponent]
    });
    fixture = TestBed.createComponent(SimpleMediaGalleryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
