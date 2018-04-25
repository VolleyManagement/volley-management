import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { JsonService } from './json.service';
import { GoogleUserInfo } from '../Models/User/GoogleUserInfo';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable()
export class GoogleLoginService {
    private token: string;
    private userFullName: string;

    constructor(private _jsonService: JsonService, private http: HttpClient) { }

    getToken(): string {
        return this.token ? this.token : '';
    }

    getUserName(): string {
        return this.userFullName ? this.userFullName : '';
    }

    getGoogleUserInfo(googleUser: any): GoogleUserInfo {
        return {
            AccessToken: googleUser.getAuthResponse().access_token,
            ExpiresAt: googleUser.getAuthResponse().expires_at,
            ExpiresIn: googleUser.getAuthResponse().expires_in,
            IdToken: googleUser.getAuthResponse().id_token,
            Code: googleUser.getAuthResponse().code,
            ID: googleUser.getBasicProfile().getId(),
            Name: googleUser.getBasicProfile().getName(),
            ImageURL: googleUser.getBasicProfile().getImageUrl(),
            Email: googleUser.getBasicProfile().getEmail()
        };
    }

    validateGoogleToken(user: GoogleUserInfo): Promise<Response> {
        const url = 'api/Account/TokenSigninGoogle';
        return new Promise((resolve, reject) => {
            const encoding: any = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
            const body: any = new HttpParams().set('user', JSON.stringify(user));
            this.http.post(environment.apiUrl.concat(url), body.toString(), { headers: encoding, withCredentials: true })
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
