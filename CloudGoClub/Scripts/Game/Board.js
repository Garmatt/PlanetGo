function Board(size) {
    if (!(this instanceof Board)) {
        return new Board(size);
    }
    this.Size = size;
    this.IsSizeOdd = (size % 2) == 1;
    this.IsSizeLarge = size >= 13;
    this.Groups = [];
    this.Points = new Array(size);
    for (let i = 0; i < size; i++) {
        let line = new Array(size);
        let previousLine = this.Points[i - 1];
        for (let j = 0; j < size; j++) {
            let currentPoint = new Point(i + 1, j + 1, this);
            if (j > 0) {
                let neighborW = line[j - 1];
                neighborW.NeighborE = currentPoint;
                currentPoint.NeighborW = neighborW;
            };
            if (i > 0) {
                let neighborN = previousLine[j];
                neighborN.NeighborS = currentPoint;
                currentPoint.NeighborN = neighborN;
            };
            line[j] = currentPoint;
        }
        this.Points[i] = line;
    }
    this.KoPoint = null;
};

Board.prototype.GetPoint = function (x, y) {
    return this.Points[x][y];
}

Board.prototype.AddGroup = function (group, move) {
    this.Groups.push(group);
    group.Stones.forEach(function (stone) {
        stone.Group = group;
    });
    group.Board = this;
    if (move) {
        move.NewGroups.push(group);
    }
};

Board.prototype.RemoveGroup = function (group, move) {
    var index = this.Groups.indexOf(group);
    if (index > -1) {
        this.Groups.splice(index, 1);
        group.Board = null;
        group.Stones.forEach(function (stone) {
            stone.Group = null;
        });
    }
    if (move) {
        move.OldGroups.push(group);
    }
}

Board.prototype.GetConnectedGroup = function (groups) {
    var stones = [];
    groups.forEach(function (group) {
        stones = stones.concat(group.Stones);
    });
    stones = stones.unique();
    var connectedGroup = new Group(groups[0].Color);
    stones.forEach(function (stone) {
        connectedGroup.AddStone(stone, false);
    });
    return connectedGroup;
};

Board.prototype.ToggleColor = function () {
    this.NextToPlay = this.NextToPlay.GetOppositeColor();
}
