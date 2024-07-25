import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, catchError, delay, of, tap, throwError } from 'rxjs';
import { IUserModel } from '../Models/userModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  //BehaviorSubject est un type d'observable qui conserve la dernière valeur émise,
  //ce qui en fait une excellente option pour stocker l'état de l'utilisateur connecté
  AuthenticatedUser$ = new BehaviorSubject<IUserModel | null>(null);
  //isLoggedIn: boolean = false;
  cookieService = inject(CookieService);
  storageService = inject(StorageService);
  http = inject(HttpClient);
  router = inject(Router);

  //constructor(private http: HttpClient, private router: Router){}

  register(registerObjet: any) {
    const headers = {'Content-Type': 'application/json'};
    return this.http.post('/api/user/register',registerObjet, {'headers': headers});
  }

  
  //  login(loginObject: any) {
  //    return this.http.post('/api/user/login', loginObject)
  //        .pipe(
  //          tap((result) => {
  //            //localStorage.setItem('authUser',JSON.stringify(result));
  //            this.cookieService.set('authUser',JSON.stringify(result));
  //          })
  //        );
  // }

  //Login avec rôles
   login(loginObject: any){
    const headers = {'Content-Type': 'application/json'};
     return this.http.post('/api/user/login', loginObject, {'headers': headers})
       .pipe(
        
         catchError(err =>{
           let errorMessage = "An unknown error occurred!";
           if(err.error.message === "Bad credentials") {
             errorMessage = "The email address or password you entered is invalid"
           }
           return throwError(() => new Error(errorMessage))
         }), 
         tap((data: any) => {
          debugger;
           const extractedUser: IUserModel = {
             email: data.email,
             id: data.id,
             message: data.message,
             isauthenticated: data.isAuthenticated,
             username: data.userName,
             roles: data.roles,
             token: data.token,
             refreshtoken: data.refreshToken,
             refreshtokenexpiration: data.refreshTokenExpiration
           }
           this.storageService.saveUser(extractedUser);
           this.AuthenticatedUser$.next(extractedUser);
           this.cookieService.set('authUser',JSON.stringify(extractedUser));
         })
       );
   }

   autoLogin(){
    const userData = this.storageService.getSavedUser();
    if(!userData){
      return;
    }
    this.AuthenticatedUser$.next(userData);
   }

  logout() {
    //localStorage.removeItem('authUser');
    this.cookieService.delete('authUser');
    this.storageService.clean();
    this.AuthenticatedUser$.next(null);
    this.router.navigate(['/login']);
  }
  
  isLoggedIn() {
    //return localStorage.getItem('authUser') !== null;
    return this.cookieService.get('authUser') !== null;
  }

  // login(email: string, password: string): Observable<boolean>{
  //   const isLoggedIn = (email =='bolan@test.fr' && password =='bolan');
  //   return of(isLoggedIn).pipe(
  //     delay(1000), 
  //     tap(isLoggedIn => this.isLoggedIn = isLoggedIn)
  //     );
  // }
  
  // logout(){
  //   this.isLoggedIn = false;
  // } 
}
