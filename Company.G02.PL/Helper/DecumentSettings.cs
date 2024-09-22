namespace Company.G02.PL.Helper
{
    public static class DecumentSettings
    {

        public static string UplodeFile(IFormFile file,string FolderName)
        {

            string folderPath= Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", FolderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            string filePath = Path.Combine(folderPath, fileName);

           using var fileStream = new FileStream(filePath, FileMode.Create);


            file.CopyTo(fileStream);

            return fileName;


        }

        public static void DeleteFile (string fileName, string FolderName)
        {
          string  filePath = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot/files" ,FolderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }



    }
}
