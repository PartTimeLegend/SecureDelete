using System;
using System.IO;
using System.Security.Cryptography;
[assembly: CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(true)]
namespace SecureDelete
{
    public class SecureDelete
    {
        public bool Delete(string filePath, int timesToWrite)
        {
            if (File.Exists(filePath))
            {
                double sectors = Math.Ceiling((double) (new FileInfo(filePath).Length / 512));

                byte[] emptyBytes = new byte[1024];

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                // Fix RO files
                File.SetAttributes(filePath, FileAttributes.Normal);
                FileStream inputStream = new FileStream(filePath, FileMode.Open);
                
                for (int currentPass = 0; currentPass < timesToWrite; currentPass++)
                {
                    inputStream.Position = 0;
                    for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                    {
                        rng.GetBytes(emptyBytes);
                        inputStream.Write(emptyBytes, 0, emptyBytes.Length);
                    }
                }

                inputStream.SetLength(0);
                inputStream.Close();
                File.Delete(filePath);
                return !File.Exists(filePath);
            }
            throw new FileNotFoundException("The file at " + filePath + "was not found");
        }
    }
}