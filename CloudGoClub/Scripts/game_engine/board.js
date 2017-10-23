angular.module('gameEngine').directive('board', function () {
    return {
        restrict: 'E',
        scope: {
            size: '='
        },
        link: function (scope) {
            scope.Board = new Board(scope.size);
            scope.Board.NextToPlay = 'black';
            scope.GetIndices = function () {
                return new Array(scope.size);
            };
        },
        controller: ['$scope', function BoardController($scope) {
            $scope.MakeMove = function (x, y) {
                var selectedPoint = $scope.Board.GetPoint(x, y);
                if (selectedPoint.Group) {
                    return;
                }
                var color = $scope.Board.NextToPlay;
                var neighbors = selectedPoint.GetNeighbors();
                var neighboringGroupsOfOppositeColor = neighbors.map(function (neighbor) {
                    if (neighbor.Group && neighbor.Group.Color !== color) {
                        return neighbor.Group;
                    }
                }).unique();
                var isMoveLegal = neighboringGroupsOfOppositeColor.some(function (val) { return val && val.GetLiberties() <= 1; });
                var newGroup = new Group(color);
                newGroup.AddStone(selectedPoint);
                var neighboringGroupsOfSameColor = neighbors.map(function (neighbor) {
                    if (neighbor.Group && neighbor.Group.Color === color) {
                        return neighbor.Group;
                    }
                }).unique();
                neighboringGroupsOfSameColor.push(newGroup);
                newGroup = $scope.Board.GetConnectedGroup(neighboringGroupsOfSameColor);
                isMoveLegal = isMoveLegal || (newGroup.GetLiberties() > 0);
                if (!isMoveLegal) {
                    return;
                }
                $scope.Board.AddGroup(newGroup);
                neighboringGroupsOfOppositeColor.forEach(function (groupToCheckRemove) {
                    if (groupToCheckRemove.GetLiberties() < 1) {
                        $scope.Board.RemoveGroup(groupToCheckRemove);
                    }
                });
                if (color === 'black') {
                    $scope.Board.NextToPlay = 'white';
                }
                else {
                    $scope.Board.NextToPlay = 'black';
                }
            };
        }],
        //templateUrl: '/templates/board.html'
        template: "<table class='goban playable' cellspacing='0'><tbody><tr ng-repeat='y in GetIndices() track by $index'><td ng-repeat='x in GetIndices() track by $index'><board-point point='Board.GetPoint($index, size - 1 - $parent.$index)' ng-click='MakeMove($index, size - 1 - $parent.$index)'></board-point></td></tr></tbody></table>"
    };
});

