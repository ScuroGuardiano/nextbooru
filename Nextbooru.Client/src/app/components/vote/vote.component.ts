import { CommonModule } from '@angular/common';
import { Component, EventEmitter, HostBinding, Input, Output } from '@angular/core';

@Component({
  selector: 'app-vote',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './vote.component.html',
  styleUrl: './vote.component.scss',
  host: {
    class: "vote-component"
  }
})
export class VoteComponent {
  @Input() score = 0;
  @Input() userVote: "upvote" | "downvote" | "none" = "none";

  @HostBinding("class.disabled")
  @Input() disabled = false;

  @Output() vote = new EventEmitter<"upvote" | "downvote">();
}
