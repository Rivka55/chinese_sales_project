import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../../models/user/LoginModel';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/user/auth-service';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login implements OnInit {
  loginForm: FormGroup;
  errorMsg: string = '';
  isSubmitting: boolean = false;
  showPassword: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          this.emailValidator,
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
        ],
      ],
    });
  }

  ngOnInit() {
    const state = history.state as { email?: string; password?: string };
    if (state?.email) {
      this.loginForm.patchValue({
        email: state.email,
        password: state.password || ''
      });
    }
  }

  get f() {
    return this.loginForm.controls;
  }

  /**
   * Matches the C# [EmailAddress] DataAnnotation behavior
   */
  emailValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value?.trim();
    if (!value) return null;

    const emailRegex = /^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$/;

    if (!emailRegex.test(value)) {
      if (!value.includes('@')) {
        return { emailNoAt: true };
      }
      const parts = value.split('@');
      if (parts.length !== 2) {
        return { emailMultiAt: true };
      }
      if (!parts[0] || parts[0].length === 0) {
        return { emailNoLocal: true };
      }
      if (!parts[1] || !parts[1].includes('.')) {
        return { emailNoDomain: true };
      }
      return { emailInvalid: true };
    }

    return null;
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  login() {
    this.loginForm.markAllAsTouched();
    this.errorMsg = '';

    if (this.loginForm.invalid) {
      return;
    }

    this.isSubmitting = true;

    const user: LoginModel = this.loginForm.value as LoginModel;

    this.authService.login(user).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isSubmitting = false;

        // 400 Bad Request - server model validation errors
        if (err.status === 400) {
          if (err.error?.message) {
            this.errorMsg = err.error.message;
          } else if (err.error?.errors) {
            // ModelState errors from server
            const messages: string[] = [];
            for (const key of Object.keys(err.error.errors)) {
              const fieldErrors = err.error.errors[key];
              if (Array.isArray(fieldErrors)) {
                messages.push(...fieldErrors);
              }
            }
            this.errorMsg = messages.join(' | ') || 'נתונים לא תקינים';
          } else {
            this.errorMsg = 'נתונים לא תקינים';
          }
        }
        // 401 Unauthorized - wrong email/password
        else if (err.status === 401) {
          this.errorMsg = err.error?.message || 'אימייל או סיסמה שגויים';
        }
        // 500 Server Error
        else if (err.status === 500) {
          this.errorMsg = err.error?.message || 'שגיאת שרת, נסה שוב מאוחר יותר';
        }
        // Network / connection error
        else if (err.status === 0) {
          this.errorMsg = 'לא ניתן להתחבר לשרת, בדוק את החיבור לאינטרנט';
        }
        // Other errors
        else {
          this.errorMsg = err.error?.message || 'אימייל או סיסמה שגויים';
        }
      },
    });
  }
}
