'use strict';

angular.module('app.models').factory('Tournament', ['$resource', 'navigationService', function ($resource, navigationService) {
    var matchResource = new $resource(navigationService.urls.tournament, { id: '@id' }, {
        get: { method: 'get' },
        update: { method: 'put', isArray: false },
        create: { method: 'post' },
        remove: {method:'delete'},
        join: { method: 'post', url: navigationService.urls['tournament-join'] },
        start: { method: 'post', url: navigationService.urls['tournament-start'] },
        randomize: { method: 'post', url: navigationService.urls['tournament-randomize'] },
        leave: { method: 'post', url: navigationService.urls['tournament-leave'] },
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
    