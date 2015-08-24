'use strict';

angular.module('app.controllers').controller('classementController', ['$scope', 'layoutService', 'utilsService', 'rankService', '$routeParams', 'sportService',
    function ($scope, layoutService, utilsService, rankService, $routeParams, sportService) {
        layoutService.setActivePage("classement");

        if ($routeParams.sport) {
            rankService.get($routeParams.sport).then(function(ranks) {
                $scope.ranks = ranks;
            });

            sportService.get().then(function (sports) {
                $scope.currentSport = utilsService.findByKey(sports, "key", $routeParams.sport);
            });
        } else
            $scope.ranks = rankService.get($routeParams.sport).then(function (ranks) {
                $scope.ranks = ranks;
            });

    }]);
    