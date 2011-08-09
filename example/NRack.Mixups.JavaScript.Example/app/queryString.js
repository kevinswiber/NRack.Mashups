var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    if (env["QUERY_STRING"]) {
        return { status: 200, headers: { "Content-Type": "text/plain" }, body: "Your query string: " + env["QUERY_STRING"] };
    }

    return app.call(env);
};