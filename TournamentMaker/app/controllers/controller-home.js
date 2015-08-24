'use strict';

angular.module('app.controllers').controller('homeController', ['$scope', 'layoutService','utilsService','Match','$routeParams','sportService',
    function ($scope, layoutService, utilsService, Match, $routeParams,sportService) {
        layoutService.setActivePage("home");

        if ($routeParams.sport) {
            $scope.nextMatches = Match.query({ sport: $routeParams.sport });

            sportService.get().then(function (sports) {
                $scope.currentSport = utilsService.findByKey(sports, "key", $routeParams.sport);
            });
        } else
            $scope.nextMatches = Match.query();

    }]);
    