
using System.Security.Cryptography;
using System.Text;

static public class FileFolderHelper
{

    /// <summary>
    /// this method will return all the files in the patch
    /// </summary>
    /// <param name="path">where to look for the files</param>
    /// <returns>list of file names</returns>
    static public List<string> GetFilesInfolder(string path)
    {
        return Directory.EnumerateFiles(path).ToList();
    }

    /// <summary>
    /// this method will return all the files in the patch, ordered by creation date
    /// </summary>
    /// <param name="path">where to look for the files</param>
    /// <returns>list of file names</returns>
    static public List<string> GetFilesInfolderByDate(string path)
    {
        DirectoryInfo info = new(path);
        List<FileInfo> fileInfos = info.GetFiles().OrderBy(p => p.CreationTime).ToArray().ToList();
        var orderedFiles = fileInfos.OrderBy(f => f.CreationTime);
        var t = from a in orderedFiles
                where a.Attributes != FileAttributes.Directory
                select a.FullName;
        return t.ToList();
    }

    /// <summary>
    /// this method will return all the directories in the patch
    /// </summary>
    /// <param name="path">where to look for the directories</param>
    /// <returns>list of directory names</returns>
    static public List<string> GetDirectoriesInfolder(string path)
    {
        List<string> result = new();
        //ensure the path is a directory
        if (Directory.Exists(path))
        {
            return Directory.GetDirectories(path).ToList();
        }
        return result;
    }

    /// <summary>
    /// This method will look to see if the directory exists and if forced,
    /// will create the directory if the directory is not there
    /// </summary>
    /// <param name="directoryName">name of the directory to look up</param>
    /// <param name="forced">If forced = true, the directory will be created</param>
    static public void DirectoryExists(string directoryName, bool forced)
    {
        if (!Directory.Exists(directoryName))
        {
            if(forced)
                Directory.CreateDirectory(directoryName);
        }
    }

    /// <summary>
    /// This method will force a file copy.  if the file already exists in the
    /// destination location, it will be over written
    /// </summary>
    /// <param name="oldName">Source file</param>
    /// <param name="newname">destination file</param>
    /// <returns>true if it was able to move it, else false</returns>
    static public bool ForceFileCopy(string oldName, string newname)
    {
        bool result = false;
        try
        {
            if (oldName.IsEmpty() || newname.IsEmpty())
                return result;

            //copy the file with the new name
            if (File.Exists(newname))
            {
                //delete the old one
                File.Delete(newname);
            }
            File.Copy(oldName, newname);
            return result;
        }
        catch
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// this method will force a file move.  if the file already exists in the
    /// destination location, it will be over written
    /// </summary>
    /// <param name="oldName">Source file</param>
    /// <param name="newname">destination file</param>
    /// <returns>true if it was able to move it, else false</returns>
    static public bool ForceFileMove(string oldName, string newname)
    {
        bool result = false;
        if (oldName.IsEmpty() || newname.IsEmpty())
            return result;

        try
        {
            //copy the file with the new name
            if (File.Exists(newname))
            {
                //delete the old one
                File.Delete(newname);
            }
            File.Move(oldName, newname);
        }
        catch
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// This method will rename a file with a GUID
    /// </summary>
    /// <param name="oldName">source file</param>
    /// <returns>new file name</returns>
    static public string RenameFileWithGuid(string oldName)
    {
        string result = "";
        if (oldName != null)
        {
            string dir = Path.GetDirectoryName(oldName)!;
            if(dir == null)
            {
                dir = string.Empty;
            }
            result = Path.Combine(dir, Guid.NewGuid() + Path.GetExtension(oldName));
            try
            {
                if (!ForceFileMove(oldName, result))
                {
                    result = "";
                    return result;
                }
            }
            catch
            {
                result = "";
            }
        }
        return result;
    }

    /// <summary>
    /// If the directory does not exists, this method will create it
    /// </summary>
    /// <param name="directory"></param>
    static public void CreateDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// If the folder exists, this method will delete all files.  If it does 
    /// not exists, it will create it
    /// </summary>
    /// <param name="newDir"></param>
    public static void CreateCleanTempFolder(string newDir)
    {
        if (Directory.Exists(newDir))
        {
            FileFolderHelper.CleanFolder(newDir);
        }
        else
        {
            FileFolderHelper.CreateDirectory(newDir);
        }
    }

    /// <summary>
    /// This method will transverse a folder and delete all files and sub folders
    /// </summary>
    /// <param name="directory">root folder</param>
    /// <returns>true if successful, else false</returns>
    static public bool CleanFolder(string directory)
    {
        bool result = false;
        try
        {
            if (Directory.Exists(directory))
            {
                //clean it
                DirectoryInfo downloadedMessageInfo = new(directory);

                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                result = true;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// This method will delete all sub folders and all the files in those sub 
    /// folders
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    static public bool CleanFolderAndRemove(string directory)
    {
        bool result = false;
        try
        {
            if (Directory.Exists(directory))
            {
                //clean it
                DirectoryInfo downloadedMessageInfo = new(directory);

                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(directory);
                result = true;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// This method will return the folder last written to
    /// </summary>
    /// <param name="path">root folder</param>
    /// <returns>name of the last folder written to</returns>
    public static string GetLatestFolder(string path)
    {
        string result = "";

        if (path == null || !Directory.Exists(path))
            return result;

        DateTime lastHigh = new (1900, 1, 1);
        foreach (string subdir in Directory.GetDirectories(path))
        {
            DirectoryInfo fi1 = new(subdir);
            DateTime created = fi1.LastWriteTime;

            if (created > lastHigh)
            {
                result = subdir;
                lastHigh = created;
            }
        }
        return result;
    }

    /// <summary>
    /// This method will create a new output file that is encrypted
    /// </summary>
    /// <param name="inputFile">The file to encrypt</param>
    /// <param name="outputFile">The encrypted file</param>
    static public void EncryptFile(string inputFile, string outputFile)
    {
        const string skey = "B@tM@n9898";
#pragma warning disable SYSLIB0022 // Type or member is obsolete
        using RijndaelManaged aes = new();
        byte[] key = Encoding.UTF8.GetBytes(skey);

        byte[] iv = Encoding.UTF8.GetBytes(skey);

        using FileStream fsCrypt = new(outputFile, FileMode.Create);
        using ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
        using CryptoStream cs = new(fsCrypt, encryptor, CryptoStreamMode.Write);
        using FileStream fsIn = new(inputFile, FileMode.Open);
        int data;
        while ((data = fsIn.ReadByte()) != -1)
        {
            cs.WriteByte((byte)data);
        }
#pragma warning restore SYSLIB0022 // Type or member is obsolete
    }

    /// <summary>
    /// This method is used to decrypt a file that was encrypted by this utility
    /// </summary>
    /// <param name="inputFile">The encrypted file</param>
    /// <param name="outputFile">The unencrypted file</param>
    static public void DecryptFile(string inputFile, string outputFile)
    {
        const string password = @"B@tM@n9898";

        UnicodeEncoding ue = new();
        byte[] key = ue.GetBytes(password);

        FileStream fsCrypt = new(inputFile, FileMode.Open);

#pragma warning disable SYSLIB0022 // Type or member is obsolete
        RijndaelManaged rmCrypto = new();
#pragma warning restore SYSLIB0022 // Type or member is obsolete

        CryptoStream cs = new(fsCrypt,
            rmCrypto.CreateDecryptor(key, key),
            CryptoStreamMode.Read);

        FileStream fsOut = new(outputFile, FileMode.Create);

        int data;
        while ((data = cs.ReadByte()) != -1)
            fsOut.WriteByte((byte)data);

        fsOut.Close();
        cs.Close();
        fsCrypt.Close();
    }
}
