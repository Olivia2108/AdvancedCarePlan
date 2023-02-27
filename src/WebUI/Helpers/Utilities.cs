﻿using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using NLog.Fluent;
using System.Security.Cryptography;
using System.Text;

namespace WebUI.Helpers
{
    public  class Utilities
    {
        IConfiguration configuration;

        public Utilities(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Encrypt(string val)
        {
            MemoryStream ms = new MemoryStream();
            // string rsp = "";
            try
            {
                string sharedkeyval = ""; string sharedvectorval = "";
                sharedkeyval = configuration.GetValue<string>("sharedkey");
                sharedkeyval = BinaryToString(sharedkeyval);

                sharedvectorval = configuration.GetValue<string>("sharedvector");
                sharedvectorval = BinaryToString(sharedvectorval);
                byte[] sharedkey = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedkeyval);
                byte[] sharedvector = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedvectorval);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                byte[] toEncrypt = Encoding.UTF8.GetBytes(val);

                CryptoStream cs = new CryptoStream(ms, tdes.CreateEncryptor(sharedkey, sharedvector), CryptoStreamMode.Write);
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                Console.WriteLine(ex.Message.ToString());
                return "";
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string val)
        {
            MemoryStream ms = new MemoryStream();
            //string rsp = "";
            try
            {
                string sharedkeyval = ""; string sharedvectorval = "";

                sharedkeyval = configuration.GetValue<string>("sharedkey");
                sharedkeyval = BinaryToString(sharedkeyval);

                sharedvectorval = configuration.GetValue<string>("sharedvector");
                sharedvectorval = BinaryToString(sharedvectorval);

                byte[] sharedkey = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedkeyval);
                byte[] sharedvector = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedvectorval);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                byte[] toDecrypt = Convert.FromBase64String(val);

                CryptoStream cs = new CryptoStream(ms, tdes.CreateDecryptor(sharedkey, sharedvector), CryptoStreamMode.Write);


                cs.Write(toDecrypt, 0, toDecrypt.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                Console.WriteLine(ex.Message.ToString());
            }
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string BinaryToString(string binary)
        {
            if (string.IsNullOrEmpty(binary))
                throw new ArgumentNullException("binary");

            if ((binary.Length % 8) != 0)
                throw new ArgumentException("Binary string invalid (must divide by 8)", "binary");

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 8)
            {
                string section = binary.Substring(i, 8);
                int ascii = 0;
                try
                {
                    ascii = Convert.ToInt32(section, 2);
                }
                catch (Exception ex)
                {
                    LoggerMiddleware.LogError(ex.Message);
                    throw new ArgumentException("Binary string contains invalid section: " + section, "binary");
                }
                builder.Append((char)ascii);
            }
            return builder.ToString();
        }


    }
}
