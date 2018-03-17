angular.module('gameEngine').directive('board', function () {
    return {
        restrict: 'E',
        scope: {
            size: '='
        },
        link: function (scope) {
            scope.Board = new Board(scope.size);
            scope.Board.NextToPlay = Color.Black;
            scope.GetIndices = function () {
                return new Array(scope.size);
            };
            scope.GameHistory = [];
            scope.MoveIndex = -1;
            scope.StepForward = function () {
                scope.GameHistory[++scope.MoveIndex].Do();
                scope.Board.ToggleColor();
                scope.Board.SelectedPoint = scope.GameHistory[scope.MoveIndex].PlayedStone;
                scope.Board.KoPoint = scope.GameHistory[scope.MoveIndex].KoPoint;
            };
            scope.StepBackward = function () {
                scope.GameHistory[scope.MoveIndex--].Undo();
                scope.Board.ToggleColor();
                scope.Board.SelectedPoint = scope.GameHistory[scope.MoveIndex].PlayedStone;
                scope.Board.KoPoint = scope.GameHistory[scope.MoveIndex].KoPoint;
            };
        },
        controller: ['$scope', function BoardController($scope) {
            $scope.MakeMove = function (x, y) {
                console.log('-------------');
                var selectedPoint = $scope.Board.GetPoint(x, y);
                if (selectedPoint.Group || selectedPoint.Equals($scope.Board.KoPoint)) {
                    return;
                }
                var color = $scope.Board.NextToPlay;
                console.log('Playing ' + selectedPoint + ' ' + color);
                var neighbors = selectedPoint.GetNeighbors();
                var neighboringGroupsOfOppositeColor = neighbors.filter(function (neighbor) {
                    return neighbor.Group && neighbor.Group.Color !== color;
                }).map(function (neighbor) {
                    return neighbor.Group;
                    }).unique();
                console.log('Neighboring groups of opposite color: ' + neighboringGroupsOfOppositeColor.join('; '));
                var isMoveLegal = neighboringGroupsOfOppositeColor.some(function (group) { return group.GetLiberties() <= 1; });
                var newGroup = new Group(color);
                newGroup.AddStone(selectedPoint, true);
                console.log('New group: ' + newGroup);
                var neighboringGroupsOfSameColor = neighbors.filter(function (neighbor) {
                    return neighbor.Group && neighbor.Group.Color === color;
                }).map(function (neighbor) {
                    return neighbor.Group;
                }).unique();
                console.log('Neighboring groups of same color: ' + neighboringGroupsOfSameColor.join('; '));
                var connectedGroup = $scope.Board.GetConnectedGroup(neighboringGroupsOfSameColor.concat([newGroup]));
                console.log('Connected group: ' + connectedGroup);
                var connectedGroupLiberties = connectedGroup.GetLiberties();
                isMoveLegal = isMoveLegal || (connectedGroupLiberties > 0);
                if (!isMoveLegal) {
                    selectedPoint.Group = null;
                    return;
                }
                var move = new Move($scope.Board, color, selectedPoint);
                neighboringGroupsOfSameColor.forEach(function (groupToRemove) {
                    $scope.Board.RemoveGroup(groupToRemove, move);
                });
                $scope.Board.AddGroup(connectedGroup, move);
                console.log('Added group: ' + connectedGroup);
                var lookForKo = connectedGroup.Stones.length === 1 && connectedGroupLiberties === 0;
                var koPoint = null;
                neighboringGroupsOfOppositeColor.forEach(function (groupToCheckRemove) {
                    if (groupToCheckRemove.GetLiberties() < 1) {
                        if (lookForKo && groupToCheckRemove.Stones.length === 1)
                            koPoint = groupToCheckRemove.Stones[0];
                        else
                            koPoint = null;
                        $scope.Board.RemoveGroup(groupToCheckRemove, move);
                        lookForKo = false;
                    }
                });
                $scope.Board.ToggleColor();
                console.log(' ');
                $scope.Board.SelectedPoint = selectedPoint;
                $scope.Board.KoPoint = koPoint;
                move.KoPoint = koPoint;
                $scope.MoveIndex++;
                $scope.GameHistory.splice($scope.MoveIndex, $scope.GameHistory.length - $scope.MoveIndex, move);
            };
        }],
        templateUrl: '/Scripts/Game/angular/templates/board.html'
        //template: "<table class='goban playable' cellspacing='0'><tbody><tr ng-repeat='y in GetIndices() track by $index'><td ng-repeat='x in GetIndices() track by $index'><board-point point='Board.GetPoint($index, size - 1 - $parent.$index)' ng-click='MakeMove($index, size - 1 - $parent.$index)'></board-point></td></tr></tbody></table>"
    };
});

