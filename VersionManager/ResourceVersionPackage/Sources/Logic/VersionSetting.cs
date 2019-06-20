using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class VersionSetting
{
    // 最后一次发布的版本
    public VersionStruct lastVersion = new VersionStruct();
    // 比较版本号
    public VersionStruct compareVersion = new VersionStruct();


    // 版本号是否自动加1
    public bool isAutoAdd = true;
    // 是否将最后一次发布的版本设置为下一次比较版本
    public bool isSetLastToNextCompera = true;

    // 是否删除文件夹 -- all
    public bool isDeleteVerFolderAll = true;
    // 是否删除文件夹 -- patch
    public bool isDeleteVerFolderPatch = true;
    // 是否打包zip
    public bool isZipVerFolderPatch = true;


    // 是否使用updateFolder
    public bool isUseUpdateFolder = true;
    // 更新的资源目录
    public string updateFolder = "ver_01_00_00_00";
    // 是否替换js路径
    public bool isReplaceJsPath = true;

    // 替换版本号 文件
    public string replaceVersionText = "__VERSION__";
    public string[] replaceVersionTextFiles = new string[]{};
    public string replaceVersionDelayText = "__VERSION__";
    public string[] replaceVersionTextDelayFiles = new string[] { "pulish-res-wx--git.bat" };

    public string ToJson()
    {
        return JsonHelper.ToJsonType(this, true);
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
            return Setting.Options.versionRoot + "/VersionSetting.json" ;
        }
    }


    public static VersionSetting Read()
    {
        return Read(defaultPath);
    }


    public static VersionSetting Read(string path)
    {
        if (!File.Exists(path))
            return new VersionSetting();

        string jsonStr = File.ReadAllText(path);

        VersionSetting result = JsonHelper.FromJson<VersionSetting>(jsonStr);

        return result;


    }
}