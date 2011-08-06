var echo = require('app/echo'),
    helloWorld = require('app/hello_world');

map('/', function () { run(helloWorld); });
map('/echo', function() { run(echo); });
//run(helloWorld);