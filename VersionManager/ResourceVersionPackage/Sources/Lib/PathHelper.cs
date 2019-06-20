using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PathHelper
{
    // 绝对路径 转 相对路径
    public static string RelativePath(string absolutePath, string relativeTo)
    {
        //from - www.cnphp6.com

        string[] absoluteDirectories = absolutePath.Split('/');
        string[] relativeDirectories = relativeTo.Split('/');

        //Get the shortest of the two paths
        int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

        //Use to determine where in the loop we exited
        int lastCommonRoot = -1;
        int index;

        //Find common root
        for (index = 0; index < length; index++)
            if (absoluteDirectories[index] == relativeDirectories[index])
                lastCommonRoot = index;
            else
                break;

        //If we didn't find a common prefix then throw
        if (lastCommonRoot == -1)
            throw new ArgumentException("Paths do not have a common base");

        //Build up the relative path
        StringBuilder relativePath = new StringBuilder();

        //Add on the ..
        for (index = lastCommonRoot + 2; index < absoluteDirectories.Length; index++)
            if (absoluteDirectories[index].Length > 0)
                relativePath.Append("../");

        if (lastCommonRoot + 3 == relativeDirectories.Length)
        {
            relativePath.Append("./");
        }

        //Add on the folders
        for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
            relativePath.Append(relativeDirectories[index] + "/");
        relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

        return relativePath.ToString();
    }

    public static void CheckPath(string path, bool isFile = true)
    {
        if (isFile) path = path.Substring(0, path.LastIndexOf('/'));
        string[] dirs = path.Split('/');
        string target = "";

        bool first = true;
        foreach (string dir in dirs)
        {
            if (first)
            {
                first = false;
                target += dir;
                continue;
            }

            if (string.IsNullOrEmpty(dir)) continue;
            target += "/" + dir;
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
        }
    }

    public static string ChangeExtension(string path, string ext)
    {
        string e = Path.GetExtension(path);
        if (string.IsNullOrEmpty(e))
        {
            return path + ext;
        }

        bool backDSC = path.IndexOf('\\') != -1;
        path = path.Replace('\\', '/');
        if (path.IndexOf('/') == -1)
        {
            return path.Substring(0, path.LastIndexOf('.')) + ext;
        }

        string dir = path.Substring(0, path.LastIndexOf('/'));
        string name = path.Substring(path.LastIndexOf('/'), path.Length - path.LastIndexOf('/'));
        name = name.Substring(0, name.LastIndexOf('.')) + ext;
        path = dir + name;

        if (backDSC)
        {
            path = path.Replace('/', '\\');
        }
        return path;
    }


    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}