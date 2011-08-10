var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    var request = env["rack.input"].readToEnd();

    if (!request || request === "") {
        return app.call(env);
    }

    var reqArray = request.split('&');

    for (var i = 0; i < reqArray.length; i++) {
        var kv = reqArray[i].split('=');
        if (kv[0] == "_method") {
            env["REQUEST_METHOD"] = kv[1];
            break;
        }
    }

    return app.call(env);
};