﻿exports.call = function (env) {
    if (env["PATH_INFO"] === "/") {
        return {
            status: 200,
            headers: { "Content-Type": "text/plain" },
            body: ["Hello world!", env["REQUEST_METHOD"]]
        };
    }

    return {
        status: 404,
        headers: { "Content-Type": "text/plain" },
        body: ["Not found."]
    };
}; 