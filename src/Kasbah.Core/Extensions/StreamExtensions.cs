using System;
using System.IO;

namespace Kasbah.Core
{
    public static class StreamExtensions
    {
        #region Public Methods

        public static string SHA1(this Stream stream)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        public static byte[] ToArray(this Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        #endregion
    }
}