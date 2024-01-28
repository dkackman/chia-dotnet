using System;
using System.Security.Cryptography.X509Certificates;

namespace chia.dotnet
{
    /// <summary>
    /// Information about how to connect and authenticate with the RPC endpoint
    /// </summary>
    /// <remarks>Using <see cref="CertPath"/>/<see cref="KeyPath"/> vs <see cref="Cert"/>/<see cref="Key"/> are independent of each other</remarks>
    public record EndpointInfo
    {
        /// <summary>
        /// The <see cref="Uri"/> of the RPC endpoint
        /// </summary>
        public Uri Uri { get; init; } = new("http://localhost");

        /// <summary>
        /// The full file system path to the public certificate used to authenticate with the endpoint (.crt)
        /// </summary>
        public string CertPath { get; init; } = string.Empty;

        /// <summary>
        /// The full file system path to the base64 encoded RSA private key to authenticate with the endpoint (.key)
        /// </summary>
        public string KeyPath { get; init; } = string.Empty;

        /// <summary>
        /// The loaded cert as base 64 encoded blob  
        /// </summary>
        public string Cert { get; init; } = string.Empty;

        /// <summary>
        /// The loaded key as base 64 encoded blob  
        /// </summary>
        public string Key { get; init; } = string.Empty;

        public X509Certificate2Collection GetCert()
        {
            // if the cert blobs are empty get certs from the file paths
            // otherwise use the blobs
            return string.IsNullOrEmpty(Cert) && string.IsNullOrEmpty(Key)
                ? CertLoader.GetCertFromFiles(CertPath, KeyPath)
                : CertLoader.DeserializeCert(Cert, Key);
        }
    }
}
