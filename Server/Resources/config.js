
var args = require('minimist')(process.argv.slice(2));
var extend = require('extend');
var environment = args.env || 'development';

console.log(environment)


var common_conf = {
    name: 'mmo',
    version: '0.0.1',
    environment: environment,
    max_player: 100,
    data_paths: {
        items: __dirname + '\\Game Data\\' + '\\Items\\',
        maps: __dirname + '\\Game Data\\' + '\\Maps\\',
    },
    starting_map: 'start',
};

var conf = {
    production: {
        ip: args.ip || "0.0.0.0",
        port: args.port || 8081,
        database: "mongodb://localhost:27017/mmo",
    },
    development: {
        ip: args.ip || "0.0.0.0",
        port: args.port || 8082,
        database: "mongodb://localhost:27017/mmo",
    },
};

extend(false, conf.development, common_conf);
extend(false, conf.production, common_conf);

module.exports = config = conf[environment];