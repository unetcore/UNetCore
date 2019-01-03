using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

public class GlobalHttpContext {
    private static IHttpContextAccessor _accessor;

    public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

    internal static void Configure (IHttpContextAccessor accessor) {
        _accessor = accessor;
    }

    #region HttpContext for Net Framework
    HttpResponseWrapper response;
    HttpRequestWrapper request;
    HttpSessionState session;
    HttpServerUtility server;

    /// <summary>
    /// 使用的上下文，可能是旧的。
    /// </summary>
    private Microsoft.AspNetCore.Http.HttpContext useContext;
    public GlobalHttpContext () {
        useContext = Current;
        response = new HttpResponseWrapper (useContext);
        request = new HttpRequestWrapper (useContext);
        try {
            if (useContext.Session != null) {
                session = new HttpSessionState (useContext);
            }
        } catch { }
        server = new HttpServerUtility ();

    }

    public HttpRequestWrapper Request => request;
    public HttpResponseWrapper Response => response;
    public HttpSessionState Session => session;
    public HttpServerUtility Server => server;
    public IFeatureCollection Features => Current.Features;
    public ConnectionInfo Connection => Current.Connection;
    public Exception Error { get; set; }
    public WebSocketManager WebSockets => Current.WebSockets;

    [Obsolete ("This is obsolete and will be removed in a future version. See https://go.microsoft.com/fwlink/?linkid=845470.")]
    public Microsoft.AspNetCore.Http.Authentication.AuthenticationManager Authentication => Current.Authentication;

    public ClaimsPrincipal User { get => Current.User; set => Current.User = value; }
    public IDictionary<object, object> Items { get => Current.Items; set => Current.Items = value; }
    public IServiceProvider RequestServices { get => Current.RequestServices; set => Current.RequestServices = value; }
    public CancellationToken RequestAborted { get => Current.RequestAborted; set => Current.RequestAborted = value; }
    public string TraceIdentifier { get => Current.TraceIdentifier; set => Current.TraceIdentifier = value; }

    public void RewritePath (string path) {
        request.Path = '/' + path.TrimStart ('/');
    }
    #endregion
}