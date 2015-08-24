'use strict';

angular.module('app.services').factory('sportService', ['$q','$http', 'navigationService',
function ($q, $http, navigationService) {
    return {
        get: function() {
            return $http.get(navigationService.urls.sport, { cache: true }).then(function(data) {
                return data.data;
            });
        },
        getActiveSport: function() {
            return localStorage.ActiveSport;
        },
        setActiveSport: function (sportId) {
            localStorage.ActiveSport = sportId;
        }
}
    }]);
    