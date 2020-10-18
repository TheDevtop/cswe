/*
 * C# Written Ecryptor
 * Version 4.0.2
 * Written by Thijs Haker
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

static public class Cswe
{
    // Global Variables
    static string MSG_NOARGS = "Error: Invalid Arguments Specified!";
    static string MSG_NOFILE = "Error: File Not Found!";
    static string MSG_NOMEM = "Error: Invalid Filesize!";
    static string MSG_VER = "Cswe: 4.0.2-Git";
    static string KEY = "46280174";

    // FMT & FML: Formatted & Formatless
    // DES.IV: Initialization Vector
    // Try() & Catch() used for hacked-up exception handling
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
    static int Main(string[] args)
    {
        int STATUS;
        Console.WriteLine(MSG_VER);
        if (args.Length != 2)
        {
            Console.WriteLine(MSG_NOARGS);
            STATUS = 1;
            Console.WriteLine("Exitcode: {0}", STATUS);
            return STATUS;
        }
        switch (args[0])
        {
            case "-e":
                STATUS = Cswe.Encrypt(args[1]);
                break;
            case "-d":
                STATUS = Cswe.Decrypt(args[1]);
                break;
            default:
                Console.WriteLine(MSG_NOARGS);
                STATUS = 1;
                break;
        }
        Console.WriteLine("Exitcode: {0}", STATUS);
        return STATUS;
    }
}