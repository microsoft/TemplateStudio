using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Extensions
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this System.Threading.Tasks.Task t)
        {
        }
    }

    public static class StringExtensions
    {
        public static string Obfuscate(this string data)
        {
            string result = data;
            byte[] b64data = Encoding.UTF8.GetBytes(data);

            using (MD5 md5Hash = MD5.Create())
            {
                result = GetMd5Hash(md5Hash, b64data);
            }
            return result.ToUpper();
        }

        static string GetMd5Hash(MD5 md5Hash, byte[] inputData)
        {
            byte[] data = md5Hash.ComputeHash(inputData);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }

}
