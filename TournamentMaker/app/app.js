'use strict';
/*
* app.js
* Définit le module principal et les sous modules de l'application angularJs tarifri
* version : 1.0
 */
angular.module('app.models', ['ngResource']);
angular.module('app.services', []);
angular.module('app.directives', ['app.services']);
angular.module('app.controllers', ['app.services', 'app.models']);

angular.module('app', ['ui.bootstrap', 'ngRoute', 'app.controllers', 'app.services',
                                            'app.directives']);

angular.module('app')
    .config([
        '$routeProvider', '$httpProvider', function($routeProvider, $httpProvider) {

            $httpProvider.interceptors.push('globalRequestInterceptor');
            
            $routeProvider
                .when('/home', { controller: 'homeController', templateUrl: 'app/views/home.html', })
                .when('/home/:sport', { controller: 'homeController', templateUrl: 'app/views/home.html', })
                .when('/classement', { controller: 'classementController', templateUrl: 'app/views/classement.html', })
                .when('/classement/:sport', { controller: 'classementController', templateUrl: 'app/views/classement.html', })
                .when('/match', { controller: 'matchController', templateUrl: 'app/views/match.html', })
                .when('/match/:id', { controller: 'matchController', templateUrl: 'app/views/match.html', })
                .when('/tournament', { controller: 'tournamentController', templateUrl: 'app/views/tournament.html', })
                .when('/tournament/:id', { controller: 'tournamentController', templateUrl: 'app/views/tournament.html', })
                .otherwise({ redirectTo: '/home' });
        }
]);