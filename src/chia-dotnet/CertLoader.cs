using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace chia.dotnet
{
    static class CertLoader
    {
        public static X509Certificate2 GetCert(string certPath, string keyPath)
        {
            using X509Certificate2 pubOnly = new(certPath);

            using StreamReader streamReader = new(keyPath);
            string base64 = new StringBuilder(streamReader.ReadToEnd())
                .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .ToString();

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64), out _);

            using X509Certificate2 certWithKey = pubOnly.CopyWithPrivateKey(rsa);

            return new X509Certificate2(certWithKey.Export(X509ContentType.Pkcs12));
        }
    }
}
