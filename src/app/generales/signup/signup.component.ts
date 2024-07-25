import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink,ReactiveFormsModule, RouterModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {

  authService  =  inject(AuthService);
  router  =  inject(Router);
  
  public signupForm = new FormGroup({
    firstname: new FormControl('',[Validators.required]),
    lastname: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required])
  })

  public onSubmit() {
    if (this.signupForm.valid) {
      debugger;
      console.log(this.signupForm.value);
      this.authService.register(this.signupForm.value)
        // .subscribe({
        //   next: (data: any) => {
        //     debugger;
        //     console.log(data);
        //     this.router.navigate(['/login']);
        //   },
        //   error: (err) => console.log(err)
        // });
        .subscribe((data: any) => {
          debugger;
          console.log(data);
          this.router.navigate(['/login']);
        })
    }
  }
}
