using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// SSH上传资源
/// </summary>
public class SSHUpload
{

    private static SSHUpload _Instance;
    public static SSHUpload Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SSHUpload();
            }
            return _Instance;
        }
    }

    public void Run()
    {
        string host = "10.1.5.110";
        int port = 2222;
        string username = "fengzeng";
        string password = "Zhmzhm123";
        string rsaKey = "C:/Users/FengZeng/.ssh/id_rsa.pub";

        var connectionInfo = new ConnectionInfo(host, port,
                                        username,
                                        new PasswordAuthenticationMethod(username, password),
                                        new PrivateKeyAuthenticationMethod(rsaKey));
        using (var client = new SshClient(connectionInfo))
        {
            client.Connect();
            Console.WriteLine("IsConnected:" + client.IsConnected);

            SshCommand cmd = client.RunCommand("h");
            Console.WriteLine("cmd:" + cmd.Result);

        }

        //    using (var sshClient = new SshClient(host, port, username, password))

        //    {
        //        byte[] expectedFingerPrint = new byte[] {
        //                                        0x66, 0x31, 0xaf, 0x00, 0x54, 0xb9, 0x87, 0x31,
        //                                        0xff, 0x58, 0x1c, 0x31, 0xb1, 0xa2, 0x4c, 0x6b
        //                                    };

        //        sshClient.HostKeyReceived += (sender, e) =>
        //        {
        //            if (expectedFingerPrint.Length == e.FingerPrint.Length)
        //            {
        //                for (var i = 0; i < expectedFingerPrint.Length; i++)
        //                {
        //                    if (expectedFingerPrint[i] != e.FingerPrint[i])
        //                    {
        //                        //e.CanTrust = false;
        //                        //break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //e.CanTrust = false;
        //            }


        //            using (var cmd = sshClient.CreateCommand("1s"))
        //            {
        //                var res = cmd.Execute();
        //                Console.WriteLine(res);
        //            }
        //        };
        //        sshClient.Connect();

        //        //using (var cmd = sshClient.CreateCommand("1s"))
        //        //{
        //        //    var res = cmd.Execute();
        //        //    Console.WriteLine(res);
        //        //}
        //    }
    }
}