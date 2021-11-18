// ###############################################################################
// 
// источник: https://www.calabonga.net/blog/post/self-signed-certificate-on-csharp
// 
// ###############################################################################

using System;

namespace WebNet5Auth.GenerateCertificate
{
    public class GenerateCertificateOptions
    {
        public GenerateCertificateOptions(string pathToSave, string commonName, string fileName, string password, int years = 1)
        {
            CommonName = commonName;
            if (string.IsNullOrEmpty(CommonName))
            {
                throw new ArgumentNullException(nameof(CommonName));
            }

            PathToSave = pathToSave;
            if (string.IsNullOrEmpty(PathToSave))
            {
                throw new ArgumentNullException(nameof(PathToSave));
            }

            Password = password;
            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentNullException(nameof(Password));
            }

            CertificateFileName = fileName;
            if (string.IsNullOrEmpty(CertificateFileName))
            {
                throw new ArgumentNullException(nameof(CertificateFileName));
            }

            Years = years;
            if (Years <= 0)
            {
                Years = 1;
            }
        }

        public string CommonName { get; }
        public string PathToSave { get; }
        public string Password { get; }
        public string CertificateFileName { get; }
        public int Years { get; }
    }
}
