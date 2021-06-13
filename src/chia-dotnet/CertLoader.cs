using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace chia.dotnet
{
    /// <summary>
    /// Helper class for loading certificates
    /// </summary>
    static class CertLoader
    {
        /// <summary>
        /// Constructs an ephemeral <see cref="X509Certificate2"/> from crt and keys stored as files
        /// </summary
        /// <param name="certPath">The full path the the .crt public cert</param>
        /// <param name="keyPath">The full path to the RSA encoded private key</param>
        /// <returns>An ephermal certificate that can be used for WebSocket authentication</returns>
        public static X509Certificate2Collection GetCerts(string certPath, string keyPath)
        {
            using X509Certificate2 cert = new(certPath);

            using StreamReader streamReader = new(keyPath);
            string base64 = new StringBuilder(streamReader.ReadToEnd())
                .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .ToString();

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64), out _);

            using X509Certificate2 certWithKey = cert.CopyWithPrivateKey(rsa);

            var ephermeralCert = new X509Certificate2(certWithKey.Export(X509ContentType.Pkcs12));
            return new X509Certificate2Collection(ephermeralCert);
        }
    }
}
