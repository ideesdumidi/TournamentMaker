'use strict';

angular.module('app.controllers').controller('layoutController', ['$scope', 'layoutService', 'sportService', 'identityService', 'utilsService',
    function ($scope, layoutService, sportService, identityService, utilsService) {
        $scope.currentPage = layoutService.getActivePage;
        sportService.get().then(function (sports) {
            $scope.sports = sports;
            $scope.activeSport = sportService.getActiveSport();

            if (!$scope.activeSport) {
                $scope.activeSport = sports[0].key;
                sportService.setActiveSport($scope.activeSport);

            }
        });

        $scope.getSport = function(key) {
            return utilsService.findByKey($scope.sports, "key", key);
        };

        identityService.get().then(function (identity) {
            $scope.identity = identity;
        });

        $scope.dropdown = {
            isOpen: false
        };

        $scope.selectSport = function (sport) {
            sportService.setActiveSport(sport.key);
            $scope.activeSport = sport.key;
            $scope.dropdown.isOpen = false;
        };
    }]);
    