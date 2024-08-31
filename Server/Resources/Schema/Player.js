const schema = require('@colyseus/schema');
const Schema = schema.Schema;

// class MyState extends Schema {
// }

// type("string")(MyState.prototype, "currentTurn");


class Player extends Schema {
    x = 0;
}
schema.defineTypes(Player, {
  x: "float32",
});

module.exports = Player;
