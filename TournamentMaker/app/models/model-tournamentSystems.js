'use strict';

angular.module('app.models').factory('TournamentSystems', function() {
    return [
        {
            name: "Rondes",
            key: "TournamentMaker.BO.Tournaments.RoundTournament,TournamentMaker.BO",
            description: "Toutes les équipes se rencontrent, le classement se fait en fonction des resultats",
            options: [
                {
                    label: "Manches",
                    key: "sleeves",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    },
                    default:1
                },
                {
                    label: "Équipes par match",
                    key: "teamsByMatch",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    },
                    default: 1

                }
            ]
        },
        {
            name: "Poules",
            key: "TournamentMaker.BO.Tournaments.PoolTournament,TournamentMaker.BO",
            description: "Comme pour les Rondes, sauf que la taille des poules est limité",
            options: [
                {
                    label: "Free for All",
                    description: "Toutes les équipes se rencontrent lors d'un seul match",
                    key: "freeForAll",
                    type: "boolean"
                },
                {
                    label: "Manches",
                    key: "sleeves",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    default:1
                    }
                }
            ]
        },
        {
            name: "Elimination directe",
            key: "TournamentMaker.BO.Tournaments.EliminationTournament,TournamentMaker.BO",
            description: "",
            options: [
                {
                    label: "Équipes par match",
                    key: "teamsByMatch",
                    type: "minMaxTeams",
                    default: "min"
                },
                {
                    label: "Manches",
                    key: "sleeves",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    },
                    default: 1

                }
            ]
        },
        {
            name: "Poules + élimination directe",
            key: "TournamentMaker.BO.Tournaments.PoolEliminationTournament,TournamentMaker.BO",
            description: "",
            options: [
                {
                    label: "Free for All",
                    description: "Toutes les équipes se rencontrent en un seul match lors des pools",
                    key: "poolFreeForAll",
                    type: "boolean"
                },
                {
                    label: "Manches lors des poules",
                    key: "poolSleeves",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    },
                    default: 1
                },
                {
                    label: "Manches lors des éliminations",
                    key: "qualificationSleeves",
                    type: "radio",
                    values: {
                        1: "1",
                        2: "2",
                        3: "3",
                        4: "4",
                        5: "5",
                    },
                    default: 1
                },
                {
                    label: "Équipes par match d'élimination",
                    key: "teamsByMatch",
                    type: "minMaxTeams",
                    default: "min"
                }
            ]
        }
    ];
});