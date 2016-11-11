"use strict";var __decorate=this&&this.__decorate||function(decorators,target,key,desc){var d,c=arguments.length,r=c<3?target:null===desc?desc=Object.getOwnPropertyDescriptor(target,key):desc;if("object"==typeof Reflect&&"function"==typeof Reflect.decorate)r=Reflect.decorate(decorators,target,key,desc);else for(var i=decorators.length-1;i>=0;i--)(d=decorators[i])&&(r=(c<3?d(r):c>3?d(target,key,r):d(target,key))||r);return c>3&&r&&Object.defineProperty(target,key,r),r},__metadata=this&&this.__metadata||function(k,v){if("object"==typeof Reflect&&"function"==typeof Reflect.metadata)return Reflect.metadata(k,v)},core_1=require("@angular/core"),forms_1=require("@angular/forms"),router_1=require("@angular/router"),auth_service_1=require("./auth.service"),LoginComponent=function(){function LoginComponent(fb,router,authService){this.fb=fb,this.router=router,this.authService=authService,this.title="Login",this.loginError=!1,this.externalProviderWindow=null}return LoginComponent.prototype.ngOnInit=function(){this.loginForm=this.fb.group({username:["",forms_1.Validators.required],password:["",forms_1.Validators.required]})},LoginComponent.prototype.performLogin=function(e){var _this=this;e.preventDefault();var username=this.loginForm.value.username,password=this.loginForm.value.password;this.authService.login(username,password).subscribe(function(data){_this.loginError=!1,_this.router.navigate([""])},function(err){console.log(err),_this.loginError=!0})},LoginComponent.prototype.onRegister=function(){this.router.navigate(["register"])},LoginComponent.prototype.callExternalLogin=function(providerName){var url="api/Accounts/ExternalLogin/"+providerName,w=screen.width>=1050?1050:screen.width,h=screen.height>=550?550:screen.height,params="toolbar=yes,scrollbars=yes,resizable=yes,width="+w+",height="+h;this.externalProviderWindow&&this.externalProviderWindow.close(),this.externalProviderWindow=window.open(url,"ExternalProvider",params,!1)},LoginComponent=__decorate([core_1.Component({selector:"login",templateUrl:"./template/login.component.html"}),__metadata("design:paramtypes",[forms_1.FormBuilder,router_1.Router,auth_service_1.AuthService])],LoginComponent)}();exports.LoginComponent=LoginComponent;
//# sourceMappingURL=login.component.js.map
