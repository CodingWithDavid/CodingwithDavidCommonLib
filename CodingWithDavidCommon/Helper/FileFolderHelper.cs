
using System.Security.Cryptography;
using System.Text;

namespace CodingWithDavid.Common;

/// <summary>Provides file and directory management and password-based file encryption helpers.</summary>
public static class FileFolderHelper
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
        try
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newname))
                return false;

            File.Copy(oldName, newname, true);
            return true;
        }
        catch
        {
            return false;
        }
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
        if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newname))
            return false;

        try
        {
            if (File.Exists(newname))
                File.Delete(newname);

            File.Move(oldName, newname);
            return true;
        }
        catch
        {
            return false;
        }
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
    /// Encrypts a file with AES-GCM using a password-derived key.
    /// </summary>
    /// <param name="inputFile">The file to encrypt</param>
    /// <param name="outputFile">The encrypted file</param>
    /// <param name="password">The password used to derive the encryption key</param>
    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        ValidateEncryptionInputs(inputFile, outputFile, password);

        byte[] plaintext = File.ReadAllBytes(inputFile);
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] nonce = RandomNumberGenerator.GetBytes(12);
        byte[] key = DeriveKey(password, salt);
        byte[] ciphertext = new byte[plaintext.Length];
        byte[] tag = new byte[16];

        using (AesGcm aes = new(key, tag.Length))
        {
            aes.Encrypt(nonce, plaintext, ciphertext, tag);
        }

        using FileStream output = new(outputFile, FileMode.Create, FileAccess.Write);
        output.Write(salt);
        output.Write(nonce);
        output.Write(tag);
        output.Write(ciphertext);
    }

    /// <summary>
    /// Decrypts a file encrypted by <see cref="EncryptFile(string, string, string)"/>.
    /// </summary>
    /// <param name="inputFile">The encrypted file</param>
    /// <param name="outputFile">The unencrypted file</param>
    /// <param name="password">The password used to derive the decryption key</param>
    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        ValidateEncryptionInputs(inputFile, outputFile, password);

        byte[] encrypted = File.ReadAllBytes(inputFile);
        const int headerLength = 16 + 12 + 16;
        if (encrypted.Length < headerLength)
            throw new InvalidDataException("The encrypted file is not a valid Common Library encrypted file.");

        byte[] salt = encrypted[..16];
        byte[] nonce = encrypted[16..28];
        byte[] tag = encrypted[28..44];
        byte[] ciphertext = encrypted[44..];
        byte[] plaintext = new byte[ciphertext.Length];
        byte[] key = DeriveKey(password, salt);

        using (AesGcm aes = new(key, tag.Length))
        {
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
        }

        File.WriteAllBytes(outputFile, plaintext);
    }

    /// <summary>Encrypts a file using the configured legacy environment password.</summary>
    /// <param name="inputFile">The file to encrypt.</param>
    /// <param name="outputFile">The encrypted output file.</param>
    [Obsolete("Pass an explicit password to EncryptFile to avoid relying on an environment variable.")]
    public static void EncryptFile(string inputFile, string outputFile)
    {
        EncryptFile(inputFile, outputFile, GetEnvironmentPassword());
    }

    /// <summary>Decrypts a file using the configured legacy environment password.</summary>
    /// <param name="inputFile">The encrypted file.</param>
    /// <param name="outputFile">The decrypted output file.</param>
    [Obsolete("Pass an explicit password to DecryptFile to avoid relying on an environment variable.")]
    public static void DecryptFile(string inputFile, string outputFile)
    {
        DecryptFile(inputFile, outputFile, GetEnvironmentPassword());
    }

    private static void ValidateEncryptionInputs(string inputFile, string outputFile, string password)
    {
        if (string.IsNullOrEmpty(inputFile))
            throw new ArgumentException("An input file is required.", nameof(inputFile));
        if (string.IsNullOrEmpty(outputFile))
            throw new ArgumentException("An output file is required.", nameof(outputFile));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("A password is required.", nameof(password));
        if (!File.Exists(inputFile))
            throw new FileNotFoundException("The input file was not found.", inputFile);
    }

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
    }

    private static string GetEnvironmentPassword()
    {
        string? password = Environment.GetEnvironmentVariable("CODINGWITHDAVID_COMMON_ENCRYPTION_PASSWORD");
        return !string.IsNullOrEmpty(password)
            ? password
            : throw new InvalidOperationException(
                "Set CODINGWITHDAVID_COMMON_ENCRYPTION_PASSWORD or pass an explicit password.");
    }
}
