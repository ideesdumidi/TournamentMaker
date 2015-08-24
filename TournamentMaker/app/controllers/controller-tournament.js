'use strict';

angular.module('app.controllers').controller('tournamentController', [
    '$scope', 'layoutService', 'sportService', 'playerService', 'Tournament','TournamentSystems', '$routeParams', 'utilsService',
    function($scope, layoutService, sportService, playerService, Tournament,TournamentSystems, $routeParams, utilsService) {
        layoutService.setActivePage("create");

        var createTournament = function() {
            var dateCreation = new Date();
            dateCreation.setMinutes(Math.round(dateCreation.getMinutes() / 15) * 15);
            $scope.model = new Tournament({
                $type: null,
                sportKey: sportService.getActiveSport() * 1,
                teams: [
                    {
                        name: "Équipe 1",
                        players: []
                    }, {
                        name: "Équipe 2",
                        players: []
                    }, {
                        name: "Équipe 3",
                        players: []
                    }
                ],
                date: dateCreation
            });
        }

        if ($routeParams.id) {
            Tournament.get({ id: $routeParams.id }, function (match) {
                if (match) {
                    $scope.model = match;
                    updateSport();
                } else {
                    createTournament();
                }
            },createTournament);
        } else {
            createTournament();
        }

        $scope.systems = TournamentSystems;

        $scope.$watch("model.type", function (type) {
            if ($scope.model) {
                $scope.model.$type = type ? type.key : null;

                if (!type)
                    return;

                angular.forEach(type.options, function(option) {
                    if (option.default != undefined && !$scope.model[option.key]) {
                        if (option.type == "minMaxTeams") {
                            if (option.default == "min")
                                $scope.model[option.key] = $scope.currentSport.minTeams;
                            else if (option.default == "max")
                                $scope.model[option.key] = $scope.currentSport.maxTeams;
                            else
                                $scope.model[option.key] = option.default;
                        }else
                        $scope.model[option.key] = option.default;
                    }
                });
            }
        });
        

        $scope.currentSport = null;
        $scope.equipes = [2];
        $scope.joueurs = [1];

        $scope.players = [];
        $scope.filteredPlayers = [];
        playerService.getAll().then(function(players) {
            $scope.players = players;
        });

        $scope.addTeam = function() {
            var team = {
                name: "Équipe " + ($scope.model.teams.length + 1),
                players: []
            };
            updateTeam(team);
            $scope.model.teams.push(team);
        }

        $scope.removeTeam = function(index) {
            if (index >= 0 && $scope.currentSport)
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

        var updateFilteredPlayers = function() {
            $scope.filteredPlayers = angular.copy($scope.players);
            if (!$scope.model)
                return;
            angular.forEach($scope.model.teams, function (team) {
                if (!team)
                    return;
                angular.forEach(team.players, function (player) {
                    if (!player) return;

                    var currentPlayer = utilsService.findByKey($scope.filteredPlayers, 'matricule', player.matricule);
                    if (!currentPlayer) return;

                    $scope.filteredPlayers.splice($scope.filteredPlayers.indexOf(currentPlayer), 1);
                });
            });
        };

        $scope.createLoop = function (start, end) {
            if (end === undefined) {
                end = start;
                start = 0;
            }
            var array = [];
            for (var i = 0; i <= end-start; i++) {
                array[i] = start + i;
            }
            return array;
        }

        $scope.createSleeves = function (qualification,match) {

        }

        $scope.save = function() {
            $scope.model.$save();
        };
        $scope.start = function() {
            $scope.model.$start();
        };
        $scope.randomize = function() {
            $scope.model.$randomize();
        };
        $scope.openDatepicker = function($event) {
            $event.stopPropagation();
            $event.preventDefault();
            $scope.opened = true;
        };

        $scope.$watch("model.ranked", function(ranked) {
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

        $scope.$watch("model.qualifications",function(qualifications) {
            angular.forEach(qualifications, function(qualification) {
                angular.forEach(qualification.matchs, function(match) {
                    var nbTeams = qualification.nbTeams;
                    var sleeves = qualification.sleeves;
                    var array = new Array(nbTeams);
                    match.results = array;

                    for (var i = 0; i < array.length; i++) {
                        array[i] = new Array(sleeves);
                    }

                    if (!match.scores)
                        return;

                    var result = match.scores.split(";");

                    for (var k = 0; k < result.length; k++) {
                        var sleeve = result[k].split("-");
                        for (var l = 0; l < sleeve.length; l++) {
                            var score = sleeve[l].split(":")[1];
                            array[l][k] = { result: score };
                        }
                    }

                    return;
                });
            });
        });
    }
]);