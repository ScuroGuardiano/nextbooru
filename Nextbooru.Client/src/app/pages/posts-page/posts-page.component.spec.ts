import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostsPageComponent } from './posts-page.component';

describe('PostsPageComponent', () => {
  let component: PostsPageComponent;
  let fixture: ComponentFixture<PostsPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [PostsPageComponent]
    });
    fixture = TestBed.createComponent(PostsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
