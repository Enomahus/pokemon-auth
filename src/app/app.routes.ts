import { tick } from '@angular/core/testing';
import { Routes } from '@angular/router';
import { authGuard } from './services/auth.guard';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        title: 'Login',
        loadComponent: () => import('./generales/login/login.component').then(c => c.LoginComponent)
    },
    {
        path:'pokemons',
        title: 'Pokédex',
        loadChildren: () => import('./pokemon/pokemon.routes').then(route => route.routes),
        canActivate: [authGuard],
        data: { profil:['User','Administrator'] }
    },
    {
        path: 'dashboard',
        title: 'Dashboard',
        loadComponent: () => import('./generales/dashboard/dashboard.component').then(c => c.DashboardComponent),
        canActivate: [authGuard],
        data: { profil:['User','Administrator'] }
    },
    {
        path: 'admin',
        title: 'Administrator',
        loadComponent: () => import('./generales/admin/admin.component').then(c => c.AdminComponent),
        canActivate: [authGuard],
        data: { profil:['Administrator'] }
    },
    {
        path: 'signup',
        title: 'Enregister',
        loadComponent: () => import('./generales/signup/signup.component').then(c => c.SignupComponent)
    },
    {
        path: 'contact',
        title: 'Contact',
        loadComponent: () => import('./generales/contact/contact.component').then(c => c.ContactComponent)
    },
    {
        path: 'about',
        title: 'About',
        loadComponent: () => import('./generales/about/about.component').then(c => c.AboutComponent)
    },
    {
        path: 'notfound',
        title: 'Page not found',
        loadComponent: () => import('./generales/page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent)
    },
    {
        path: '**',
        title: 'Page not found',
        loadComponent: () => import('./generales/page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent)
    }
];
