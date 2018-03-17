function Move(board, color, playedStone) {
    if (!(this instanceof Move)) {
        return new Move(board);
    }
    this.Board = board;
    this.NewGroups = [];
    this.OldGroups = [];
    this.Color = color;
    this.PlayedStone = playedStone;
    this.KoPoint = null;
};

(function () {
    var addGroups = function (groups) {
        groups.forEach(function (groupToAdd) {
            this.Board.AddGroup(groupToAdd);
        }, this);
    };
    var removeGroups = function (groups) {
        groups.forEach(function (groupToRemove) {
            this.Board.RemoveGroup(groupToRemove);
        }, this);
    };
    Move.prototype.Do = function () {
        removeGroups.call(this, this.OldGroups);
        addGroups.call(this, this.NewGroups);
    };
    Move.prototype.Undo = function () {
        removeGroups.call(this, this.NewGroups);
        addGroups.call(this, this.OldGroups);
    };
})();
