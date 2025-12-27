namespace UglyToad.PdfPig.Tests.Writer.TestImages
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Helper for loading the CCITT Group 4 TIFF fixture used by PDF page builder tests.
    /// </summary>
    internal sealed class CcittG4TestImage
    {
        private CcittG4TestImage(int width, int height, byte[] rawCcittData, string sourcePath, bool blackIs1)
        {
            Width = width;
            Height = height;
            RawCcittData = rawCcittData;
            SourcePath = sourcePath;
            BlackIs1 = blackIs1;
        }

        public int Width { get; }

        public int Height { get; }

        public byte[] RawCcittData { get; }

        public string SourcePath { get; }

        public bool BlackIs1 { get; }

        /// <summary>
        /// Loads the CCITT Group 4 sample image relative to the test output directory.
        /// </summary>
        public static CcittG4TestImage Load()
        {
            var base64Path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..",
                "Images", "Files", "Tif", "TiffCcittG4.base64"));

            var tiffPath = EnsureTiffMaterialized(base64Path);

            LibTiffSilencer.SuppressWarnings();

            var payload = CcittExtractor.FromTiff(tiffPath);
            return new CcittG4TestImage(payload.Width, payload.Height, payload.Data, tiffPath, payload.BlackIs1);
        }

        private static string EnsureTiffMaterialized(string base64Path)
        {
            if (!File.Exists(base64Path))
            {
                throw new FileNotFoundException(base64Path);
            }

            var targetPath = Path.Combine(Path.GetTempPath(), "PdfPig_TiffCcittG4.tif");

            if (File.Exists(targetPath))
            {
                return targetPath;
            }

            var base64 = File.ReadAllText(base64Path, Encoding.ASCII);
            var bytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(targetPath, bytes);
            return targetPath;
        }
    }
}
