'use strict';

angular.module('app.models').factory('Match', ['$resource', 'navigationService', function ($resource, navigationService) {
    var matchResource = new $resource(navigationService.urls.match, { id: '@id' }, {
        get: { method: 'get' },
        update: { method: 'put', isArray: false },
        create: { method: 'post' },
        remove: {method:'delete'},
        join: {method:'post', url:navigationService.urls['match-join']},
        start: {method:'post', url:navigationService.urls['match-start']},
        randomize: {method:'post', url:navigationService.urls['match-randomize']},
        leave: { method: 'post', url: navigationService.urls['match-leave'] },
        close: { method: 'post', url: navigationService.urls['match-close'] },
    });

    matchResource.prototype.$save = function () {
        if (!this.id) {
            return this.$create();
        } else {
            return this.$update();
        }
    };

    return matchResource;
}]);
    