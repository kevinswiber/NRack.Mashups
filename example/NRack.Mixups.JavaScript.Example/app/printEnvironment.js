var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    if (!env["QUERY_STRING"] || env["QUERY_STRING"] != "env") {
            return app.call(env);
    }
 
    var body = '';
    for (var key in env) {
        body += key + " = " + env[key] + "\n";
    }

    return { status: 200, headers: { "Content-Type": "text/plain" }, body: [body] };
};