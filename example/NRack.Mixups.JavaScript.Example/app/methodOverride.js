var app;

exports.initialize = function (application) {
    app = application;
};

exports.call = function (env) {
    //var request = env["rack.input"].read();
    //print(typeof (env["rack.input"]));
    //    var reqArray = request.split('&');

    //    for (var i = 0; i < reqArray.length; i++) {
    //        var kv = reqArray.split('=');
    //        if (kv[0] == "_method") {
    //            env["REQUEST_METHOD"] = kv[1];
    //            break;
    //        }
    //    }

    return app.call(env);
};