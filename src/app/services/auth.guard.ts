import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // if(authService.isLoggedIn()){
  //    return true;
  // }

  // router.navigate(['login']);
  // return false;

  return authService.AuthenticatedUser$
      .pipe(
        take(1), // take the first one and then unsubscribe automatically
        map(user => {
          // check if route is restricted by role
          const { profil } = route.data;
          if(user && user.roles ){ //&& profil.includes(user.roles)              
              for(let role in user.roles){
                if(profil.includes(user.roles[role])){
                  return true;
                }
              }
            
            //return true;
          }
          if(user){
            return router.createUrlTree(['/notfound']);
          }
          return router.createUrlTree(['/login']);
        })
      )
};
