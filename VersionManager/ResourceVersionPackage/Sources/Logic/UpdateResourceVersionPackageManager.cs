using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 资源版本包管理器
/// </summary>
public class UpdateResourceVersionPackageManager
{
    private static UpdateResourceVersionPackageManager _Instance;
    public static UpdateResourceVersionPackageManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new UpdateResourceVersionPackageManager();
            }
            return _Instance;
        }
    }



    public VersionSetting versionSetting;
    public VersionData compareVersionData = new VersionData();
    public VersionData currentVerData = new VersionData();

    public void Run()
    {
        versionSetting = VersionSetting.Read();
        currentVerData.version.CopyValue(versionSetting.lastVersion);
        compareVersionData.version.CopyValue(versionSetting.compareVersion);

        string preVerStr = versionSetting.lastVersion.ToString();

        if (versionSetting.isAutoAdd)
        {
            currentVerData.version.Add();
        }

        readCompareVersionData();


        currentVerData.manifest.Add("__ResVersion__", currentVerData.version.ToString());

        generateAll();
        generatePatch();


        versionSetting.lastVersion.CopyValue(currentVerData.version);
        if (versionSetting.isAutoAdd && versionSetting.isSetLastToNextCompera)
        {
            versionSetting.compareVersion.CopyValue(currentVerData.version);
        }
        versionSetting.Save();

        foreach(string path in versionSetting.replaceVersionTextFiles)
        {
            string verStr = currentVerData.version.ToString();

            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                content = content.Replace(versionSetting.replaceVersionText, verStr);
                content = content.Replace(preVerStr, verStr);
                File.WriteAllText(path, content);
            }
        }


        foreach (string path in versionSetting.replaceVersionTextDelayFiles)
        {
            string verStr = currentVerData.version.ToString();

            if (versionSetting.isAutoAdd)
            {
                currentVerData.version.Add();
            }
            string nextVerStr = currentVerData.version.ToString();


            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                content = content.Replace(versionSetting.replaceVersionDelayText, nextVerStr);
                content = content.Replace(verStr, nextVerStr);
                File.WriteAllText(path, content);
            }
        }


    }

    /// <summary>
    /// 读取对比版本信息
    /// </summary>
    public void readCompareVersionData()
    {
        string ver_manifest = compareVersionData.ver_manifest2;
        compareVersionData.manifest = VersionManifest.Read(ver_manifest);
    }

    /// <summary>
    /// 生成版本文件 all
    /// </summary>
    public void generateAll()
    {
        string ver_all = currentVerData.ver_all;
        if (versionSetting.isUseUpdateFolder)
            ver_all = Setting.Options.versionRoot + "/" + versionSetting.updateFolder + "/";

        Console.WriteLine("开始 生成All :" + ver_all);


        Dictionary<string, string> jsList = new Dictionary<string, string>();


        string[] files = Directory.GetFiles(Setting.Options.sourceRoot, "", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            string path = file.Replace('\\', '/');
            string relativePath = path.Replace(Setting.Options.sourceRoot, "");
            if (Setting.Options.isIgnorePath(relativePath))
                continue;




            string md5 = HashHelper.ComputeCRC32(path);
            string destPath = Path.GetDirectoryName(relativePath) + "/" + Path.GetFileNameWithoutExtension(relativePath) + "-" + md5 + Path.GetExtension(relativePath);
            destPath = destPath.Replace('\\', '/');
            if (destPath.IndexOf('/') == 0)
                destPath = destPath.Substring(1);

            currentVerData.manifest.Add(relativePath, destPath);

            string absoluteDestPath = ver_all + destPath;
            CopyCommand.CopyFile(path, absoluteDestPath, false);


            if(versionSetting.isReplaceJsPath)
            {
                string exp = Path.GetExtension(relativePath);
                if (exp == ".js")
                {
                    jsList.Add(relativePath, destPath);
                }
            }

            Console.WriteLine(relativePath + "; " + destPath);
        }

        currentVerData.manifest.SaveTo(ver_all + Setting.Options.versionConfig.ver_manifest );
        currentVerData.manifest.SaveTo(currentVerData.ver_manifest2);

        if (versionSetting.isReplaceJsPath)
        {
            if(jsList.ContainsKey("index.js"))
            {

                string indexHtmlPath = ver_all + "/index.html";
                if (File.Exists(indexHtmlPath))
                {
                    string indexContent = File.ReadAllText(indexHtmlPath);
                    indexContent = indexContent.Replace("\"index.js\"", jsList["index.js"]);
                    File.WriteAllText(indexHtmlPath, indexContent);
                }

                string indexJsPath = ver_all + jsList["index.js"];
                if (File.Exists(indexJsPath))
                {
                    string indexContent = File.ReadAllText(indexJsPath);

                    foreach(var kvp in jsList)
                    {
                        indexContent = indexContent.Replace("\"" + kvp.Key + "\"", "\"" +  kvp.Value + "\"");
                    }


                    File.WriteAllText(indexJsPath, indexContent);

                }

            }





        }
    }

    /// <summary>
    /// 生成版本文件 Patch
    /// </summary>
    public void generatePatch()
    {
        string ver_all = currentVerData.ver_all;


        foreach (var kvp in currentVerData.manifest.dict)
        {
            if (!compareVersionData.manifest.ItemEquals(kvp.Key, kvp.Value))
            {
                currentVerData.manifestPatch.Add(kvp.Key, kvp.Value);
            }
        }

        currentVerData.manifestPatch.SaveTo(currentVerData.ver_manifestPatch2);

    }



}
