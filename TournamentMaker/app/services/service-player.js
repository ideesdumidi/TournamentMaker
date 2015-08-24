'use strict';

angular.module('app.services').factory('playerService', ['$q','$http', 'navigationService',
function ($q, $http, navigationService) {
        return {
            getAll: function () {
                return $http.get(navigationService.urls.player).then(function (data) {
                    angular.forEach(data.data, function(player) {
                        player.fullName = player.firstname + ' ' + player.lastname;
                    });
                    return data.data;
                });

            },
        }
    }]);
    