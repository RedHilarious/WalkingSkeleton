﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationLib.Model;
using System.Security;

namespace CommunicationLib
{
    /// <summary>
    ///  Interface that describes the provided methods of this RestAPI.
    /// </summary>
    public interface IRestRequester
    {
        /// <summary>
        /// Initialize Method for client properties.
        /// </summary>
        /// <param name="username">username of registered user</param>
        /// <param name="password">password of registered user</param>
        void InitializeClientProperties(string username, String password);

        /// <summary>
        /// Reset client properties.
        /// </summary>
        void DeleteClientProperties();

        // RESSOURCE-METHODS - get, post, update, delete - do something on the ressources

        /// <summary>
        ///  Method to retrieve all existent objects of given element on server.
        /// </summary>
        /// <returns>List of all elements</returns>
        IList<O> GetAllElements<O>();

        /// <summary>
        ///  Method to retrieve all startable workflows of one given user. 
        /// </summary>
        /// <returns>List of all startable workflows of this user</returns>
        IList<string> GetStartablesByUser();

        /// <summary>
        ///  Method to retrieve all relevant items of one given user. Relevant means all items where the user can accept or close actions.
        /// </summary>
        /// <param name="workflowID">The actual handled workflow</param>
        /// <returns>List of relevant items</returns>
        IList<Item> GetRelevantItemsByUser(string workflowID);

        /// <summary>
        ///     Get an object from the server, with HTTP-Method GET.
        ///     Path for this HTTP-Method is always: ressource/{typename}/{id}/
        /// </summary>
        /// <typeparam name="O">Type of the requested object</typeparam>
        /// <param name="id">Id of the requested object</param>
        /// <returns>The requested object</returns>
        O GetObject<O>(string id) where O : new();

        /// <summary>
        ///     Update an object on the server, with HTTP-Method PUT. Path if sendObj is Workflow or Item: 'resource/{typename}/{id}', if user:  'resource/{typename}/{username}'
        /// </summary>
        /// <param name="sendObj">The object to update</param>
        /// <returns>If it worked or not</returns>
        Boolean UpdateObject(RootElement sendObj);

        /// <summary>
        ///     Create an object on the server, with HTTP-Method POST. Path is: 'resource/{typename}'
        /// </summary>
        /// <param name="sendObj">The specified object to create</param>
        /// <returns>True if it worked, false/exception otherwise</returns>
        Boolean PostObject(RootElement sendObj);

        /// <summary>
        ///  Delete an object (Item or Workflow) on the server, with HTTP-Method DEL. Path is: 'resource/{typename}/{resId}'
        /// </summary>
        /// <typeparam name="O">Type of the delete object</typeparam>
        /// <param name="id">ID of the object to delete</param>
        /// <returns>The deleted Object</returns>
        O DeleteObject<O>(string id) where O : new();

        // COMMAND-METHODS

        /// <summary>
        ///     Does a login access to the server. Path ist always: '/command/user/login'
        /// </summary>
        /// <returns>True if it worked, false otherwhise, or an exception</returns>
        Boolean CheckUser();

        /// <summary>
        ///     Sends a request to the server to start a workflow (create an item). Path is always: '/command/workflow/start/{workflowId}/{userName}'
        /// </summary>
        /// <param name="wId">Workflow-Id</param>
        Boolean StartWorkflow(string wId);

        /// <summary>
        ///     Sends a state change of an action to the server. Path is always:  '/command/workflow/forward/{stepId}/{itemId}/{username}'
        /// </summary>
        /// <param name="stepId">Id of the current step</param>
        /// <param name="itemId">Id of the current item</param>
        /// <returns>True if it worked, false/exception otherwise</returns>
        Boolean StepForward(string stepId, string itemId);

        /// <summary>
        /// Refreshs the rest client.
        /// </summary>
        /// <param name="serverAddress">new server address</param>
        void Refresh(string serverAddress);
    }
}