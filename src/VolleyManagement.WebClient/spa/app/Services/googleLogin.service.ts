import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { JsonService } from './json.service';
import { GoogleUserInfo } from '../Models/User/GoogleUserInfo';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Injectable()
export class GoogleLoginService {
    private token: string;
    private userFullName: string;

    constructor(private _jsonService: JsonService, private http: HttpClient) { }

    getToken(): string {
        return this.token ? this.token : '';
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
        const url = 'https://localhost:44370/api/Account/TokenSignin';
        return new Promise((resolve, reject) => {
            const enco: any = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
            const body: any = new HttpParams().set('user', JSON.stringify(user));
            this.http.post(url, body.toString(), { headers: enco, withCredentials: true })
                .subscribe(data => {
                    resolve(data as Response);
                }, error => {
                    reject({ error: error });
                });
        });
    }


    handleUserInfo(response): boolean {
        if (response) {
            this.token = response.token;
            this.userFullName = response.fullName;
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
        this.userFullName = '';
        // TODO add call to backend for logout
    }
}
