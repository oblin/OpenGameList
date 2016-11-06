import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
    selector: 'login',
    templateUrl: './template/login.component.html'
})
export class LoginComponent implements OnInit {
    title = 'Login';
    loginForm: FormGroup;
    loginError = false;

    constructor(private fb: FormBuilder,
        private router: Router,
        private authService: AuthService) {}

    ngOnInit() {
        this.loginForm = this.fb.group({
            username: ['', Validators.required ],
            password: ['', Validators.required ]
        });
    }

    performLogin(e) {
        e.preventDefault();
        let username = this.loginForm.value.username;
        let password = this.loginForm.value.password;
        this.authService.login(username, password)
            .subscribe(
                (data) => {
                    this.loginError = false;
                    let auth = this.authService.getAuth();
                    alert('Our Token is: ' + auth.access_token);
                    this.router.navigate(['']);
                },
                (err) => {
                    console.log(err);
                    this.loginError = true;
                }
            );
        // alert(JSON.stringify(this.loginForm.value));
    }
}
