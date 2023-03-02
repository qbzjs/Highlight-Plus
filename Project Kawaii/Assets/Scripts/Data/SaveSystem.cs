using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

/*
public static class SaveSystem
{
    public static string SaveLocation = Application.persistentDataPath + "/savedGames.gd";
    public static int Version = 1;

    #region Custom Save Location
    public static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".sav");
    }
    #endregion Custom Save Location

    #region Saving
    public static void Save()
    {
        PopupText.ScreenText = "Progress has been saved";

        BinaryFormatter bf = new BinaryFormatter();

        //Vector3 Surrogate Stuff
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
        bf.SurrogateSelector = surrogateSelector;
        //End Of Surrogate Stuff

        FileStream file = File.Create(SaveLocation);

        GameManager.SaveGame();
        bf.Serialize(file, Version);
        bf.Serialize(file, SceneManager.GetActiveScene().buildIndex);
        bf.Serialize(file, GameManager.PlayerLocation);
        bf.Serialize(file, GameManager.PlayerStats);
        bf.Serialize(file, GameManager.EnemyTransforms);
        bf.Serialize(file, GameManager.EnemyStats);
        bf.Serialize(file, GameManager.InteractedItems);
        file.Close();
    }
    #endregion Saving

    #region Loading
    public static bool Load()
    {
        Debug.Log("File exists " + File.Exists(SaveLocation));
        if (File.Exists(SaveLocation))
        {
            BinaryFormatter bf = new BinaryFormatter();

            //Surrogate Stuff
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            bf.SurrogateSelector = surrogateSelector;
            //End Of Surrogate Stuff

            FileStream file = File.Open(SaveLocation, FileMode.Open, FileAccess.ReadWrite);

            if (Version == (int)bf.Deserialize(file))
            {
                GameManager.LevelToLoad = (int)bf.Deserialize(file);
                GameManager.PlayerLocation = (TransformStuff)bf.Deserialize(file);
                GameManager.PlayerStats = (CoreStats)bf.Deserialize(file);
                GameManager.EnemyTransforms = (TransformStuff[])bf.Deserialize(file);
                GameManager.EnemyStats = (CoreStats[])bf.Deserialize(file);
                GameManager.InteractedItems = (bool[])bf.Deserialize(file);
                file.Close();
                PopupText.ScreenText = "Progress has been loaded";
                return true;
            }
            else
            {
                File.Delete(SaveLocation);
                Debug.LogWarning("The saved file was created with a different version so the file was deleted.");
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion Loading

    #region Delete
    public static void Delete()
    {
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }
    }
    #endregion Delete

    #region Vector3Surrogate
    public class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        //Method called to serialize a Vector3 object
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 v3 = (Vector3)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
        }
        //Method called to deserialize a Vector3 object
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v3 = (Vector3)obj;
            v3.x = (float)info.GetValue("x", typeof(float));
            v3.y = (float)info.GetValue("y", typeof(float));
            v3.z = (float)info.GetValue("z", typeof(float));
            obj = v3;
            return obj;
        }
    }
    #endregion Vector3Surrogate
}
*/