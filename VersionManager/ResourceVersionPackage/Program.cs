using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        //注册EncodeProvider
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);



        Setting.Init(args);

        //Setting.cmd = CmdType.SSHUpload;

        switch (Setting.cmd)
        {
            // 生成资源版本 md5码
            case CmdType.GenerateManifest:
                ResourceVersionPackageManager.Instance.Run();
                break;
            // 刷新资源清单
            case CmdType.UpdateManifest:
                UpdateResourceVersionPackageManager.Instance.Run();
                break;

            // 上传资源
            case CmdType.SSHUpload:
                SSHUpload.Instance.Run();
                break;
        }

        Console.WriteLine("完成!");

        if (!Setting.Options.autoEnd)
            Console.Read();

    }


}