﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Web;
using CommunicationLib.Model;
using CommunicationLib.Exception;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Security;
using CommunicationLib;

namespace RestAPI
{
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
        ///     Static Constructor - is called automatically at first use of the class.
        /// </summary>
        static RestRequester()
        {
            restserverurl = Constants.serverUrl;
            client = new RestClient(restserverurl);
            _ressourceParam = "resource/";
            _operationParam = "command/";
            _jsonSettings = new JsonSerializerSettings { 
                TypeNameHandling = TypeNameHandling.Auto, 
                Formatting = Formatting.Indented,
                Binder = new CustomSerializationBinder()};
        }

        
        /// <summary>
        ///     Requests all Objects (Items, Workflows or Users) belonging to the given user.
        /// </summary>
        /// <typeparam name="RootElementList">The list of RootElements</typeparam>
        /// <param name="userName">The users name</param>
        /// <returns>The list with RootElements requested from server</returns>
        public static IList<O> GetAllObjects<O>(String userName) where O : new()
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
                IList<O> eleList = JsonConvert.DeserializeObject<List<O>>(response.Content, _jsonSettings);
                return eleList;
            }
            catch (Exception)
            {
                var response = client.Execute<Exception>(request);
                throw response.Data;
            }
        }

        /// <summary>
        ///     Get an object from the server, with HTTP-Method GET.
        ///     Path for this HTTP-Method is always: ressource/<typename>/<id>/
        /// </summary>
        /// <typeparam name="O">Type of the requested object</typeparam>
        /// <param name="id">Id of the requested object</param>
        /// <returns>The requested object</returns>
        public static O GetObject<O>(int id) where O : new()
        {
            IRestResponse response;
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName +"/" + id;
            try
            {
                response = GetObjectRequest<O>(url, Method.GET);
                
            }
            catch (BasicException)
            {
                throw;
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.WriteLine("Ein anderer Fehler in getObject");
                throw;
            }
            //Deserialize JSON
            O desObj = JsonConvert.DeserializeObject<O>(response.Content, _jsonSettings);

            if (desObj.GetType() == typeof(Workflow))
            {
                convertIdListToReferences(ChangeType<Workflow>(desObj));
            }     

            return desObj;
        }

        /// <summary>
        /// This method changes the type of a generic object.
        /// </summary>
        /// <typeparam name="T">The expected new type.</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ChangeType<T>(object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Incoming order of step ids are converted into references.
        /// </summary>
        /// <param name="workflow"></param>
        public static void convertIdListToReferences(Workflow workflow)
        {
            foreach (Step s in workflow.steps)
            {
                foreach (int id in s.nextStepIds)
                {
                    s.nextSteps.Add(workflow.getStepById(id));
                }
            }

        }

        /// <summary>
        ///     Update an object on the server, with HTTP-Method PUT. Path is always: 'resource/<typename>/<objId>'
        /// </summary>
        /// <param name="sendObj">The object to update</param>
        /// <returns>If it worked or not</returns>
        public static Boolean UpdateObject(RootElement sendObj)
        {
            IRestResponse response;
            String typeName = sendObj.GetType().FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName + "/" + sendObj.id;

            // Serialize to JSON
            String serializedObj = JsonConvert.SerializeObject(sendObj, _jsonSettings);
            try
            {
                response = SendObjectRequest(url, Method.PUT, serializedObj);
            }
            catch (BasicException)
            {
                throw;
            }
            return  response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        ///     Create an object on the server, with HTTP-Method POST. Path is: 'resource/<typename>'
        /// </summary>
        /// <typeparam name="O">The type of the object to be created</typeparam>
        /// <param name="sendObj">The specified object to create</param>
        /// <returns>True if it worked, false/exception otherwise</returns>
        public static Boolean PostObject<O>(RootElement sendObj) where O : new()
        {
            IRestResponse response;
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName;
            
            // Serialize to JSON
            String serializedObj = JsonConvert.SerializeObject(sendObj, _jsonSettings);

            try
            {
                response = SendObjectRequest(url, Method.POST, serializedObj);
            }
            catch (BasicException)
            {
                throw;
            }
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        ///     Delete an object on the server, with HTTP-Method DEL. Path is: 'resource/<typename>/<resId>'
        /// </summary>
        /// <typeparam name="O">Type of the deleted object</typeparam>
        /// <param name="id">Id of the deleted object</param>
        /// <returns>The deleted object</returns>
        public static O DeleteObject<O>(int id) where O : new()
        {
            IRestResponse response;
            String typeName = typeof(O).FullName.Split('.').Last().ToLower();
            String url = _ressourceParam + typeName + "/" + id;
            try
            {
                response = GetObjectRequest<O>(url, Method.DELETE);
            }
            catch (BasicException)
            {
                throw;
            }

            //Deserialize JSON
            O desObj = JsonConvert.DeserializeObject<O>(response.Content, _jsonSettings);

            return desObj;
        }

        /// <summary>
        ///     Does a login access to the server. Path ist always: '/command/user/login'
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>True if it worked, false otherwhise, or an exception</returns>
        public static Boolean checkUser(String username, SecureString password)
        {
            IRestResponse resp;
            String url = _operationParam + "user/" + "login";

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);

            try
            {
                resp = SendSimpleRequest(request);
            }
            catch (BasicException)
            {
                throw;
            }

            return resp.StatusCode == HttpStatusCode.OK;
        }


        /// <summary>
        ///     Sends a request to the server to start a workflow (create an item). Path is always: '/command/workflow/start/{workflowId}/{userName}'
        /// </summary>
        /// <param name="wId">Workflow-Id</param>
        /// <param name="uId">Username</param>
        public static Boolean StartWorkflow(int wId, string username)
        {
            IRestResponse resp;
            String url = _operationParam + "workflow/"+ "start/" + wId + "/" + username;

            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");

            try
            {
                resp = SendSimpleRequest(request);
            }
            catch (BasicException)
            {
                throw;
            }
            
            return resp.StatusCode == HttpStatusCode.OK ;
        }

        /// <summary>
        ///     Sends a state change of an action to the server. Path is always:  '/command/workflow/forward/{stepId}/{itemId}/{username}'
        /// </summary>
        /// <param name="stepId">Id of the current step</param>
        /// <param name="itemId">Id of the current item</param>
        /// <param name="uId">Name of the current user</param>
        /// <returns>True if it worked, false/exception otherwise</returns>
        public static Boolean StepForward(int stepId, int itemId, string username)
        {
            IRestResponse resp;
            String url = _operationParam + "workflow/" + "forward/" + stepId + "/" + itemId + "/" + username;
            
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "text/plain");
            // Parameters are set in URL; alternative is adding them to the request
            //request.AddParameter("username", username, ParameterType.GetOrPost);
            //request.AddParameter("password", password, ParameterType.GetOrPost);

            try
            {
                resp = SendSimpleRequest(request);
            }
            catch (BasicException)
            {
                throw;
            }
            return resp.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        ///     Sends a HTTP-Request to the server to get an object.
        /// </summary>
        /// <typeparam name="O">Type of the requested object</typeparam>
        /// <param name="url">Requested url</param>
        /// <param name="method">HTTP-Method of the request</param>
        /// <returns>Response object from server</returns>
        private static IRestResponse GetObjectRequest<O>(String url, RestSharp.Method method) where O : new()
        {
            var request = new RestRequest(url, method);
            request.AddHeader("Accept", "text/plain");
            IRestResponse response;

            // decide wether the server does return the right excepted object or throws an exception
            try
            {
                response = client.Execute(request);
                System.Diagnostics.Trace.WriteLine("response: " + response.Content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Data + " / " + ex.Message);
                // this has to be a HttpException with the Connection
                throw new Exception(ex.Message);
            }

            // test the StatusCode of response; if 500 happened there is a Server Error
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode == HttpStatusCode.InternalServerError)
            {
                int errorCode = Int32.Parse(response.Content);
                BasicException ex = (BasicException)Activator.CreateInstance(ErrorMessageMapper.GetErrorType(errorCode));
                System.Diagnostics.Trace.WriteLine("errorCode: " + errorCode);
                throw ex;
            }
            return response;
        }

        /// <summary>
        ///     Method to send a C# Object to server.
        /// </summary>
        /// <param name="url">Request url</param>
        /// <param name="method">Method of the request</param>
        /// <param name="serializedObjPath">JSON string serialized object</param>
        /// <returns>The response from server</returns>
        private static IRestResponse SendObjectRequest(String url, RestSharp.Method method, String serializedObj)
        {
            IRestResponse response;
            var request = new RestRequest(url, method);
            request.AddHeader("Accept", "text/plain");

            // if there is an object that shall be send to server
            if (serializedObj != null)
            {
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddParameter("data", serializedObj, ParameterType.GetOrPost);
            }

            // decide wether the server does return the right excepted object or throws an exception
            try
            {
                response = client.Execute(request);
            }
            catch (Exception)
            {
                // this has to be a HttpException with the Connection
                throw new ConnectionException();
            }

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode == HttpStatusCode.InternalServerError)
            {
                int errorCode = Int32.Parse(response.Content);
                BasicException ex = (BasicException)Activator.CreateInstance(ErrorMessageMapper.GetErrorType(errorCode));
                System.Diagnostics.Trace.WriteLine("errorCode: " + errorCode);
                throw ex;
            }
            return response;
        }

        /// <summary>
        ///     Sends a simple request, just containing some information in the url. No more parameters or objects send in the request.
        /// </summary>
        /// <param name="request">The request to send to server</param>
        /// <returns>The response object</returns>
        private static IRestResponse SendSimpleRequest(RestRequest request)
        {
            IRestResponse response;
            try
            {
                response = client.Execute(request);
            }
            catch(Exception)
            {
                // this has to be a HttpException with the Connection
                throw new ConnectionException();
            }

            // if no HttpException happened and although the StatusCode is not "OK", there must be on Exception of our own
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode == HttpStatusCode.InternalServerError)
            {
                int errorCode = Int32.Parse(response.Content);
                BasicException ex = (BasicException)Activator.CreateInstance(ErrorMessageMapper.GetErrorType(errorCode));
                System.Diagnostics.Trace.WriteLine("errorCode: " + errorCode);
                throw ex;
            }
            return response;
        }
    }
}