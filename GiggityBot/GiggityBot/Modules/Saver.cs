using GiggityBot.Modules;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public class Saver
{

    private string dataPath;

    private bool secondAttempt = false;

    private BinaryFormatter binaryFromatter = new BinaryFormatter();

    //public string dataPath;
    //internal string hashPath;

    public enum DataState
    {
        ok,
        notfound,
        corrupt,
        busy
    }

    public Saver(string dataPath) => this.dataPath = dataPath;

    public (QuagData data, DataState state) Load()
    {
        try
        {
            if (!File.Exists(dataPath))
                return (null, DataState.notfound);

            FileStream dataStream = new FileStream(dataPath, FileMode.Open);
            QuagData data = (QuagData)binaryFromatter.Deserialize(dataStream);
            return (data, DataState.ok);
        }
        catch (FileNotFoundException ex)
        {
            return (null, DataState.notfound);
        }
        catch (Exception ex)
        {
            return (null, DataState.corrupt);
        }
    }

    public void Save(QuagData data)
    {
        try
        {
            if (File.Exists(dataPath))
                File.Delete(dataPath);

            FileStream dataStream = new FileStream(dataPath, FileMode.Create);
            binaryFromatter.Serialize(dataStream, data);

            dataStream.Close();
        }
        catch (Exception ex)
        {
            if (!secondAttempt)
            {
                Console.WriteLine("Save failed, retrying...");
                secondAttempt = true;
                Save(data);
                return;
            }
            else
                Console.WriteLine("Save failed. " + ex.ToString());
        }
    }

    internal void DeleteSave(string dataPath)
    {
        if (File.Exists(dataPath))
            File.Delete(dataPath);
    }
}