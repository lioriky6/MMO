require(__dirname + '\\Resources\\config.js');
const fs = require('fs');
const colyseus = require('colyseus');
const http = require("http");
const { verify } = require('crypto');


const schema = require('@colyseus/schema');
const Schema = schema.Schema;
const Player = require(__dirname + '\\Resources\\Schema\\Player.js');
const ChatRoomState = require(__dirname + '\\Resources\\Schema\\ChatRoomState.js');




var init_files = fs.readdirSync(__dirname + '\\Initializer\\');

init_files.forEach(function(file) {
    console.log('Initializing ' + file);
    require(__dirname + '\\Initializer\\' + file)
});



var model_files = fs.readdirSync(__dirname + '\\Models\\');

model_files.forEach(function(file) {
    console.log('Loading Model ' + file);
    require(__dirname + '\\Models\\' + file)
});


maps = {};
var map_files = fs.readdirSync(config.data_paths.maps);

map_files.forEach(function(file) {
    console.log('Loading Map ' + file);
    var map = require(config.data_paths.maps + '\\' + file)
    maps[map.room] = map;
});

const gameserver = new colyseus.Server({
    server: http.createServer(),
    verifyClient: function(info, next) {
        console.log('verifyClient');
        //console.log(info);
        next(true);
    }
});

class ChatRoom extends colyseus.Room {
    // When room is initialized
    onCreate(options) {
        this.setState(new ChatRoomState());
 
        this.onMessage("message", (client, message) => {
            console.log("message", message);
            this.broadcast("message", "message recieved " + message);
        });

        this.onMessage("move", (client, value) => {
            console.log("move update", this.state.players[client.sessionId].x);
            this.state.players[client.sessionId].x = value;
        });        
    }
    
    onJoin(client, options) {
        this.state.players[client.sessionId] = new Player();
    }

    onLeave(client, consented) {
        delete this.state.players[client.sessionId];
    }

}
gameserver.define("chat", ChatRoom)

gameserver.listen(config.port);