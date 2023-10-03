import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss']
})
export class AuthPageComponent {
  activeTab = signal("register");

  loginHeaderClassess = computed(() => ({ active: this.activeTab() === "login" }));
  registerHeaderClasses = computed(() => ({ active: this.activeTab() === "register" }));


}
