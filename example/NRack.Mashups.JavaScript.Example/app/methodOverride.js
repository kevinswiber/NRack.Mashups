var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    var request = env["rack.input"].read();

    if (!request) {
        return app.call(env);
    }

    var reqArray = request.split('&');

    var method;
    for (var i = 0; i < reqArray.length; i++) {
        var kv = reqArray[i].split('=');
        if (kv[0] == "_method") {
            method = kv[1];
            break;
        }
    }

    if (!method) {
        if (env["HTTP_X_HTTP_METHOD_OVERRIDE"]) {
            method = env["HTTP_X_HTTP_METHOD_OVERRIDE"];
        }
    }

    if (method) {
        env["rack.methodoverride.original_method"] = env["REQUEST_METHOD"];
        env["REQUEST_METHOD"] = method.toUpperCase();
    }

    return app.call(env);
};