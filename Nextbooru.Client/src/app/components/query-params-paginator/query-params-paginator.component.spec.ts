import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueryParamsPaginatorComponent } from './query-params-paginator.component';

describe('QueryParamsPaginatorComponent', () => {
  let component: QueryParamsPaginatorComponent;
  let fixture: ComponentFixture<QueryParamsPaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueryParamsPaginatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(QueryParamsPaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
