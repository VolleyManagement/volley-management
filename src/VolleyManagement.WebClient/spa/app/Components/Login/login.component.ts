import { Component, ElementRef, AfterViewInit, Output, NgZone } from '@angular/core';
import { GoogleUserInfo } from '../../Models/User/GoogleUserInfo';
import { GoogleLoginService } from '../../Services/googleLogin.service';

declare const gapi: any;

@Component({
  selector: 'vm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {

  private clientId = '390650442273-o866vham0ds02m9pb84tvpfkfo69g4bd.apps.googleusercontent.com';
  public token: string;
  private scope = [
    'profile',
    'email'
  ].join(' ');

  public user: GoogleUserInfo;
  public auth2: any;

  public googleInit() {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: this.clientId,
        cookiepolicy: 'single_host_origin',
        scope: this.scope,
        redirect_uri: 'http://localhost:49940/api/Account/TokenSignin'
      });
      this.attachSignin(this.element.nativeElement.firstChild);
    });
  }

  public attachSignin(element) {
    this.auth2.attachClickHandler(element, {},
      (googleUser) => {
        let isLogined = false;
        const result = this.googleLoginService.login(googleUser);
        result.then((response) => {
          isLogined = response;
          if (isLogined) {
            this.zone.run(() => {
              this.token = this.googleLoginService.getToken();
            });
          }
        });
      }, function (error) {
        console.log(JSON.stringify(error, undefined, 2));
      });
  }

  constructor(private element: ElementRef, private googleLoginService: GoogleLoginService, private zone: NgZone) {
  }

  ngAfterViewInit() {
    this.googleInit();
  }
}
