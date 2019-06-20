using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 资源版本包管理器
/// </summary>
public class ResourceVersionPackageManager
{
    private static ResourceVersionPackageManager _Instance;
    public static ResourceVersionPackageManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = new ResourceVersionPackageManager();
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
        if (versionSetting.isAutoAdd)
        {
            currentVerData.version.Add();
        }

        readCompareVersionData();

        generateAll();
        generatePatch();


        versionSetting.lastVersion.CopyValue(currentVerData.version);
        if (versionSetting.isAutoAdd && versionSetting.isSetLastToNextCompera)
        {
            versionSetting.compareVersion.CopyValue(currentVerData.version);
        }
        versionSetting.Save();

        if (versionSetting.isDeleteVerFolderAll)
        {
            Directory.Delete(currentVerData.ver_all, true);
            Console.WriteLine("开始 删除目录 :" + currentVerData.ver_all);
        }


        if (versionSetting.isDeleteVerFolderPatch)
        {
            Directory.Delete(currentVerData.ver_patch, true);
            Console.WriteLine("开始 删除目录 :" + currentVerData.ver_patch);
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

        Console.WriteLine("开始 生成All :" + ver_all);
        if (Directory.Exists(ver_all))
        {
            Directory.Delete(ver_all, true);
        }




        string[] files = Directory.GetFiles(Setting.Options.sourceRoot, "",  SearchOption.AllDirectories);
        foreach(string file in files)
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



            Console.WriteLine(relativePath + "; " + destPath);
        }

        currentVerData.manifest.SaveTo(currentVerData.ver_manifest_all);
        currentVerData.manifest.SaveTo(currentVerData.ver_manifest2);
    }

    /// <summary>
    /// 生成版本文件 Patch
    /// </summary>
    public void generatePatch()
    {
        string ver_all = currentVerData.ver_all;
        string ver_patch = currentVerData.ver_patch;

        Console.WriteLine("开始 生成Patch :" + ver_patch);
        if (Directory.Exists(ver_patch))
        {
            Directory.Delete(ver_patch, true);
        }


        foreach (var kvp in currentVerData.manifest.dict)
        {
            if(!compareVersionData.manifest.ItemEquals(kvp.Key, kvp.Value))
            {
                string srcPath = ver_all + kvp.Value;
                string absoluteDestPath = ver_patch + kvp.Value;

                currentVerData.manifestPatch.Add(kvp.Key, kvp.Value);
                CopyCommand.CopyFile(srcPath, absoluteDestPath, false);
            }
        }

        currentVerData.manifest.SaveTo(currentVerData.ver_manifest_patch);
        currentVerData.manifestPatch.SaveTo(currentVerData.ver_manifestPatch);

        if(versionSetting.isZipVerFolderPatch)
        {
            Console.WriteLine("开始 压缩 :" + currentVerData.ver_patch_zip);
            ZipHelper.ZipDirectory(ver_patch, currentVerData.ver_patch_zip);
        }
    }


    
}
