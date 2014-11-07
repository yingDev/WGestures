using System;
using System.IO;
using IO;

namespace NativeMultiFileArchiveLib
{
    /// <summary>
    /// represents a file stored within an archive.
    /// </summary>
    [Serializable]
    public class ArchiveFile
    {
        /// <summary>
        /// empty constructor, supports deserialization
        /// </summary>
        public ArchiveFile()
        {
        }

        /// <summary>
        /// construct and load in the file specified.
        /// </summary>
        /// <param name="originalPath"></param>
        public ArchiveFile(string originalPath)
        {
            // get info of the file:
            FileInfo info = new FileInfo(originalPath);

            // construct the archive file from the original path.
            this.Path = System.IO.Path.GetDirectoryName(originalPath);
            this.Name = System.IO.Path.GetFileName(originalPath);
            this.OriginalFileName = originalPath;
            this.Created = info.CreationTime;
            this.Modified = info.LastWriteTime;
            this.Archived = DateTime.Now;
            this.OriginalFileLength = (int)info.Length;
            this.FileData = File.ReadAllBytes(originalPath);
        }

        #region Properties

        /// <summary>
        /// the path (folder) within the archive.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// the name of the file (without the path)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the original name of the file.
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// original file created date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// original file modified date.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// file archived date.
        /// </summary>
        public DateTime Archived { get; set; }

        /// <summary>
        /// the original length of the file.
        /// </summary>
        public int OriginalFileLength { get; set; }

        /// <summary>
        /// the actual uncompressed file data.
        /// </summary>
        public byte[] FileData { get; set; }

        #endregion

        /// <summary>
        /// return a stream from the stored file data.
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            return new MemoryStream(FileData);
        }

        /// <summary>
        /// save the file data to the specified location.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(String fileName)
        {
            File.WriteAllBytes(fileName, FileData);
        }

        /// <summary>
        /// returns this archive file class as a compressed array of bytes.
        /// if the data is already compressed, this can cause the file to get bigger.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return TinySerializer.SerializeCompressed(this, true);
        }

        /// <summary>
        /// creates this archive file information from a compressed array of bytes.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ArchiveFile FromByteArray(byte[] data)
        {
            return TinySerializer.DeSerializeCompressed<ArchiveFile>(data, true);
        }

        #region Overrides

        /// <summary>
        /// get the string representation of the archive file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Path + "\\" + Name;
        }

        /// <summary>
        /// determinine if the string refers to the same file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Equals(String fileName)
        {
            return fileName.Equals(ToString());
        }

        /// <summary>
        /// determine if the two archive files are equivalent.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool Equals(ArchiveFile file)
        {
            return (file.Name == this.Name && file.Path == this.Path);
        }

        /// <summary>
        /// override the equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is String)
                return Equals((String)obj);
            if (obj is ArchiveFile)
                return Equals((ArchiveFile)obj);

            return base.Equals(obj);
        }

        /// <summary>
        /// override the get-hash-code method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion
    }

}
