using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace chia.dotnet
{
    /// <summary>
    /// Helper class for loading certificates
    /// </summary>
    internal static class CertLoader
    {
        /// <summary>
        /// Constructs an ephemeral <see cref="X509Certificate2"/> from a crt and key stored as files
        /// </summary>
        /// <param name="certPath">The full path the public cert (.crt)</param>
        /// <param name="keyPath">The full path to the RSA encoded private key (.key)</param>
        /// <returns>An ephemeral certificate that can be used for WebSocket authentication</returns>
        public static X509Certificate2Collection GetCertFromFiles(string certPath, string keyPath)
        {
            certPath = ExpandTilde(certPath);
            if (!File.Exists(certPath))
            {
                throw new FileNotFoundException($"crt file {certPath} not found");
            }

            keyPath = ExpandTilde(keyPath);
            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"key file {keyPath} not found");
            }

            using StreamReader certStreamReader = new(certPath);
            using StreamReader keyStreamReader = new(keyPath);

            return DeserializeCert(certStreamReader.ReadToEnd(), keyStreamReader.ReadToEnd());
        }

        private static string ExpandTilde(string path)
        {
            if (path.StartsWith('~'))
            {
                var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return string.Concat(homeDirectory, path.AsSpan(1));
            }

            return path;
        }

        public static X509Certificate2Collection DeserializeCert(string certBlob, string keyBlob)
        {
            if (string.IsNullOrEmpty(certBlob))
            {
                throw new ArgumentNullException(nameof(certBlob));
            }

            if (string.IsNullOrEmpty(keyBlob))
            {
                throw new ArgumentNullException(nameof(keyBlob));
            }

            using X509Certificate2 cert = new(Encoding.UTF8.GetBytes(certBlob));
            using var rsa = DeserializePrivateKey(keyBlob);
            using var certWithKey = cert.CopyWithPrivateKey(rsa);

            var keyBytes = certWithKey.Export(X509ContentType.Pkcs12);
            var ephemeralX509Cert = new X509Certificate2(keyBytes);
            return new(ephemeralX509Cert);
        }

        private static RSA DeserializePrivateKey(string serializedKey)
        {
            var rsa = RSA.Create();
            if (serializedKey.StartsWith("-----BEGIN RSA PRIVATE KEY-----", StringComparison.Ordinal))
            {
                var base64 = new StringBuilder(serializedKey)
                    .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                    .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                    .ToString();

                var keyBytes = Convert.FromBase64String(base64);
                rsa.ImportRSAPrivateKey(keyBytes, out _);
            }
            else
            {
                var base64 = new StringBuilder(serializedKey)
                    .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                    .Replace("-----END PRIVATE KEY-----", string.Empty)
                    .ToString();

                var keyBytes = Convert.FromBase64String(base64);
                rsa.ImportPkcs8PrivateKey(keyBytes, out _);
            }

            return rsa;
        }
    }
}
