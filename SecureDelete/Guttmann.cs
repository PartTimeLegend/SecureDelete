using System.IO;
using System.Security.Cryptography;

namespace SecureDelete
{
    public class Guttmann
    {
        private FileInfo GetFileInfo(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo;
        }

        private void WriteBytesToStream(FileInfo fileInfo, int passes)
        {
            var b = new byte[passes][];

            for (var i = 0; i < b.Length; i++)
            {
                if (i == 14 || i == 19 || i == 25 || i == 26 || i == 27) continue;

                b[i] = new byte[fileInfo.Length];
            }

            using (var rnd = new RNGCryptoServiceProvider())
            {
                for (var i = 0L; i < 4; i++)
                {
                    rnd.GetBytes(b[i]);
                    rnd.GetBytes(b[31 + i]);
                }
            }

            for (var i = 0L; i < fileInfo.Length;)
            {
                b[4][i] = 85;
                b[5][i] = 170;
                b[6][i] = 146;
                b[7][i] = 73;
                b[8][i] = 36;
                b[10][i] = 17;
                b[11][i] = 34;
                b[12][i] = 51;
                b[13][i] = 68;
                b[15][i] = 102;
                b[16][i] = 119;
                b[17][i] = 136;
                b[18][i] = 153;
                b[20][i] = 187;
                b[21][i] = 204;
                b[22][i] = 221;
                b[23][i] = 238;
                b[24][i] = 255;
                b[28][i] = 109;
                b[29][i] = 182;
                b[30][i++] = 219;
                if (i >= fileInfo.Length)
                {
                    continue;
                }

                b[4][i] = 85;
                b[5][i] = 170;
                b[6][i] = 73;
                b[7][i] = 36;
                b[8][i] = 146;
                b[10][i] = 17;
                b[11][i] = 34;
                b[12][i] = 51;
                b[13][i] = 68;
                b[15][i] = 102;
                b[16][i] = 119;
                b[17][i] = 136;
                b[18][i] = 153;
                b[20][i] = 187;
                b[21][i] = 204;
                b[22][i] = 221;
                b[23][i] = 238;
                b[24][i] = 255;
                b[28][i] = 182;
                b[29][i] = 219;
                b[30][i++] = 109;
                if (i >= fileInfo.Length)
                {
                    continue;
                }

                b[4][i] = 85;
                b[5][i] = 170;
                b[6][i] = 36;
                b[7][i] = 146;
                b[8][i] = 73;
                b[10][i] = 17;
                b[11][i] = 34;
                b[12][i] = 51;
                b[13][i] = 68;
                b[15][i] = 102;
                b[16][i] = 119;
                b[17][i] = 136;
                b[18][i] = 153;
                b[20][i] = 187;
                b[21][i] = 204;
                b[22][i] = 221;
                b[23][i] = 238;
                b[24][i] = 255;
                b[28][i] = 219;
                b[29][i] = 109;
                b[30][i++] = 182;
            }

            b[14] = b[4];
            b[19] = b[5];
            b[25] = b[6];
            b[26] = b[7];
            b[27] = b[8];

            Stream stream = new FileStream(
                fileInfo.FullName,
                FileMode.Open,
                FileAccess.Write,
                FileShare.None,
                (int) fileInfo.Length,
                FileOptions.DeleteOnClose | FileOptions.RandomAccess | FileOptions.WriteThrough);

            using (stream)
            {
                for (long i = 0L; i < b.Length; i++)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Write(b[i], 0, b[i].Length);
                    stream.Flush();
                }
            }
        }

        public void Delete(string fileName, int passes)
        {
            FileInfo fileInfo = GetFileInfo(fileName);
            WriteBytesToStream(fileInfo, passes);
        }
    }
}