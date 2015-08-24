'use strict';

/*
* globalRequest-interceptor.js
* Intercepteur d'erreurs sur les requêtes Http
* version : 1.0
*/

angular.module('app.services').factory('globalRequestInterceptor', ['$q',
    function ($q) {
        return {
            'request': function (config) {
                config.headers['X-Requested-With'] = "XMLHttpRequest";
                if (!config.cache)
                    config.cache = false;
                return config;
            },
            'response': function (promise) {
                if (promise.data && promise.data.errors) {
                    if (promise.data.errors && promise.data.errors.length > 0) {
                        return $q.reject(promise);
                    }
                }

                return promise;
            },
            'responseError': function (rejection) {
                // TODO : Gérer les erreurs 
                return $q.reject(rejection);
            }
        };
    }
]);