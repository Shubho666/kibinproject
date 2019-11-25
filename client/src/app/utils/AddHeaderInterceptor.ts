// import {
//   HttpEvent,
//   HttpInterceptor,
//   HttpHandler,
//   HttpRequest
// } from "@angular/common/http";
// import { Observable } from "rxjs";
// import Cookies from "js-cookie";
// import { environment } from "src/environments/environment";
// export class AddHeaderInterceptor implements HttpInterceptor {
//   intercept(
//     req: HttpRequest<any>,
//     next: HttpHandler
//   ): Observable<HttpEvent<any>> {
//     let a = new RegExp("^" + environment.starturlserver, "i");
//     let clonedRequest;
//     if (req.url.match(a)) {
//       console.log("matched");
//       clonedRequest = req.clone({
//         headers: req.headers.set(
//           "Authorization",
//           "Bearer " + Cookies.get("jwt")
//         )
//       });
//     } else {
//       console.log("not matched");
//       clonedRequest = req.clone();
//     }
//     // Pass the cloned request instead of the original request to the next handle
//     return next.handle(clonedRequest);
//   }
// }
