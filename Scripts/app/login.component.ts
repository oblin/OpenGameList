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
    externalProviderWindow = null;  // 必須要是 public 讓外部可以呼叫

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

    callExternalLogin(providerName: string) {
        let url = 'api/Accounts/ExternalLogin/' + providerName;
        // minialistic mobile devices support
        let w = (screen.width >= 1050) ? 1050 : screen.width;
        let h = (screen.height >= 550) ? 550 : screen.height;
        let params = `toolbar=yes,scrollbars=yes,resizable=yes,width=${w},height=${h}`;
        // close previously opened windows (if any)
        if (this.externalProviderWindow) {
            this.externalProviderWindow.close();
        }
        this.externalProviderWindow =
            window.open(url, 'ExternalProvider', params, false);
    }
}
