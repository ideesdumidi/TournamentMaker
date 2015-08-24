'use strict';

angular.module('app.services').factory('rankService', ['$q','$http', 'navigationService',
function ($q, $http, navigationService) {
    return {
        get: function (sportKey) {
            var params = {};
            if(sportKey)
                params.sport = sportKey;

            return $http.get(navigationService.urls.rank, { cache: false, params: params }).then(function (data) {
                return data.data;
            });
        }}
    }]);
    