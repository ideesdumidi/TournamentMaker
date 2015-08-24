'use strict';

angular.module('app.services').factory('layoutService', [
    function () {
        var currentPage = '';
        return {
            setActivePage: function(page) {
                currentPage = page;
            },
            getActivePage: function() {
                return currentPage;
            }
        }
    }]);
    