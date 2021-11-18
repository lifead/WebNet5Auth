// ###############################################################################
// 
// источник: https://www.calabonga.net/blog/post/self-signed-certificate-on-csharp
// 
// ###############################################################################

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace WebNet5Auth.GenerateCertificate
{
    class Program
    {
        static void Main()
        {
            var options = new GenerateCertificateOptions
            (
                pathToSave: "d:\\Temp",
                commonName: "IdentityServer4",
                fileName: "IdentityServer4_certificate",
                password: "P@55w0rd",
                10
            );
            MakeCert(options);
        }

        static void MakeCert(GenerateCertificateOptions options)
        {
            var rsa = RSA.Create(4096);
            var req = new CertificateRequest($"cn={options.CommonName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(options.Years));
            var path = Path.Combine(options.PathToSave, options.CertificateFileName);

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes($"{path}.pfx", cert.Export(X509ContentType.Pfx, options.Password));

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText($"{path}.cer",
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }
    }
}
