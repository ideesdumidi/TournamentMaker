'use strict';

angular.module('app.services').factory('utilsService', [
    function() {
        return {
            findByKey: function(array, key, value) {
                for (var k in array) {
                    var tre = array[k];
                    if (tre[key] == value) {
                        return tre;
                    }
                }
                return null;
            }
        };
    }
]);