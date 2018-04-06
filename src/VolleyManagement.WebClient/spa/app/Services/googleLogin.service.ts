import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { JsonService } from './json.service';
import { GoogleUserInfo } from '../Models/User/GoogleUserInfo';

@Injectable()
export class GoogleLoginService {
    private token: string;

    constructor(private _jsonService: JsonService) { }

    getToken(): string {
        return this.token ? this.token : 'EMPTY';
    }
    getGoogleUserInfo(googleUser: any): GoogleUserInfo {
        const token = googleUser.getAuthResponse().id_token;
        const profile = googleUser.getBasicProfile();

        const user = {
            AccessToken: googleUser.getAuthResponse().access_token,
            ExpiresAt: googleUser.getAuthResponse().expires_at,
            ExpiresIn: googleUser.getAuthResponse().expires_in,
            IdToken: token,
            Code: googleUser.getAuthResponse().code,
            ID: profile.getId(),
            Name: profile.getName(),
            ImageURL: profile.getImageUrl(),
            Email: profile.getEmail()
        };

        return user;
    }

    validateGoogleToken(user: GoogleUserInfo): Promise<Response> {
        return new Promise(function (resolve, reject) {
            const xhr = new XMLHttpRequest();
            xhr.open('POST', 'http://localhost:49940/api/Account/TokenSignin', true);
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr.onreadystatechange = function (e) {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        resolve(xhr.response);
                    } else {
                        reject(xhr.status);
                    }
                }
            };
            xhr.ontimeout = function () {
                reject('timeout');
            };
            xhr.send('user=' + JSON.stringify(user));
        });
    }


    handleUserInfo(response): boolean {
        const data = JSON.parse(response);
        if (data) {
            this.token = data.token;

            // store username and jwt token in local storage to keep user logged in between page refreshes
            // localStorage.setItem('currentUser', JSON.stringify({ username: isInfoReturned, token: isInfoReturned }));

            return true;
        }
        return false;
    }

    login(googleUser: any): Promise<boolean> {
        return new Promise((resolve, reject) => {
            const user = this.getGoogleUserInfo(googleUser);
            const validationResult = this.validateGoogleToken(user);
            const self = this;
            validationResult
                .then((r) => {
                    resolve(this.handleUserInfo(r));
                })
                .catch(function handleErrors(error) {
                    reject(error);
                });
        });
    }

    logout(): void {
        this.token = null;
        localStorage.removeItem('currentUser');
    }
}
