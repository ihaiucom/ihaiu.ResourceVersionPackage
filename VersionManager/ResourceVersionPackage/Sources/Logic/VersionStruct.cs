using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

public class VersionStruct
{

    // 第一部分为主版本号
    public int master = 0;

	// 第二部分为次版本号, 需要更新app版本
	public int minor = 0;

	// 第三部分为修订版
	public int revised = 0;

	// 第四部分为修订版
	public int revised2 = 0;

    [JsonIgnore]
    public bool isZero
	{
        get
        {
            return master == 0 &&
                minor == 0 &&
                revised == 0 &&
                revised2 == 0;
        }
	}

    public void Add()
    {
        this.revised2++;
    }

    public void Zero()
    {
        master = 0;
        minor = 0;
        revised = 0;
        revised2 = 0;
    }


    public void SetVersionTxt(string versionTxt)
	{
		if (string.IsNullOrEmpty(versionTxt))
			return;

		if (string.IsNullOrEmpty(versionTxt.Trim()))
			return;


        versionTxt = versionTxt.ToLower();
		versionTxt = versionTxt.Replace("version:", "").Replace("version", "").Replace("ver", "").Replace("v", "");

        Zero();

        var arr = versionTxt.Split('.');
		master = arr.GetInt32(0);

		if (arr.Length > 1)
			minor = arr.GetInt32(1);

        if (arr.Length > 2)
			revised = arr.GetInt32(2);

        if (arr.Length > 3)
			revised2 = arr.GetInt32(3);
    }


    public bool Equal(VersionStruct target)
    {
        return this.master == target.master
            && this.minor == target.minor
            && this.revised == target.revised
            && this.revised2 == target.revised2;
    }

    public void CopyValue(VersionStruct target)
    {
        master = target.master;
        minor = target.minor;
        revised = target.revised;
        revised2 = target.revised2;
    }

    public override string ToString()
    {
		return $"v{master.ToString("D2")}.{minor.ToString("D2")}.{revised.ToString("D2")}.{revised2.ToString("D2")}";
    }
}