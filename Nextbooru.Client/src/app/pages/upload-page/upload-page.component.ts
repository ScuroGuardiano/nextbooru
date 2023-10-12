import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Select } from '@ngxs/store';
import { AuthState, AuthStateModel } from 'src/app/store/state/auth.state';
import { Observable, firstValueFrom, map } from 'rxjs';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UploadService } from 'src/app/services/upload.service';

@Component({
  selector: 'app-upload-page',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './upload-page.component.html',
  styleUrls: ['./upload-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UploadPageComponent {
  private readonly uploadService = inject(UploadService);

  @Select(AuthState) authState$!: Observable<AuthStateModel>;
  isLoggedIn$ = this.authState$.pipe(
    map(s => !!s.isLoggedIn)
  );

  // TODO: add validation for tags and content-type of file.
  uploadForm = new FormGroup({
    title: new FormControl(""),
    source: new FormControl(""),
    // Because it's *taggable image board* then tags must be required ^^
    tags: new FormControl("", [Validators.required]),
    file: new FormControl<File | null>(null, [Validators.required])
  });

  onSubmit(event: Event) {
    event.preventDefault();
    console.dir(this.uploadForm);
    // @ts-ignore
    firstValueFrom(this.uploadService.upload(this.uploadForm.value));
  }

  onFilePicked(event: Event) {
    const file = (event?.target as HTMLInputElement)?.files?.[0];
    this.uploadForm.patchValue({ file });
  }
}
