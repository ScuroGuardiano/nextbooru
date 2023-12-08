import { Component, Input } from '@angular/core';
import { TagDto } from 'src/app/backend/backend-types';

@Component({
  selector: 'app-tags-table',
  standalone: true,
  imports: [],
  templateUrl: './tags-table.component.html',
  styleUrl: './tags-table.component.scss'
})
export class TagsTableComponent {
  @Input({ required: true }) tags!: TagDto[];
  @Input() loading = false;
}
