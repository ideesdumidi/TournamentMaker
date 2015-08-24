'use strict';

angular.module('app.services').factory('identityService', ['$q','$http', 'navigationService',
function ($q, $http, navigationService) {
    return {
        get: function () {
                return $http.get(navigationService.urls.identity, {cache:true}).then(function (data) {
                    return data.data;
                });

            },
        }
    }]);
    