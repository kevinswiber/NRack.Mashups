var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    if (env["QUERY_STRING"]) {
        return { status: 200, headers: { "Content-Type": "text/plain" }, body: "This is a GET request." };
    }

    return app.call(env);
};