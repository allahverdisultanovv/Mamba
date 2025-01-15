namespace MambaMVC.Utilities.Extensions
{
    public static class FileHelper
    {
        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = String.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, fileName);
            File.Delete(path);
        }
    }
}
