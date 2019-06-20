using System;
using System.Collections.Generic;
using System.Text;

public class VersionData
{
    public VersionStruct version = new VersionStruct();
    public VersionManifest manifest = new VersionManifest();
    public VersionManifest manifestPatch = new VersionManifest();

    public string ver_all
    {
        get
        {
            return Setting.Options.versionRoot + "/" + string.Format(Setting.Options.versionConfig.ver_xx_all, version.master, version.minor, version.revised, version.revised2) + "/";
        }
    }


    public string ver_patch
    {
        get
        {
            return Setting.Options.versionRoot + "/" +  string.Format(Setting.Options.versionConfig.ver_xx_patch, version.master, version.minor, version.revised, version.revised2) + "/";
        }
    }


    public string ver_patch_zip
    {
        get
        {
            return Setting.Options.versionRoot + "/" + string.Format(Setting.Options.versionConfig.ver_xx_patch, version.master, version.minor, version.revised, version.revised2) + ".zip";
        }
    }

    public string ver_manifest_all
    {
        get
        {
            return ver_all + Setting.Options.versionConfig.ver_manifest;
        }
    }

    public string ver_manifest_patch
    {
        get
        {
            return ver_patch + Setting.Options.versionConfig.ver_manifest;
        }
    }


    public string ver_manifestPatch
    {
        get
        {
            return ver_patch + Setting.Options.versionConfig.ver_manifestPatch;
        }
    }


    public string ver_manifest2
    {
        get
        {
            return Setting.Options.versionRoot + "/versionjson/" + $"version-{version.ToString()}.json";
        }
    }


    public string ver_manifestPatch2
    {
        get
        {
            return Setting.Options.versionRoot + "/versionjson/" + $"version-{version.ToString()}-patch.json";
        }
    }

}