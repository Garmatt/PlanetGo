function Group(color) {
    if (!(this instanceof Group)) {
        return new Group(color);
    }
    this.Color = color;
    this.Stones = [];
    this.NeighboringPoints = [];
    this.Board = null;
};

Group.prototype.toString = function () {
    return this.Color + ' group with ' + this.GetLiberties() + ' liberties: ' + this.Stones.join('-');
}

Group.prototype.AddStone = function (point, assignToGroup) {
    var compareToPoint = function (p) {
        return function (val) {
            return val.X === p.X && val.Y === p.Y;
        };
    };
    if (this.Stones.some(compareToPoint(point))) {
        return false;
    }
    this.Stones.push(point);
    if (assignToGroup) {
        point.Group = this;
    }
    var neighborToRemove = this.NeighboringPoints.find(compareToPoint(point));
    if (neighborToRemove) {
        this.NeighboringPoints.splice(this.NeighboringPoints.indexOf(neighborToRemove), 1);
    }
    var neighborsToAdd = point.GetNeighbors().filter(function (val) {
        return !(this.Stones.some(compareToPoint(val))) || (this.NeighboringPoints.some(compareToPoint(val)));
    }, this);
    if (neighborsToAdd.length > 0) {
        this.NeighboringPoints = this.NeighboringPoints.concat(neighborsToAdd);
    }
    return true;
}

Group.prototype.GetLiberties = function () {
    return this.NeighboringPoints.filter(function (point) { return point.Group === null; }).length || 0;
}