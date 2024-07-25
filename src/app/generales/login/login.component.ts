import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {
  authService = inject(AuthService);
  router = inject(Router);
  errorMessage! : string;
  AuthUserSub! : Subscription;

 protected loginForm = new FormGroup({
  email: new FormControl('',[Validators.required, Validators.email]),
  password: new FormControl('', [Validators.required])
 })

 ngOnInit(): void {
  this.AuthUserSub = this.authService.AuthenticatedUser$.subscribe({
    next: user => {
      if(user){
        this.router.navigate(['/dashboard']);
      }
    }
   })
 }

  onSubmit() {
    if(!this.loginForm.valid){
      return;
    } 
    else {
      console.log(this.loginForm.value);
      this.authService.login(this.loginForm.value)
        // .subscribe((data: any) =>{
        //   debugger;
        //    console.log(data);
        //    if(this.authService.isLoggedIn() ){
        //      this.router.navigate(['/dashboard']);
        //    }

        // }
        // )
        
        .subscribe({
          next: userData => {
            this.router.navigate(['/dashboard']);
          },
          error: err => {
            this.errorMessage = err;
            console.log(err);
          }
        })
    }
  }

  ngOnDestroy(): void {
    this.AuthUserSub.unsubscribe();
  }

}
