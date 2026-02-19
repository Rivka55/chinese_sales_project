import { Routes } from '@angular/router';
import { GiftList } from './components/Gift/gift-list/gift-list';
import { Register } from './components/user/register/register';
import { Login } from './components/user/login/login';
import { DonorList } from './components/Donor/donor-list/donor-list';
import { Cart } from './components/Cart/cart/cart';
import { CartReport } from './components/Cart/cart-report/cart-report';
import { GiftDetails } from './components/Gift/gift-details/gift-details';
import { CategoryList } from './components/Category/category-list/category-list';
import { ReportsDashboard } from './components/Reports/reports-dashboard/reports-dashboard';
import { authGuard } from './guards/auth.guard';
import { managerGuard } from './guards/manager.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'gifts', pathMatch: 'full' },
  { path: 'register', component: Register },
  { path: 'login', component: Login },
  { path: 'gifts', component: GiftList },
  { path: 'gifts/:id', component: GiftDetails },
  { path: 'donors', component: DonorList, canActivate: [managerGuard] },
  { path: 'categories', component: CategoryList, canActivate: [managerGuard] },
  { path: 'cart', component: Cart, canActivate: [authGuard] },
  { path: 'reports', component: ReportsDashboard, canActivate: [managerGuard] },
  { path: 'cart-report', component: CartReport, canActivate: [managerGuard] },
  { path: '**', redirectTo: 'gifts' },
];
