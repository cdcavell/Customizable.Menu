namespace ClassLibrary.Mvc.Http
{
    public static class StatusCodeDefinition
    {
        private static readonly List<KeyValuePair<int, string>> _StatusCodeList = new();

        private static void Load()

        {
            _StatusCodeList.Clear();

            _StatusCodeList.Add(new KeyValuePair<int, string>(100, "Continue"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(101, "Switching Protocols"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(200, "OK"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(201, "Created"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(202, "Accepted"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(203, "Non-Authoritative Information"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(204, "No Content"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(205, "Reset Content"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(206, "Partial Content"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(300, "Multiple Choices - The requested resource corresponds to any one of a set of representations, each with its own specific location, and agent- driven negotiation information (section 12) is being provided so that the user (or user agent) can select a preferred representation and redirect its request to that location."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(301, "Moved Permanently - The requested resource has been assigned a new permanent URI and any future references to this resource SHOULD use one of the returned URIs."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(302, "Found - The requested resource resides temporarily under a different URI. Since the redirection might be altered on occasion, the client SHOULD continue to use the Request-URI for future requests."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(303, "See Other - The response to the request can be found under a different URI and SHOULD be retrieved using a GET method on that resource."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(304, "Not Modified - If the client has performed a conditional GET request and access is allowed, but the document has not been modified, the server SHOULD respond with this status code. The 304 response MUST NOT contain a message-body, and thus is always terminated by the first empty line after the header fields."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(305, "Use Proxy - The requested resource MUST be accessed through the proxy given by the Location field. The Location field gives the URI of the proxy. The recipient is expected to repeat this single request via the proxy."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(306, "Unused - The 306 status code was used in a previous version of the specification, is no longer used, and the code is reserved."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(307, "Temporary Redirect - The requested resource resides temporarily under a different URI. Since the redirection MAY be altered on occasion, the client SHOULD continue to use the Request-URI for future requests."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(400, "Bad Request - The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(401, "Unauthorized - The request requires user authentication."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(402, "Payment Required"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(403, "Forbidden - The server understood the request, but is refusing to fulfill it. Authorization will not help and the request SHOULD NOT be repeated."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(404, "Not Found - The server has not found anything matching the Request-URI. No indication is given of whether the condition is temporary or permanent."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(405, "Method Not Allowed - The method specified in the Request-Line is not allowed for the resource identified by the Request-URI."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(406, "Not Acceptable - The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(407, "Proxy Authentication Required - This code is similar to 401 (Unauthorized), but indicates that the client must first authenticate itself with the proxy."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(408, "Request Timeout - The client did not produce a request within the time that the server was prepared to wait."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(409, "Conflict - The request could not be completed due to a conflict with the current state of the resource."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(410, "Gone - The requested resource is no longer available at the server and no forwarding address is known. This condition is expected to be considered permanent."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(411, "Length Required - The server refuses to accept the request without a defined Content- Length."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(412, "Precondition Failed - The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(413, "Request Entity Too Large - The server is refusing to process a request because the request entity is larger than the server is willing or able to process."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(414, "Request-URI Too Long - The server is refusing to service the request because the Request-URI is longer than the server is willing to interpret."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(415, "Unsupported Media Type - The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(416, "Requested Range Not Satisfiable"));
            _StatusCodeList.Add(new KeyValuePair<int, string>(417, "Expectation Failed - The expectation given in an Expect request-header field (see section 14.20) could not be met by this server, or, if the server is a proxy, the server has unambiguous evidence that the request could not be met by the next-hop server."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(500, "Internal Server Error - The server encountered an unexpected condition which prevented it from fulfilling the request."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(501, "Not Implemented - The server does not support the functionality required to fulfill the request."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(502, "Bad Gateway - The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(503, "Service Unavailable - The server is currently unable to handle the request due to a temporary overloading or maintenance of the server."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(504, "Gateway Timeout - The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server specified by the URI (e.g. HTTP, FTP, LDAP) or some other auxiliary server (e.g. DNS) it needed to access in attempting to complete the request."));
            _StatusCodeList.Add(new KeyValuePair<int, string>(505, "HTTP Version Not Supported - The server does not support, or refuses to support, the HTTP protocol version that was used in the request message."));
        }

        public static KeyValuePair<int, string> GetCodeDefinition(int code)
        {
            if (!_StatusCodeList.Any())
                Load();

            KeyValuePair<int, string> definition = _StatusCodeList.Find(x => x.Key == code);
            if (definition.Key != code || code == 0)
                definition = new KeyValuePair<int, string>(600, "Unknown status code");

            return definition;
        }

        public static string ToString(int code)
        {
            KeyValuePair<int, string> definition = GetCodeDefinition(code);
            return $"[{definition.Key}] {definition.Value}";
        }
    }
}
