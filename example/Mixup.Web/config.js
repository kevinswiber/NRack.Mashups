var echo = require('./app/echo'),
    helloWorld = require('./app/helloWorld'),
    printEnvironment = require('./app/printEnvironment'),
    queryString = require('./app/queryString');

use(printEnvironment);
use(queryString);
map('/', function (config) { config.run(helloWorld); });
map('/echo', function (config) { config.run(echo); });