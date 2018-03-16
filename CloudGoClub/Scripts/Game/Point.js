function Point(x, y, board) {
    if (!(this instanceof Point)) {
        return new Point(x, y, board);
    }
    this.Board = board;
    this.X = x;
    this.Y = y;
    this.Group = null;
    this.NeighborN = null;
    this.NeighborE = null;
    this.NeighborS = null;
    this.NeighborW = null;
};

Point.prototype.GetNeighbors = function () {
    return [this.NeighborN, this.NeighborE, this.NeighborS, this.NeighborW].filter(function (val) { return val !== null; });
}

Point.prototype.Equals = function (otherPoint) {
    return otherPoint && this.X === otherPoint.X && this.Y === otherPoint.Y;
}

Point.prototype.toString = function () {
    return "(" + this.X + "," + this.Y + ")";
}