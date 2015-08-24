'use strict';

angular.module('app.controllers').controller('closeMatchController', ['$scope', 'layoutService','match',
    function ($scope, layoutService, match) {
        $scope.match = match;

        $scope.results = [];
        for (var i = 0; i < match.sleeves; i++) {
            $scope.results[i] = {};
            for (var j in match.teams) {
                $scope.results[i][match.teams[j].id] = {result:""};
            };
        };

        $scope.close = function () {
            $scope.match.scores = "";

            for (var i = 0; i < match.sleeves; i++) {
                for (var j in match.teams) {
                    var team = match.teams[j];
                    $scope.match.scores += team.id+":"+$scope.results[i][team.id].result;
                    if (j < match.teams.length - 1)
                        $scope.match.scores += "-";
                };
                if (i < match.sleeves - 1)
                    $scope.match.scores += ";";
            };

            $scope.match.$close();
            this.$close();
        };
    }]);
    