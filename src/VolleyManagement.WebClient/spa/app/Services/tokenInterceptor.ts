import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';

@Injectable()
// Intercepts an outgoing HttpRequest and optionally transform it or the response.
// Used to add 'withCredentials: true' to every request
export class TokenInterceptor implements HttpInterceptor {

    constructor() { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        request = request.clone({ withCredentials: true });
        return next.handle(request).do(event => {
            if (event instanceof HttpResponse) {
                console.log(event);
            }
        });
    }
}
