import { Injectable } from '@angular/core';
import { BehaviorSubject, distinctUntilChanged, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingIndicatorService {
  constructor() { }

  private loadingsInProgress = 0;
  private _loading$ = new BehaviorSubject(false);

  public addLoading() {
    this.loadingsInProgress++;
    this.updateSubject();
  }

  public removeLoading() {
    this.loadingsInProgress--;
    this.updateSubject();
  }

  public get loading$() {
    return this._loading$.pipe(
      distinctUntilChanged(),
      tap(x => console.log(x)),
    );
  }

  public get isLoading() {
    return this.loadingsInProgress > 0;
  }

  private updateSubject() {
    this._loading$.next(this.isLoading);
  }
}
