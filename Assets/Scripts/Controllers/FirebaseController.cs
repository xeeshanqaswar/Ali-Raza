
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Proyecto26;
using System;
using static System.Net.WebRequestMethods;
using Unity.VisualScripting;

public static class FirebaseController
{
    private const string projectId = "nico-the-weather"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = "https://sensoryfususionfordev-default-rtdb.firebaseio.com/";

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(Points user);
    public delegate void GetUsersCallback(Dictionary<string, Points> users);


    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void PostUser(Points user, string userId, PostUserCallback callback)
    {
        RestClient.Put<Points>($"{databaseURL}points/{userId}.json", user).Then(response => { callback(); });
    }
    public static void DeleteUser( string userId,Action callback)
    {
        RestClient.Delete($"{databaseURL}points/{userId}.json").Then(response => { callback(); });
    }

    /// <summary>
    /// Retrieves a user from the Firebase Database, given their id
    /// </summary>
    /// <param name="userId"> Id of the user that we are looking for </param>
    /// <param name="callback"> What to do after the user is downloaded successfully </param>
    public static void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<Points>($"{databaseURL}points/{userId}.json").Then(user => { callback(user); });
    }

    /// <summary>
    /// Gets all users from the Firebase Database
    /// </summary>
    /// <param name="callback"> What to do after all users are downloaded successfully </param>
    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}points.json").Then(response =>
        {
            var responseJson = response.Text;
            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, Points>), ref deserialized);
            var users = deserialized as Dictionary<string, Points>;
            callback(users);
        });
    }
}

[Serializable] // This makes the class able to be serialized into a JSON
public class Points
{
    public Lessonss lessons;
    public Vector3 cameraTransformPosition;
    public Quaternion cameraTransformRotation;
    public float distance;

    public Points(Lessonss lessons, Vector3 cameraTransformPosition, Quaternion cameraTransformRotation, float distance)
    {
        this.lessons = lessons;
        this.cameraTransformPosition = cameraTransformPosition;
        this.cameraTransformRotation = cameraTransformRotation;
        this.distance = distance;
    }
}


[Serializable] // This makes the class able to be serialized into a JSON
public class Lessonss
{
    public string lessonNumber;
    public string QuesitonNumber;
    public Lessonss(string lessonNumber, string QuesitonNumber)
    {
        this.lessonNumber = lessonNumber;
       this.QuesitonNumber = QuesitonNumber;
    }

}


