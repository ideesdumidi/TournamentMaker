'use strict';

angular.module('app.controllers').controller('matchController', [
    '$scope', 'layoutService', 'sportService', 'playerService', 'Match', '$routeParams', '$modal','utilsService',
    function ($scope, layoutService, sportService, playerService, Match, $routeParams, $modal, utilsService) {
        layoutService.setActivePage("create");

        if ($routeParams.id) {
            Match.get({ id: $routeParams.id }, function(match) {
                $scope.model = match;
                updateSport();
            });
        } else {
            var dateCreation = new Date();
            dateCreation.setMinutes(Math.round(dateCreation.getMinutes() /15) * 15);
            $scope.model = new Match({
                sportKey: sportService.getActiveSport() * 1,
                sleeves: 1,
                teams:[ {
                    name: "Équipe 1",
                    players: []
                },{
                name: "Équipe 2",
                players: []
                    }],
                date: dateCreation
            });
        }

        $scope.currentSport = null;
        $scope.equipes = [2];
        $scope.joueurs = [1];

        $scope.players = [];
        playerService.getAll().then(function(players) {
            $scope.players = players;
        });

        $scope.addTeam = function (){
            if ($scope.currentSport && $scope.model.teams.length < $scope.currentSport.maxTeams) {
                var team = {
                    name: "Équipe " + ($scope.model.teams.length + 1),
                    players: []
                };
                updateTeam(team);
                $scope.model.teams.push(team);
            }
        }

        $scope.removeTeam = function (index) {
            if(index>=0 && $scope.currentSport && index <$scope.currentSport.maxTeams)
                $scope.model.teams.splice(index, 1);
        }


        
        var updateSport = function() {
            if (!$scope.model)
                return;

            for (var key in $scope.sports) {
                var sport = $scope.sports[key];
                if (sport.key != $scope.model.sportKey) {
                    continue;
                }
                $scope.currentSport = sport;
                $scope.joueurs = [1];
                var i;

                for (i = 2; i <= sport.maxPlayers; i++)
                    $scope.joueurs.push(i);
            }

            if ($scope.currentSport && $scope.model.teams.length > $scope.currentSport.maxTeams)
                $scope.model.teams.splice($scope.currentSport.maxTeams-1, $scope.model.teams.length - $scope.currentSport.maxTeams);

            angular.forEach($scope.model.teams, function(team) {
                updateTeam(team);
            });
        };

        var updateTeam = function(team) {
            var maxPlayers = $scope.currentSport ? $scope.currentSport.maxPlayers : 0;
            if (team.players.length > maxPlayers)
                team.players.splice(maxPlayers - 1, team.players.length - maxPlayers);
            else
                while (team.players.length < maxPlayers) {
                    var length = [team.players.length];
                    team.players[length] = null;
                }
            updatePlayers(team);
        };

        var updatePlayers = function(team) {
            if (!$scope.players || !$scope.model)
                return;

            for (var playerKey in team.players) {
                var player = team.players[playerKey];
                if (!player)
                    continue;
                for (var key in $scope.players) {
                    var existingPlayer = $scope.players[key];
                    if (existingPlayer.matricule == player.matricule) {
                        team.players[playerKey] = existingPlayer;
                        break;
                    }
                }
            }
        };

        var updateFilteredPlayers = function () {
            $scope.filteredPlayers = angular.copy($scope.players);
            angular.forEach($scope.model.teams, function (team) {
                angular.forEach(team.players, function (player) {
                    var currentPlayer = utilsService.findByKey($scope.filteredPlayers, 'matricule', player.matricule);
                    if (!currentPlayer) return;

                    $scope.filteredPlayers.splice($scope.filteredPlayers.indexOf(currentPlayer), 1);
                });
            });
        };


        $scope.save = function() {
            $scope.model.$save();
        };
        $scope.start = function () {
            $scope.model.$start();
        };
        $scope.randomize = function () {
            $scope.model.$randomize();
        };
        $scope.close = function (match) {
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
        $scope.openDatepicker = function($event) {
            $event.stopPropagation();
            $event.preventDefault();
            $scope.opened = true;
        };

        $scope.$watch("model.ranked",function(ranked) {
            if (ranked)
                $scope.model.private = false;
        });

        $scope.$watch("model.sportKey", updateSport);
        $scope.$watch("sports", updateSport);

        $scope.$watch("players", updateSport);
        $scope.$watch("model.teams", updateSport, true);
        $scope.$watch("model", updateSport, true);

        $scope.$watch("players", updateFilteredPlayers);
        $scope.$watch("model.teams", updateFilteredPlayers, true);
    }
]);