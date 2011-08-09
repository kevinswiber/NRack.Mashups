var echo = require('./app/echo'),
    helloWorld = require('./app/helloWorld'),
    printEnvironment = require('./app/printEnvironment'),
    queryString = require('./app/queryString'),
    methodOverride = require('./app/methodOverride');

/*use(methodOverride);
use(printEnvironment);

map('/', function (config) {
    config.use(queryString);
    config.run(helloWorld);
});

map('/echo', function (config) {
    config.run(echo);
});
*/

this.run(helloWorld);