import { Component, ElementRef, AfterViewInit, Output, NgZone } from '@angular/core';
import { GoogleUserInfo } from '../../Models/User/GoogleUserInfo';
import { GoogleLoginService } from '../../Services/googleLogin.service';
import { environment } from '../../../environments/environment';

declare const gapi: any;

@Component({
  selector: 'vm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {

  private clientId = environment.googleClientId;
  public token: string;
  public userName: string;

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
        scope: this.scope
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
              this.userName = this.googleLoginService.getUserName();
            });
          }
        });
      });
  }

  constructor(private element: ElementRef, private googleLoginService: GoogleLoginService, private zone: NgZone) {
  }

  ngAfterViewInit() {
    this.googleInit();
  }
}
