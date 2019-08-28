import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';

@Injectable()
// Intercepts an outgoing HttpRequest and optionally transform it or the response.
// Used to add 'withCredentials: true' to every request.
// withCredentials property is a Boolean that indicates whether or not cross-site
// Access-Control requests should be made using credentials such as cookies,
// authorization headers or TLS client certificates.
export class TokenInterceptor implements HttpInterceptor {

    constructor() { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        request = request.clone({ withCredentials: true });
        return next.handle(request);
    }
}
