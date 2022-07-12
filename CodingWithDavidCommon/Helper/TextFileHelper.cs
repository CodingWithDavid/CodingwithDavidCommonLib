using System.Text;

public static class TextFileHelper
{
    /// <summary>
    /// This is method, a file can be read in a text file
    /// </summary>
    /// <param name="file">Name of the file to load</param>
    /// <returns>The file as a string</returns>
    /// <exception>Will re-throw any exception it encounters</exception>
    static public string LoadTextFile(string file)
    {
        string result = string.Empty;
        //make sure the file exists
        if (File.Exists(file))
        {
            result = File.ReadAllText(file);
        }
        return result;
    }

    /// <summary>
    /// This is method a file can be read in to a list of strings
    /// </summary>
    /// <param name="file">Name of the file to load</param>
    /// <returns>The file as a string</returns>
    /// <exception>Will re-throw any exception it encounters</exception>
    static public List<string> TextFileToList(string file)
    {
        List<string> result = new();
        //make sure the file exists
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file);
            string[] sdata = data.Replace('\n', ' ').Trim().Split('\r');
            result.AddRange(sdata.Where(str => !string.IsNullOrEmpty(str)));
        }
        return result;
    }

    /// <summary>
    /// This is method a file can be read in to a list of strings
    /// </summary>
    /// <param name="file">Name of the file to load</param>
    /// <param name="encode">Type of encoding to use</param>
    /// <returns>The file as a string</returns>
    /// <exception>Will re-throw any exception it encounters</exception>
    static public List<string> TextFileToList(string file, Encoding encode)
    {
        List<string> result = new();
        //make sure the file exists
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file, encode);
            string[] sdata = data.Replace('\n', ' ').Trim().Split('\r');
            result.AddRange(sdata.Where(str => !string.IsNullOrEmpty(str)));
        }
        return result;
    }
}
