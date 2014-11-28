﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Web;
using CommunicationLib.Model;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using System.Configuration;

namespace RestAPI
{
    public class OwnException : Exception
    {
        public OwnException()
        {
        }

        public OwnException(String msg):base(msg)
        {
        }
    }

    /// <summary>
    ///     Static class, that realizes the Connection to the server.
    /// </summary>
    public class RestRequester
    {
        public static String restserverurl;
        public static RestClient client;
        private static String _ressourceParam, _operationParam;
        private static JsonSerializerSettings _jsonSettings;

        ///<summary>
        ///     Initializes the RestClient. Has to be called before first use.
        /// </summary>
        static RestRequester()
        {
            restserverurl = Constants.serverUrl;
            client = new RestClient(restserverurl);
            _ressourceParam = "resource/";
            _operationParam = "command/";
            _jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None, Formatting = Formatting.Indented};
        }

        
        /// <summary>
        ///     Requests all Objects (Items, Workflows or Users) belonging to the given user.
        /// </summary>
        /// <typeparam name="RootElementList">The list of RootElements</typeparam>
        /// <param name="userName">name of the user</param>
        /// <returns>the list with RootElements requested from server</returns>
        public static IList<RootElement> GetAllObjects<RootElement>(String userName) where RootElement : new()
        {
            String typeName = typeof(RootElement).FullName.Split('.').Last().ToLower();
            // if userName is not null, it is concatenated to the url, otherwise path  is just 'resource/workflows' and will request all all workflows
            String url = _ressourceParam + typeName + "s" + (userName != null? "/" + userName : "");
            System.Diagnostics.Trace.WriteLine("url: " + url);
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Accept", "text/plain");

            // decide wether the server does return the right excepted object or throws an exception
            try
            {      
                var response = client.Execute(request);
                IList<RootElement> eleList = JsonConvert.DeserializeObject<List<RootElement>>(response.Content, _jsonSettings);
                return eleList;
            }
            catch (OwnException)
            {
                var response = client.Execute<Exception>(request);
                throw response.Data;
            }
        }

        /// <summary>
        ///     Get an object from the server, with HTTP-Method GET.
        ///     Path for this HTTP-Method is always: get/<type>/<id>
        /// </summary>
        /// <typeparam name="O">type of the requested object</typeparam>
        /// <param name="id">id of the requested object</param>
        /// <returns>the requested object</returns>
        public static O GetObject<O>(int id) where O : new()
        {
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName +"/" + id;
            String requestContent = GetObjectRequest<O>(url, Method.GET);

            //Deserialize JSON
            O desObj = JsonConvert.DeserializeObject<O>(requestContent, _jsonSettings);

            return desObj;
        }

        /// <summary>
        ///     Update an object on the server, with HTTP-Method PUT.
        /// </summary>
        /// <param name="sendObj"></param>
        /// <returns></returns>
        public static IRestResponse UpdateObject(RootElement sendObj)
        {
            String typeName = sendObj.GetType().FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName + "/" + sendObj.id;

            // Serialize to JSON
            String serializedObj = JsonConvert.SerializeObject(sendObj, _jsonSettings);
            
            IRestResponse resp = SendObjectRequest(url, Method.PUT, serializedObj);

            return  resp;
        }

        /// <summary>
        ///     Create an object on the server, with HTTP-Method POST.
        /// </summary>
        /// <typeparam name="O">the type of the object to be created</typeparam>
        /// <param name="sendObj">the specified object to create</param>
        /// <returns></returns>
        public static IRestResponse PostObject<O>(RootElement sendObj) where O : new()
        {
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName;
            
            // Serialize to JSON
            String serializedObj = JsonConvert.SerializeObject(sendObj, _jsonSettings);
            
            IRestResponse response = SendObjectRequest(url, Method.POST, serializedObj);
            
            return response;
        }

        /// <summary>
        ///     Delete an object on the server, with HTTP-Method DEL.
        /// </summary>
        /// <typeparam name="O">type of the deleted object</typeparam>
        /// <param name="id">id of the deleted object</param>
        /// <returns>the deleted object</returns>
        public static O DeleteObject<O>(int id) where O : new()
        {
            // 
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName + "/" + id;
            String requestContent = GetObjectRequest<O>(url, Method.DELETE);

            //Deserialize JSON
            O desObj = JsonConvert.DeserializeObject<O>(requestContent, _jsonSettings);

            return desObj;
        }

        public void checkUser(String username, String password)
        {
            // 'operation/user/login'
            String url = _operationParam + "user/" + "login";

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);

            SendSimpleRequest(request);
        }


        /// <summary>
        ///     Sends a request to the server to start a workflow (create an item)
        /// </summary>
        /// <param name="wId">Workflow-Id</param>
        /// <param name="uId">User-name</param>
        public static String StartWorkflow(int wId, string username)
        {
            // 'command/start/workflowid/username'
            String url = _operationParam + "workflow/"+ "start/" + wId + "/" + username;

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");

            String responseContent = SendSimpleRequest(request);
            // do something with the response, e.g. look if everything is ok.
           
            return responseContent;
        }

        /// <summary>
        ///     Sends a state change of an action to the server
        /// </summary>
        /// <param name="stepId">id of the current step</param>
        /// <param name="itemId">id of the current item</param>
        /// <param name="uId">name of the current user</param>
        public static String StepForward(int stepId, int itemId, string username)
        {
            // Request url: 'command/forward/stepid/itemid/username'
            String url = _operationParam + "workflow/" + "forward/" + stepId + "/" + itemId + "/" + username;
            
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");
            //request.AddParameter("username", username, ParameterType.GetOrPost);
            //request.AddParameter("password", password, ParameterType.GetOrPost);
            
            String responseContent = SendSimpleRequest(request);
            // do something with the response, e.g. look if everything is ok.
            
            return responseContent;
        }

        /// <summary>
        ///     Sends a HTTP-Request to the server to get an object.
        /// </summary>
        /// <typeparam name="O">the type of the requested object</typeparam>
        /// <param name="url">the requested url</param>
        /// <param name="method">the HTTP-Method of the request</param>
        /// <returns>the response object from server</returns>
        private static String GetObjectRequest<O>(String url, RestSharp.Method method) where O : new()
        {
            var request = new RestRequest(url, method);
            request.AddHeader("Accept", "text/plain");

            // decide wether the server does return the right excepted object or throws an exception
            try
            {
                var response = client.Execute(request);
                return response.Content;
            }
            catch (OwnException)
            {
                var response = client.Execute<Exception>(request);
                throw response.Data;
            }
        }

        /// <summary>
        ///     Method to send a C# Object to server. Expects an answer-string from server.
        /// </summary>
        /// <param name="url">request url</param>
        /// <param name="method">method of the request</param>
        /// <param name="serializedObjPath">path to the xml file with serialized object</param>
        /// <returns>the response from server</returns>
        private static IRestResponse SendObjectRequest(String url, RestSharp.Method method, String serializedObj)
        {
            var request = new RestRequest(url, method);
            request.AddHeader("Accept", "text/plain");

            // if there is an object that shall be send to server per XML
            if (serializedObj != null)
            {
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddParameter("data", serializedObj, ParameterType.GetOrPost);
            }

            // decide wether the server does return the right excepted object or throws an exception
            try
            {
                var response = client.Execute(request);
                return response;
            }
            catch (OwnException)
            {
                var response = client.Execute<Exception>(request);
                throw response.Data;
            }
        }

        /// <summary>
        ///     Sends a simple request, just containing some information in the url. No more parameters or objects send in the request.
        /// </summary>
        /// <param name="request">The request to send to server.</param>
        /// <returns>The content of the answer</returns>
        private static String SendSimpleRequest(RestRequest request)
        {
            try
            {
                var response = client.Execute(request);
                return response.Content;
            }
            catch(Exception e)
            {
                var response = client.Execute<Exception>(request);
                throw response.Data;
            }
        }
    }
}