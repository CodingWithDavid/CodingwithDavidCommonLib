using System.Text;

namespace CodingWithDavid.Common;

/// <summary>Provides lightweight CSV row, reader, and writer types.</summary>
public static class ReadWriteCsvUtility
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        /// <summary>Gets or sets the original or generated CSV line text.</summary>
        public string LineText { get; set; } = String.Empty;
    }

    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CsvFileWriter : StreamWriter
    {
        /// <summary>Initializes a writer over a stream.</summary>
        /// <param name="stream">The destination stream.</param>
        public CsvFileWriter(Stream stream) : base(stream)
        {
        }

        /// <summary>Initializes a writer over a stream.</summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="leaveOpen">Whether to leave the stream open when disposed.</param>
        public CsvFileWriter(Stream stream, bool leaveOpen) : base(stream, Encoding.UTF8, 1024, leaveOpen)
        {
        }
        /// <summary>Initializes a writer for a file.</summary>
        /// <param name="filename">The destination file name.</param>
        public CsvFileWriter(string filename) : base(filename)
        {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new();
            bool firstColumn = true;

            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                {
                    builder.Append(',');
                }

                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                {
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                }
                else
                {
                    builder.Append(value);
                }
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }

    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    public class CsvFileReader : StreamReader
    {
        /// <summary>Initializes a reader over a stream.</summary>
        /// <param name="stream">The source stream.</param>
        public CsvFileReader(Stream stream) : base(stream)
        {
        }

        /// <summary>Initializes a reader over a stream.</summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="leaveOpen">Whether to leave the stream open when disposed.</param>
        public CsvFileReader(Stream stream, bool leaveOpen) : base(stream, Encoding.UTF8, true, 1024, leaveOpen)
        {
        }

        /// <summary>Initializes a reader for a file.</summary>
        /// <param name="filename">The source file name.</param>
        public CsvFileReader(string filename) : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row">The row to populate.</param>
        /// <returns><see langword="true"/> when a row was read.</returns>
        public bool ReadRow(CsvRow row)
        {
            if (row != null)
            {
                row.LineText = ReadLine()!;
                if (String.IsNullOrEmpty(row.LineText))
                {
                    return false;
                }

                int pos = 0;
                int rows = 0;
                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;
                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText[start..pos];
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        {
                            pos++;
                        }
                        value = row.LineText[start..pos];
                    }

                    // Add field to list
                    if (rows < row.Count)
                    {
                        row[rows] = value;
                    }
                    else
                    {
                        row.Add(value);
                    }
                    rows++;

                    // Process up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    {
                        pos++;
                    }

                    if (pos < row.LineText.Length)
                    {
                        pos++;
                    }
                }
                // Delete any unused items
                while (row.Count > rows)
                {
                    row.RemoveAt(rows);
                }

                // Return true if any columns read
                return (row.Any());
            }
            return false;
        }
    }
}
