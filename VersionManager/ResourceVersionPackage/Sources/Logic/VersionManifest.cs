using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class VersionManifest
{
    public Dictionary<string, string> dict = new Dictionary<string, string>();

    public void Add(string key,  string value)
    {

        dict.Add(key, value);
    }

    public bool ItemEquals(string key, string dest)
    {
        string val;
        dict.TryGetValue(key, out val);
        return dest == val;
    }


    public string ToJson()
    {
        JSONObject result = new JSONObject();
        foreach(var kvp in dict)
        {
            result.Add(kvp.Key, kvp.Value);
        }

        String jsonStr = result.ToString(4);
        return jsonStr;
    }


    public void SaveTo(string path)
    {
        string content = ToJson();

        PathHelper.CheckPath(path);
        File.WriteAllText(path, content);
    }

    public void Save()
    {
        SaveTo(defaultPath);
    }

    static string defaultPath
    {
        get
        {
            return Setting.Options.versionRoot + "/" + Setting.Options.versionConfig.ver_manifest;
        }
    }


    public static VersionManifest Read()
    {
        return Read(defaultPath);
    }


    public static VersionManifest Read(string path)
    {
        if (!File.Exists(path))
            return new VersionManifest();

        string jsonStr = File.ReadAllText(path);
        JSONObject resultPrase =  JSON.Parse(jsonStr).AsObject;

        VersionManifest result = new VersionManifest();
        foreach(var kvp in resultPrase)
        {
            result.Add(kvp.Key, (kvp.Value as JSONString).Value);
        }

        return result;
        

    }
}