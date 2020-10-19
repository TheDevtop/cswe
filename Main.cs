/*
 * Program: C# Written Encryptor
 * Version: 5.0
 * Authors: Thijs Haker
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

static public class Cswe
{
    // Global Variables
    static string MSG_NOARGS = "Error: Cswe -e/-d [KEY] [FILE]!";
    static string MSG_NOFILE = "Error: File Not Found!";
    static string MSG_NOMEM = "Error: Invalid Filesize!";
    static string MSG_NOKEY = "Error: Minimum keysize is 8!";
    static string KEY;
    static int STATUS;

    static void Configure(string localkey)
    {
        KEY = localkey;
        return;
    }
    static int Encrypt(string PATH)
    {
        try
        {
            byte[] FMLCONTENT = File.ReadAllBytes(PATH);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(KEY);
                DES.Key = Encoding.UTF8.GetBytes(KEY);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var VMSTREAM = new MemoryStream())
                {
                    CryptoStream STREAM = new CryptoStream(VMSTREAM, DES.CreateEncryptor(), CryptoStreamMode.Write);
                    STREAM.Write(FMLCONTENT, 0, FMLCONTENT.Length);
                    STREAM.FlushFinalBlock();
                    File.WriteAllBytes(PATH, VMSTREAM.ToArray());
                    Console.WriteLine("Done: " + PATH);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(MSG_NOFILE);
            return 1;
        }
        catch (IOException)
        {
            Console.WriteLine(MSG_NOMEM);
            return 1;
        }
        return 0;
    }

    static int Decrypt(string PATH)
    {
        try
        {
            byte[] FMTCONTENT = File.ReadAllBytes(PATH);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(KEY);
                DES.Key = Encoding.UTF8.GetBytes(KEY);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var VMSTREAM = new MemoryStream())
                {
                    CryptoStream STREAM = new CryptoStream(VMSTREAM, DES.CreateDecryptor(), CryptoStreamMode.Write);
                    STREAM.Write(FMTCONTENT, 0, FMTCONTENT.Length);
                    STREAM.FlushFinalBlock();
                    File.WriteAllBytes(PATH, VMSTREAM.ToArray());
                    Console.WriteLine("Done: " + PATH);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(MSG_NOFILE);
            return 1;
        }
        catch (IOException)
        {
            Console.WriteLine(MSG_NOMEM);
            return 1;
        }
        return 0;
    }
    static void Printstat()
    {
        Console.WriteLine("Exitcode: {0}", STATUS);
        return;
    }
    static int Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine(MSG_NOARGS);
            STATUS = 1;
            Printstat();
            return STATUS;
        } else if (args[1].Length < 8)
        {
            Console.WriteLine(MSG_NOKEY);
            STATUS = 1;
            Printstat();
            return STATUS;
        }

        switch (args[0])
        {
            case "-e":
                Cswe.Configure(args[1]);
                STATUS = Cswe.Encrypt(args[2]);
                break;
            case "-d":
                Cswe.Configure(args[1]);
                STATUS = Cswe.Decrypt(args[2]);
                break;
            default:
                Console.WriteLine(MSG_NOARGS);
                STATUS = 1;
                break;
        }
        Printstat();
        return STATUS;
    }
}