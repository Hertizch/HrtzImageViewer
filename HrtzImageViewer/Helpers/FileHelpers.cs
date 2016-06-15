namespace HrtzImageViewer.Helpers
{
    public static class FileHelpers
    {
        public static bool IsValidImage(string filename)
        {
            var bic = new BitmapImageCheck();
            return bic.IsExtensionSupported(filename);
        }
    }
}
