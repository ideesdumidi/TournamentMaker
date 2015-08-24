'use strict';
angular.module('app.directives').directive('matchesBloc', ['$modal', 'utilsService', function ($modal, utilsService) {
        return{
            restrict: 'A',
            templateUrl: 'app/templates/directive-matchesBloc.html',
            scope: {
                matches:'=matchesBloc',
                sports:'='
            },
            link: function (scope) {

                scope.close = function (match) {
                    $modal.open({
                        templateUrl: 'app/templates/popup-closeMatch.html',
                        controller: 'closeMatchController',
                        resolve: {
                            match: function () {
                                return match;
                            }
                        }
                    }).result.then();
                };
                scope.leave = function (match) {
                    match.$leave();
                };

                scope.remove = function (match) {
                    match.$remove();
                };
                
                scope.join = function (match, teamId) {

                    $modal.open({
                        templateUrl: 'app/templates/popup-joinMatch.html',
                        controller: 'joinMatchController',
                        resolve: {
                            match: function () {
                                return match;
                            },
                            teamId: function () {
                                return teamId;
                            }
                        }
                    }).result.then();
                }

                scope.getPlayers = function (match, team) {
                    var sport = utilsService.findByKey(scope.sports, "key", match.sportKey);
                    var maxPlayers = sport ? sport.maxPlayers : 0;

                    var players = [];
                    for (var i = 0; i < maxPlayers; i++) {
                        if (team.players.length > i)
                            players[i] = team.players[i];
                        else {
                            players[i] = null;
                        }
                    }

                    return players;
                };

                scope.getSport = function (key) {
                    return utilsService.findByKey(scope.sports, "key", key);
                };
           
        }
	};
}]);
    