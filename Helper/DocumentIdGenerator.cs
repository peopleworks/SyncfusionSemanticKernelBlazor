using System;
using System.Security.Cryptography;
using System.Text;

namespace SyncfusionSemanticKernelBlazor.Helper
{
    public static class DocumentIdGenerator
    {
        public static string GenerateDocumentId(string documentName)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the SHA256 hash of the document name
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(documentName));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}
