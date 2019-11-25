// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: true,
  starturlserver: 'http://proflo.app.cgi-w3.stackroute.io/boards/api',
  starturlconnection: 'http://proflo.app.cgi-w3.stackroute.io/boards',
  starturlclient: 'http://proflo.app.cgi-w3.stackroute.io',

  taskurl:'http://proflo.app.cgi-w3.stackroute.io/charts/api/Tasks/',
  linkurl :'http://proflo.app.cgi-w3.stackroute.io/charts/api/Link/',
  Gloggerurl: 'http://proflo.app.cgi-w3.stackroute.io/charts/api',
  signalRurl:'http://proflo.app.cgi-w3.stackroute.io/charts/gantthub',

  ideazoneurl:'http://proflo.app.cgi-w3.stackroute.io/ideas',
  ideazoneurlapi:'http://proflo.app.cgi-w3.stackroute.io/ideas/api'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
