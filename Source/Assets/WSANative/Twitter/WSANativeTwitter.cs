﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if NETFX_CORE && UNITY_WSA_10_0
using CI.WSANative.Twitter.Core;
#endif

namespace CI.WSANative.Twitter
{
    public static class WSANativeTwitter
    {
#if NETFX_CORE && UNITY_WSA_10_0
        private static readonly WSATwitterApi _twitterApi = new WSATwitterApi();
#endif

        /// <summary>
        /// Is the user currently logged in
        /// </summary>
        public static bool IsLoggedIn
        {
#if NETFX_CORE && UNITY_WSA_10_0
            get { return _twitterApi.IsLoggedIn; }
#else
            get { return false; }
#endif
        }

        /// <summary>
        /// Initialise the twitter api - this must be called first - see the website for additional information
        /// </summary>
        /// <param name="consumerKey">Your apps twitter consumer key</param>
        /// <param name="consumerSecret">Your apps twitter consumer secret</param>
        /// /// <param name="oauthCallback">A callback url for oauth (should match the value entered on twitter)</param>
        public static void Initialise(string consumerKey, string consumerSecret, string oauthCallback)
        {
#if NETFX_CORE && UNITY_WSA_10_0
            _twitterApi.Initialise(consumerKey, consumerSecret, oauthCallback);
#endif
        }

        /// <summary>
        /// Shows the login dialog to the user.
        /// If login is successful an access token will be generated and automatically stored.
        /// </summary>
        /// <param name="response">Did the login request succeed</param>
        public static void Login(Action<WSATwitterLoginResult> response)
        {
#if NETFX_CORE && UNITY_WSA_10_0
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                WSATwitterLoginResult result = await _twitterApi.Login();

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(result);
                    }
                }, true);
            }, false);
#endif
        }

        /// <summary>
        /// Log the user out of your application (i.e delete their access token)
        /// </summary>
        public static void Logout()
        {
#if NETFX_CORE && UNITY_WSA_10_0
            _twitterApi.Logout();
#endif
        }

        /// <summary>
        /// Requests details about the logged in user - json response is returned (parse the fields you need)
        /// </summary>
        /// <param name="includeEmail">Should the response include the users email ("Request email addresses from users" must be set on twitter first)</param>
        /// <param name="response">Response containing user details if successful</param>
        public static void GetUserDetails(bool includeEmail, Action<WSATwitterResponse> response)
        {
#if NETFX_CORE && UNITY_WSA_10_0
            GetUserDetailsAsync(includeEmail, response);
#endif
        }

#if NETFX_CORE && UNITY_WSA_10_0
        private static async void GetUserDetailsAsync(bool includeEmail, Action<WSATwitterResponse> response)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "include_email", includeEmail ? "true" : "false" }
            };

            WSATwitterResponse result = await _twitterApi.ApiRead("https://api.twitter.com/1.1/account/verify_credentials.json", parameters);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Call any GET method on the twitter api
        /// </summary>
        /// <param name="url">The base url (e.g https://api.twitter.com/1.1/statuses/user_timeline.json) - any query string arguments should be added to the parameters dictionary</param>
        /// <param name="parameters">Any parameters to be appended to the url - can be a null or empty dictionary if there are none</param>
        /// <param name="response">Response containing the requested data if successful</param>
        public static void ApiRead(string url, IDictionary<string, string> parameters, Action<WSATwitterResponse> response)
        {
#if NETFX_CORE && UNITY_WSA_10_0
            ApiReadAsync(url, parameters, response);
#endif
        }

#if NETFX_CORE && UNITY_WSA_10_0
        private static async void ApiReadAsync(string url, IDictionary<string, string> parameters, Action<WSATwitterResponse> response)
        {
            WSATwitterResponse result = await _twitterApi.ApiRead(url, parameters);

            if (response != null)
            {
                response(result);
            }
        }
#endif
    }
}