using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;

public class Options
{
    // 运行完，是否自动关闭cmd
    [Option("autoEnd", Required = false, Default = true)]
    public bool autoEnd { get; set; }

    // 命令
    [Option("cmd", Required = false, Default = "UpdateManifest")]
    public string cmd { get; set; }

    // 启动参数设置 配置路径
    [Option("optionSetting", Required = false, Default = "./ResourceVersionPackage.json")]
    public string optionSetting { get; set; }



    // 源 资源目录
    [Option("sourceRoot", Required = false, Default = "E:/zengfeng/GameWP/client/Game/bin/")]
    public string sourceRoot { get; set; }



    // 版本 资源目录
    [Option("versionRoot", Required = false, Default = "E:/wamp64/www/wp.client_ver_res/wxgame")]
    public string versionRoot { get; set; }




    // 忽略文件
    [Option("ignorePaths", Required = false, Default = new string[] {
        "config/",
        "index.html",
        "game.js",
        "game.json",
        "version.json",
        "project.config.json",
        "project.swan.json",
        "swan-game-adapter.json",
        "swan-game-adapter.js",
        "unpack.json",
        "unpack.json",
        "weapp-adapter.js",
        "res/unity/",
        "res/threeDimen/",
        "res/fgui/__ResImageTmp"
    })]
    public string[] ignorePaths { get; set; }

    // 忽略文件后缀
    [Option("ignoreExes", Required = false, Default = new string[] { ".map" })]
    public string[] ignoreExes { get; set; }

    public VersionInfoConfig versionConfig = new VersionInfoConfig();

    /// <summary>
    /// 是否是忽略文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool isIgnorePath(string path)
    {
        path = path.Replace(sourceRoot, "");
        foreach(string p in ignorePaths)
        {
            if (path.IndexOf(p) == 0)
                return true;
        }

        string exp = Path.GetExtension(path);

        if (string.IsNullOrEmpty(exp))
        {
            return false;
        }

        foreach (string e in ignoreExes)
        {
            if (exp == e)
                return true;
        }
        return false;

    }


    public void Save(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./ReleaseLaya.json";

        string json = JsonHelper.ToJsonType(this);
        File.WriteAllText(path, json);
    }

    public static Options Load(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./ReleaseLaya.json";

        string json = File.ReadAllText(path);
        Options options = JsonHelper.FromJson<Options>(json);
        return options;
    }


}


/// <summary>
/// 版本目录设置
/// </summary>
public class VersionInfoConfig
{
    // 是否保存 all目录
    public bool ver_xx_all_isSave = true;
    public string ver_xx_all = "ver_{0}_{1}_{2}_{3}_all";
    public string ver_xx_patch = "ver_{0}_{1}_{2}_{3}_patch";
    public string ver_manifest = "version.json";
    public string ver_manifestPatch = "version-patch.json";
}