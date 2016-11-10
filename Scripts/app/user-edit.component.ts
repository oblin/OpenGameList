import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from './auth.service';
import { User } from './user';

@Component({
    selector: 'user-edit',
    templateUrl: './template/user-edit.component.html'
})
export class UserEditComponent implements OnInit {
    title = '註冊新的使用者帳號';
    userForm: FormGroup = null;
    errorMessage: string = null;
    isRegister: boolean;    // 判斷 current route is account or register

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private activateRoute: ActivatedRoute,
        private authService: AuthService) {
        // 判斷目前的路徑是那一個
        this.isRegister = activateRoute.snapshot.url.toString() === 'register';
        if ((this.isRegister && this.authService.isLoggedIn())
            || (!this.isRegister && !this.authService.isLoggedIn())) {
            this.router.navigate(['']);
        }
        if (this.isRegister) {
            this.title = 'Edit Account';
        }
    }

    ngOnInit() {
        this.userForm = this.fb.group(
            {
                username: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9]+')]],
                email: ['', [
                    Validators.required,
                    Validators.pattern("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")
                ]],
                // TODO: 加入密碼檢查：要有英數字混合
                password: ['', [Validators.required, Validators.minLength(6)]],
                passwordConfirm: ['', [Validators.required, Validators.minLength(6)]],
                displayName: ['', null]
            },
            {
                // extra checking: 當輸入 password or passwordConfirm 這兩個欄位時，會自動檢查
                validator: this.compareValidator('password', 'passwordConfirm')
            }
        );

        if (!this.isRegister) {
            // 由於修改密碼時，就會變成三個欄位：目前密碼、新密碼與確認密碼，因此這裡在多加一個欄位
            this.userForm.addControl('passwordCurrent', new FormControl('', Validators.required));
            // 如果使用者是屬於修改的狀態，不需要檢查密碼 required 的問題，僅須針對是否符合規範檢測
            let password = this.userForm.controls['password'];
            password.clearValidators();
            password.setValidators(Validators.minLength(6));
            let passwordConfirm = this.userForm.controls['passwordConfirm'];
            passwordConfirm.clearValidators();
            passwordConfirm.setValidators(Validators.minLength(6));

            this.authService.get().subscribe(user => {
                this.userForm.controls['username'].setValue(user.UserName);
                this.userForm.controls['email'].setValue(user.Email);
                this.userForm.controls['displayName'].setValue(user.DisplayName);
            });
        }
    }

    /**
     * 提供 user-edit form 使用：註冊新的使用者帳號 或者 更新使用者廖
     * @memberOf UserEditComponent
     */
    onSubmit() {
        if (this.isRegister) {
            this.addUser();
        } else {
            this.updateUser();
        }
    }

    private updateUser() {
        let user = new User(
            this.userForm.value.username,
            this.userForm.value.passwordCurrent,    // 目前的密碼
            this.userForm.value.password,           // 變更後的密碼
            this.userForm.value.email,
            this.userForm.value.displayName
        );
        this.authService.update(user)
            .subscribe(
                (data: any) => {
                    if (data.error) {
                        this.errorMessage = data.error;
                    } else {
                        this.errorMessage = null;
                        this.router.navigate(['']);
                    }
                },
                (err) => {
                    this.errorMessage = err;
                }
            );
    }

    private addUser() {
        this.authService.add(this.userForm.value)
            .subscribe(
            (data: any) => {
                // TODO: data.error 是 server 新增使用者發生 exception 所傳入，改用 error handling 來處理
                if (data.error == null) {
                    // registration successful
                    this.errorMessage = null;
                    // TODO: 如果需要 email confirmation，則要改用另外的處理方案
                    this.authService.login(this.userForm.value.username, this.userForm.value.password)
                        .subscribe(
                        (res) => {
                            this.errorMessage = null;
                            this.router.navigate(['']);
                        },
                        (err) => {
                            console.log(err);
                            this.errorMessage = 'Warning: Username of password mismatch, error = ' + err;
                        }
                        );
                } else {
                    this.errorMessage = data.error;
                }
            },
            (err) => {
                // server/connection error
                this.errorMessage = err;
            }
            );
    }

    /**
     * 檢查密碼與密碼確認是否輸入同樣的正確數值 
     * 
     * @param {string} pw1 密碼
     * @param {string} pw2 確認密碼
     * @returns {{ [key: string]: any }} 如果正確傳回 null，否則傳回錯誤物件，例如：{ compareFailed: true }
     */
    private compareValidator(pw1: string, pw2: string): { [key: string]: any } {
        return (group: FormGroup) => {
            let password = group.controls[pw1];
            let passwordConfirm = group.controls[pw2];
            if (password.value === passwordConfirm.value) {
                return null;
            }
            return { compareFailed: true };
        };
    }
}
