﻿using System.IO;
using System.Net;

    public static class WebRequestExtensions
    {
        /// <summary>
        ///     A WebRequest extension method that gets the WebRequest response or the WebException response.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The WebRequest response or WebException response.</returns>
        public static WebResponse GetResponseSafe(this WebRequest @this)
        {
            try
            {
                return @this.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
        }

    }
