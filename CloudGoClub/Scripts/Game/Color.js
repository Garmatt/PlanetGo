window.Color = {};

(function () {
    var color = function (name) {
        if (!(this instanceof color)) {
            return new color(name);
        }
        this.name = name;
    };

    color.prototype.toString = function () {
        return this.name;
    }

    color.prototype.Equals = function (otherColor) {
        return this.name === otherColor.name;
    }

    var GetOppositeColor = function () {
        return this;
    }

    var black = new color('black');
    var white = new color('white');

    black.GetOppositeColor = GetOppositeColor.bind(white);
    white.GetOppositeColor = GetOppositeColor.bind(black);

    Color.Black = black;
    Color.White = white;
})();