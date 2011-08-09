exports.call = function (env) {
    return {
        status: 200,
        headers: { "Content-Type": "text/plain" },
        body: [env["REQUEST_METHOD"], ' ', env["SCRIPT_NAME"], env["PATH_INFO"]]
    };
};