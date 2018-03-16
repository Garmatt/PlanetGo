angular.module('gameEngine').directive('boardPoint', function () {
    return {
        restrict: 'E',
        scope: {
            point: '='
        },
        link: function (scope) {

            var GetBackgroundSource = function (x, y, board) {
                var size = board.Size;
                var result = 'point';

                if (y === 1)
                    result += '-bottom';
                else if (y === size)
                    result += '-top';

                if (x === 1)
                    result += '-left';
                else if (x === size)
                    result += '-right';

                if (result !== 'point')
                    return result;

                var cornerStarLocation = 3;
                if (board.IsSizeLarge)
                    cornerStarLocation = 4;

                if ((x === cornerStarLocation || x === size - cornerStarLocation + 1) && (y === cornerStarLocation || y === size - cornerStarLocation + 1))
                    return result + '-star';

                if (board.IsSizeOdd)
                {
                    var sideStarLocation = (size + 1) / 2;
                    if (x === sideStarLocation && y === sideStarLocation)
                        return result + '-star';

                    if (board.IsSizeLarge)
                    {
                        if (((x === cornerStarLocation || x === size - cornerStarLocation + 1) && y === sideStarLocation)
                            || ((y === cornerStarLocation || y === size - cornerStarLocation + 1) && x === sideStarLocation))
                            return result + '-star';
                    }
                }

                return result;
            };

            scope.pointClass = ['tile', 'empty'];

            if (scope.point.Board.NextToPlay)
                scope.stone = 'stone-' + scope.point.Board.NextToPlay;

            scope.background = GetBackgroundSource(scope.point.X, scope.point.Y, scope.point.Board);

            scope.$watch('point.Board.NextToPlay', function (newValue, oldValue) {
                if (newValue && $.inArray('empty', scope.pointClass) >= 0)
                    scope.stone = 'stone-' + newValue;
            });

            scope.$watch('point.Group.Board', function (newValue, oldValue) {
                if (newValue) {
                    scope.pointClass = 'point';
                    scope.stone = 'stone-' + scope.point.Group.Color;
                }
                else {
                    scope.pointClass = ['point', 'empty'];
                }
            });

            scope.$watch('point.Board.KoPoint', function (newValue, oldValue) {
                if (scope.point.Equals(newValue))
                    scope.background += '-ko';

                if (scope.point.Equals(oldValue))
                    scope.background = GetBackgroundSource(scope.point.X, scope.point.Y, scope.point.Board);
            });

        },
        templateUrl: '/Scripts/Game/angular/templates/board-point.html'
        //template: "<div ng-class='pointClass'><img ng-src='/Pictures/{{background}}.jpg' /><div class='stone'><img ng-src='/Pictures/{{stone}}.png' /></div></div>"
    };
});

