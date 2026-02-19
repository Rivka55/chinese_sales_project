import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const tokenData = authService.getToken(); 

  if (tokenData) {
    let tokenValue = '';
    try {
      const parsedToken = JSON.parse(tokenData);
      if (typeof parsedToken === 'string') {
        tokenValue = parsedToken;
      } else if (parsedToken?.token) {
        tokenValue = parsedToken.token;
      }
    } catch (e) {
      tokenValue = tokenData;
    }

    // Strip any leftover surrounding quotes
    tokenValue = tokenValue.replace(/^"|"$/g, '').trim();

    if (tokenValue) {
      const cloned = req.clone({
        setHeaders: {
          Authorization: `Bearer ${tokenValue}` 
        }
      });
      return next(cloned);
    }
  }

  return next(req);
};