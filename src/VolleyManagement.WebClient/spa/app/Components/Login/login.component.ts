import { Component, ElementRef, AfterViewInit, Output, NgZone } from '@angular/core';
import { User } from '../../Models/User/User';

declare const gapi: any;

@Component({
  selector: 'vm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {

  private clientId = '390650442273-o866vham0ds02m9pb84tvpfkfo69g4bd.apps.googleusercontent.com';

  private scope = [
    'profile',
    'email'
  ].join(' ');

  public user: User;
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
        const token = googleUser.getAuthResponse().id_token;
        const profile = googleUser.getBasicProfile();
        console.log('Token || ' + token);
        console.log('ID: ' + profile.getId());
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail());

        this.zone.run(() => {
          this.user = {
            Token: token,
            ID: profile.getId(),
            Name: profile.getName(),
            ImageURL: profile.getImageUrl(),
            Email: profile.getEmail()
          };
        });

        const xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:49940/api/Account/TokenSignin');
        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.onload = function () {
          console.log('Signed in as: ' + xhr.responseText);
        };
        xhr.send('idtoken=' + token);
        // TODO code for user saving


      }, function (error) {
        console.log(JSON.stringify(error, undefined, 2));
      });
  }

  constructor(private element: ElementRef, private zone: NgZone) {
    console.log('ElementRef: ', this.element);
  }

  ngAfterViewInit() {
    this.googleInit();
  }
}
