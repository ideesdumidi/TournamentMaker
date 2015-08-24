'use strict';

angular.module('app.controllers').controller('joinMatchController', ['$scope', 'match', 'teamId',
    function ($scope, match, teamId) {
        $scope.match = match;
        $scope.team = {id:teamId};

        $scope.join = function() {
            $scope.match.$join({ teamId: $scope.team.id });
            this.$close();
        };
    }]);
    