import { Component } from '@angular/core';
import { RegisterModel } from '../../../models/user/RegisterModel';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/user/auth-service';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
  AsyncValidatorFn,
} from '@angular/forms';
import { NgClass } from '@angular/common';
import { of, timer } from 'rxjs';
import { switchMap, map, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink, NgClass],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  registerForm: FormGroup;
  errorMsg: string = '';
  successMsg: string = '';
  isSubmitting: boolean = false;
  showPassword: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.registerForm = this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
        ],
        [this.nameExistsValidator()],
      ],
      phone: [
        '',
        [
          Validators.required,
          this.phoneValidator,
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          this.emailValidator,
        ],
        [this.emailExistsValidator()],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(100),
        ],
      ],
    });
  }

  get f() {
    return this.registerForm.controls;
  }

  /**
   * Matches the C# [Phone] DataAnnotation behavior:
   * - Must start with 0 (Israeli) or + (international)
   * - Israeli mobile: 05X-XXXXXXX (10 digits)
   * - Israeli landline: 0X-XXXXXXX (9 digits)
   * - International: +XXX... (7-15 digits)
   * - Allows dashes and spaces as separators
   */
  phoneValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value?.trim();
    if (!value) return null; // required handles empty

    // Strip dashes, spaces, parentheses for digit check
    const digitsOnly = value.replace(/[\-\s\(\)]/g, '');

    // Must start with 0 or +
    if (!/^[0\+]/.test(value)) {
      return { phonePrefix: true };
    }

    // Must contain only digits, dashes, spaces, parentheses, plus
    if (!/^[\d\-\+\(\)\s]+$/.test(value)) {
      return { phoneChars: true };
    }

    // Israeli number starting with 0
    if (value.startsWith('0')) {
      // Mobile: 05X (10 digits) or landline: 02/03/04/08/09 (9 digits)
      if (digitsOnly.length < 9 || digitsOnly.length > 10) {
        return { phoneLength: true };
      }
      // Israeli mobile must start with 05
      if (digitsOnly.length === 10 && !digitsOnly.startsWith('05')) {
        return { phoneMobile: true };
      }
    }

    // International number starting with +
    if (value.startsWith('+')) {
      const intlDigits = digitsOnly.replace(/\+/g, '');
      if (intlDigits.length < 7 || intlDigits.length > 15) {
        return { phoneLength: true };
      }
    }

    return null;
  }

  /**
   * Matches the C# [EmailAddress] DataAnnotation behavior:
   * - Must have exactly one @
   * - Local part: letters, digits, dots, hyphens, underscores, plus
   * - Domain: letters, digits, dots, hyphens
   * - Domain must have at least one dot with 2+ char TLD
   * - No consecutive dots
   */
  emailValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value?.trim();
    if (!value) return null; // required handles empty

    // Full email regex matching [EmailAddress] behavior
    const emailRegex = /^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$/;

    if (!emailRegex.test(value)) {
      // Provide specific sub-errors for better UX
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
      if (parts[1].endsWith('.') || parts[1].startsWith('.')) {
        return { emailBadDomain: true };
      }
      return { emailInvalid: true };
    }

    return null;
  }

  nameExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value?.trim();
      if (!value || value.length < 2) {
        return of(null);
      }
      return timer(400).pipe(
        switchMap(() => this.authService.checkNameExists(value)),
        map(exists => (exists ? { nameExists: true } : null)),
        catchError(() => of(null))
      );
    };
  }

  emailExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value?.trim();
      if (!value || this.emailValidator(control)) {
        return of(null);
      }
      return timer(400).pipe(
        switchMap(() => this.authService.checkEmailExists(value)),
        map(exists => (exists ? { emailExists: true } : null)),
        catchError(() => of(null))
      );
    };
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  getPasswordStrength(): number {
    const val = this.f['password'].value || '';
    let score = 0;
    if (val.length >= 6) score++;
    if (val.length >= 10) score++;
    if (/[A-Z]/.test(val)) score++;
    if (/[0-9]/.test(val)) score++;
    if (/[^a-zA-Z0-9]/.test(val)) score++;
    return score;
  }

  getPasswordStrengthText(): string {
    const score = this.getPasswordStrength();
    if (score <= 1) return 'חלשה';
    if (score <= 2) return 'בינונית';
    if (score <= 3) return 'טובה';
    return 'חזקה מאוד';
  }

  getPasswordStrengthClass(): string {
    const score = this.getPasswordStrength();
    if (score <= 1) return 'weak';
    if (score <= 2) return 'medium';
    if (score <= 3) return 'good';
    return 'strong';
  }

  getPasswordStrengthPercent(): string {
    const score = this.getPasswordStrength();
    return Math.min(100, score * 25) + '%';
  }

  register() {
    this.registerForm.markAllAsTouched();
    this.errorMsg = '';
    this.successMsg = '';

    if (this.registerForm.invalid) {
      return;
    }

    this.isSubmitting = true;

    const user: RegisterModel = this.registerForm.value as RegisterModel;

    this.authService.register(user).subscribe({
      next: () => {
        this.successMsg = 'ההרשמה בוצעה בהצלחה! מעביר להתחברות...';
        this.isSubmitting = false;
        setTimeout(() => {
          this.router.navigate(['/login'], {
            state: { email: user.email, password: user.password }
          });
        }, 1500);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMsg = this.extractErrorMessage(err);
      },
    });
  }

  private extractErrorMessage(err: any): string {
    const error = err.error;

    if (!error) {
      return 'שגיאה בהרשמה, נסה שוב';
    }

    // String error
    if (typeof error === 'string') {
      return this.translateError(error);
    }

    // { message: "..." }
    if (error.message) {
      return this.translateError(error.message);
    }

    // .NET ValidationProblemDetails: { title: "...", errors: { Field: ["..."] } }
    if (error.errors) {
      const messages = Object.values(error.errors)
        .flat()
        .map((msg: any) => this.translateError(msg));
      return messages.join('\n') || 'שגיאה בהרשמה, נסה שוב';
    }

    // { title: "..." }
    if (error.title) {
      return this.translateError(error.title);
    }

    return 'שגיאה בהרשמה, נסה שוב';
  }

  private translateError(message: string): string {
    if (!message) return 'שגיאה בהרשמה, נסה שוב';

    const lowerMsg = message.toLowerCase();

    // Email related
    if (lowerMsg.includes('email') && (lowerMsg.includes('exist') || lowerMsg.includes('taken') || lowerMsg.includes('use') || lowerMsg.includes('duplicate'))) {
      return 'כתובת האימייל כבר רשומה במערכת';
    }
    if (lowerMsg.includes('email') && lowerMsg.includes('invalid')) {
      return 'כתובת אימייל לא תקינה';
    }
    if (lowerMsg.includes('email') && lowerMsg.includes('required')) {
      return 'אימייל הוא שדה חובה';
    }

    // User/account related
    if (lowerMsg.includes('user') && (lowerMsg.includes('exist') || lowerMsg.includes('taken') || lowerMsg.includes('registered'))) {
      return 'משתמש כבר קיים במערכת';
    }

    // Password related
    if (lowerMsg.includes('password') && (lowerMsg.includes('short') || lowerMsg.includes('length') || lowerMsg.includes('at least'))) {
      return 'הסיסמה קצרה מדי';
    }
    if (lowerMsg.includes('password') && lowerMsg.includes('required')) {
      return 'סיסמה היא שדה חובה';
    }
    if (lowerMsg.includes('password') && (lowerMsg.includes('upper') || lowerMsg.includes('digit') || lowerMsg.includes('special') || lowerMsg.includes('non alphanumeric'))) {
      return 'הסיסמה חייבת להכיל אות גדולה, ספרה ותו מיוחד';
    }

    // Name related
    if (lowerMsg.includes('name') && lowerMsg.includes('required')) {
      return 'שם הוא שדה חובה';
    }

    // Phone related
    if (lowerMsg.includes('phone') && lowerMsg.includes('required')) {
      return 'טלפון הוא שדה חובה';
    }
    if (lowerMsg.includes('phone') && lowerMsg.includes('invalid')) {
      return 'מספר טלפון לא תקין';
    }

    // Generic
    if (lowerMsg.includes('validation') || lowerMsg.includes('one or more')) {
      return 'הנתונים שהוזנו אינם תקינים';
    }
    if (lowerMsg.includes('server') || lowerMsg.includes('internal')) {
      return 'שגיאת שרת, נסה שוב מאוחר יותר';
    }

    // Already Hebrew - return as-is
    if (/[\u0590-\u05FF]/.test(message)) {
      return message;
    }

    return 'שגיאה בהרשמה, נסה שוב';
  }
}
