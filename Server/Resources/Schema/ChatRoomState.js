const schema = require('@colyseus/schema');
const Schema = schema.Schema;
const MapSchema = schema.MapSchema;
const Player = require(__dirname + '\\Player.js');

class ChatRoomState extends Schema {
    constructor () {
        super();

        this.players = new MapSchema();
    }
}
schema.defineTypes(ChatRoomState, {
  players: { map: Player }
});

module.exports = ChatRoomState;
