import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-query-params-paginator',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './query-params-paginator.component.html',
  styleUrl: './query-params-paginator.component.scss'
})
export class QueryParamsPaginatorComponent {
  @Input({ required: true }) currentPage!: number;
  @Input() displayedPages: number = 9;
  @Input() totalPages?: number;

  getDisplayedPages(): number[] {
    if (!this.totalPages) {
      return [];
    }

    const displayedPages = Math.min(this.displayedPages, this.totalPages);
    const halfDisplayedPages = Math.floor(this.displayedPages / 2);

    const pages = [];

    let pagesLeft =
      this.currentPage > halfDisplayedPages
      ? halfDisplayedPages
      : this.currentPage - 1;

    if ((this.currentPage + halfDisplayedPages) > this.totalPages) {
      pagesLeft = Math.min(
        displayedPages - 1,
        pagesLeft + Math.abs(this.totalPages - (this.currentPage + halfDisplayedPages))
      );
    }

    const startPage = this.currentPage - pagesLeft;

    for (let i = 0; i < displayedPages; i++) {
      pages.push(startPage + i);
    }
    return pages;
  }
}
