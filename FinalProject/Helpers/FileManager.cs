namespace FinalProject.Helpers
{
    public class FileManager
    {
        public static string Save(IFormFile file, string rootpath, string folder)
        {
            string fileName = file.FileName;
            string newFileName = Guid.NewGuid().ToString() + (fileName.Length > 14 ? fileName.Substring(fileName.Length - 14) : fileName);
            string path = Path.Combine(rootpath, folder, newFileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return newFileName;
        }

        public static void Delete(string rootpath, string folder, string filename)
        {
            string path = Path.Combine(rootpath, folder, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
