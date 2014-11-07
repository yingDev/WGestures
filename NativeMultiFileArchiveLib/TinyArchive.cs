using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using General.Hierarchy;

namespace NativeMultiFileArchiveLib
{
    /// <summary>
    /// a class that can be serialized and compressed to disk, that contains a list of files and their contents.
    /// the entire archive file must be loaded into memory in order to work
    /// so this is only appropriate for a small number of relatively small files.
    /// </summary>
    [Serializable]
    public class TinyFileArchive
    {
        #region Constructor

        public TinyFileArchive()
        {
            // create the root folder:
            CreateFolder("Archive");
        }

        #endregion

        #region Fields:

        protected Dictionary<String, List<ArchiveFile>> _fileSystem = new Dictionary<String, List<ArchiveFile>>();

        #endregion

        #region Events

        /// <summary>
        /// raised when a file is added to the archive.
        /// </summary>
        public event EventHandler<FileEventArgs> FileAdded;

        /// <summary>
        /// raised when a file is updated in the archive.
        /// </summary>
        public event EventHandler<FileEventArgs> FileUpdated;

        /// <summary>
        /// raised when a file is removed from the archive.
        /// </summary>
        public event EventHandler<FileEventArgs> FileDeleted;

        /// <summary>
        /// raised when a folder is added to the archive.
        /// </summary>
        public event EventHandler<DirEventArgs> DirectoryAdded;

        /// <summary>
        /// raised when a folder is removed from the archive.
        /// </summary>
        public event EventHandler<DirEventArgs> DirectoryRemoved;

        /// <summary>
        /// raises the FileAdded event.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void OnFileAdded(ArchiveFile file)
        {
            if (FileAdded != null)
                FileAdded(this, new FileEventArgs() { File = file });
        }

        /// <summary>
        /// raises the FileUpdated event.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void OnFileUpdated(ArchiveFile file)
        {
            if (FileUpdated != null)
                FileUpdated(this, new FileEventArgs() { File = file });
        }

        /// <summary>
        /// raises the FileDeleted event.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void OnFileDeleted(ArchiveFile file)
        {
            if (FileDeleted != null)
                FileDeleted(this, new FileEventArgs() { File = file });
        }

        /// <summary>
        /// raises the DirectoryAdded event.
        /// </summary>
        /// <param name="dirName"></param>
        protected virtual void OnDirAdded(String dirName)
        {
            if (DirectoryAdded != null)
                DirectoryAdded(this, new DirEventArgs() { DirectoryName = dirName });
        }

        /// <summary>
        /// raises the DirectoryRemoved event.
        /// </summary>
        /// <param name="dirName"></param>
        protected virtual void OnDirRemoved(String dirName)
        {
            if (DirectoryRemoved != null)
                DirectoryRemoved(this, new DirEventArgs() { DirectoryName = dirName });
        }

        #endregion

        #region Load/Save

        public void Save()
        {
            if (!String.IsNullOrEmpty(FileName.Trim()))
                SaveAs(FileName);
            else
                throw new ApplicationException("File Name Not Specified");
        }

        /// <summary>
        /// save the archive file.
        /// </summary>
        /// <param name="archiveFileName"></param>
        public void SaveAs(String archiveFileName)
        {
            // update the filename
            FileName = archiveFileName;

            // save as:
            using (FileStream fs = File.Create(archiveFileName))
            {
                IO.TinySerializer.Serialize(fs, this, false);
            }
        }

        /// <summary>
        /// open an existing archive file.
        /// </summary>
        /// <param name="archiveFileName"></param>
        /// <returns></returns>
        public static TinyFileArchive Open(String archiveFileName)
        {
            using (FileStream fs = File.Open(archiveFileName, FileMode.Open))
            {
                return IO.TinySerializer.DeSerialize<TinyFileArchive>(fs, false);
            }
        }

        #endregion

        #region File Retrieval Methods

        /// <summary>
        /// gets a list of all file paths. (directoryname \ filename)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetFileNames()
        {
            foreach (string path in _fileSystem.Keys)
            {
                foreach (ArchiveFile file in _fileSystem[path])
                    yield return path + "\\" + file.Name;
            }
        }

        /// <summary>
        /// gets a list of all the paths (directories) in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetDirectories()
        {
            return (from path in _fileSystem.Keys select path);
        }

        /// <summary>
        /// gets a list of all the files in the system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ArchiveFile> GetFiles()
        {
            foreach (string path in _fileSystem.Keys)
            {
                foreach (ArchiveFile file in _fileSystem[path])
                    yield return file;
            }
        }

        /// <summary>
        /// gets a list of all the files in a specific directory.
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public List<ArchiveFile> GetDirectoryFiles(String dirName)
        {
            if (_fileSystem.ContainsKey(dirName))
                return _fileSystem[dirName];
            else
                return new List<ArchiveFile>();
        }

        /// <summary>
        /// locate a file by it's full name within the archive.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ArchiveFile GetFileByName(string fileName)
        {
            // break the "filename" into a path and name:
            string path = Path.GetDirectoryName(fileName);
            string name = Path.GetFileName(fileName);

            // locate the folder:
            if (_fileSystem.ContainsKey(path))
            {
                // search each item in the folder:
                foreach (var file in _fileSystem[path])
                    // yeild the file that matches:
                    if (file.Name.Equals(name))
                        return file;
            }

            // couldn't find the file:
            throw new ArgumentException("File Not Found:" + fileName);
        }

        /// <summary>
        /// return a list of files based on the original filename.
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <returns></returns>
        public IEnumerable<ArchiveFile> GetFilesByOriginalFileName(String originalFileName)
        {
            return (from file in GetFiles()
                    where file.OriginalFileName.Equals(originalFileName, StringComparison.OrdinalIgnoreCase)
                    select file);
        }


        #endregion

        #region File Search

        /// <summary>
        /// search for files matching the specified regular expression. match is done on directory name as well as filename
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public IEnumerable<ArchiveFile> Dir(String regex)
        {
            return (from file in GetFiles()
                    where Regex.IsMatch(file.Path + "\\" + file.Name, regex)
                    select file);
        }

        /// <summary>
        /// search for files matching the specified regular expression, within the specified folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public IEnumerable<ArchiveFile> Dir(String folder, String regex)
        {
            return (from file in GetDirectoryFiles(folder)
                    where Regex.IsMatch(file.Name, regex)
                    select file);
        }

        #endregion

        #region Properties

        /// <summary>
        /// gets or sets a file by it's file-name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ArchiveFile this[string fileName]
        {
            get
            {
                // find and return the file or throw a file-not-found exception.
                return GetFileByName(fileName);
            }
            set
            {
                // add the file
                AddFile(fileName, value);
            }
        }

        public string FileName { get; set; }

        public int DirectoryCount
        {
            get { return _fileSystem.Keys.Count; }
        }

        public int FileCount
        {
            get { return GetFiles().Count(); }
        }

        #endregion

        #region Add/Remove

        /// <summary>
        /// add the file using it's directory and name.
        /// </summary>
        /// <param name="value"></param>
        public void AddFile(ArchiveFile value)
        {
            AddFile(value.Path + "\\" + value.Name, value);
        }

        /// <summary>
        /// add the archive file into the specific directory with the specific name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public void AddFile(String fileName, ArchiveFile value)
        {
            // extract the path and name of the file.
            string path = Path.GetDirectoryName(fileName);
            string name = Path.GetFileName(fileName);

            // the name and path and owner are modified to match:
            value.Path = path;
            value.Name = name;

            // does the path already exist?
            if (!_fileSystem.ContainsKey(path))
            {
                // create the folder:
                _fileSystem.Add(path, new List<ArchiveFile>());

                // raise the event;
                OnDirAdded(path);
            }

            // does the folder already contain the file?
            if (_fileSystem[path].Contains(value))
            {
                // yes.. get the index of the file:
                int i = _fileSystem[path].IndexOf(value);

                // set the specific index to the input value:
                _fileSystem[path][i] = value;

                // now raise the file-updated event.
                OnFileUpdated(value);
            }
            else
            {
                // add the file.
                _fileSystem[path].Add(value);

                // raise the file added event:
                OnFileAdded(value);
            }
        }

        /// <summary>
        /// add a file from disk into the specified folder of the archive.
        /// </summary>
        /// <param name="sourceFileName">the source file path</param>
        /// <param name="destDir">the target directory in the archive.</param>
        /// <returns>the archive-file details.</returns>
        public ArchiveFile AddExistingFile(string sourceFileName, string destDir)
        {
            // create the archive file object:
            ArchiveFile file = new ArchiveFile(sourceFileName);

            // create the archive path:
            string path = destDir + "\\" + file.Name;

            // add into this
            this[path] = file;

            // return the file.
            return file;
        }

        /// <summary>
        /// adds all the files in the specified source folder that match the file-spec to the destination folder of the archive.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="searchString"></param>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        public int AddExistingFolder(string sourcePath, string searchString, string destinationFolder)
        {
            // get the matching filenames:
            String[] files = Directory.GetFiles(sourcePath, searchString);

            // add them in:
            foreach (string file in files)
                AddExistingFile(file, destinationFolder);

            // return the number of added files.
            return files.Length;
        }

        public void CreateFolder(String path)
        {
            if (!_fileSystem.ContainsKey(path))
            {
                _fileSystem.Add(path, new List<ArchiveFile>());
                OnDirAdded(path);
            }
        }

        /// <summary>
        /// remove an existing file by name. returns true if the file was found and removed.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool RemoveFile(String fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            string name = Path.GetFileName(fileName);

            if (_fileSystem.ContainsKey(path))
            {
                foreach (var file in _fileSystem[path])
                {
                    if (file.Name.Equals(name))
                    {
                        // remove the file from the list:
                        _fileSystem[path].Remove(file);

                        // raise the removed event.
                        OnFileDeleted(file);

                        // return with success:
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// remove all the files that match the search expression.
        /// </summary>
        /// <param name="searchExpr"></param>
        /// <returns></returns>
        public int RemoveFiles(String searchExpr)
        {
            int removed = 0;

            // enumerate the files that match the expression:
            foreach (ArchiveFile file in Dir(searchExpr))
            {
                // remove each file. increment the count:
                if (RemoveFile(file.Path + "\\" + file.Name))
                    removed++;
            }

            // return the number of files removed.
            return removed;
        }

        /// <summary>
        /// remove the specified folder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool RemoveFolder(String path)
        {
            if (_fileSystem.ContainsKey(path))
            {
                // raise the file deleted event for each file in the folder
                foreach (ArchiveFile file in _fileSystem[path])
                    OnFileDeleted(file);

                // remove the folder
                _fileSystem.Remove(path);

                // raise the directory removed event:
                OnDirRemoved(path);

                // return true indicating success.
                return true;
            }

            // return false... folder not found.
            return false;
        }

        #endregion

        #region Hierarchy

        /// <summary>
        /// returns an enumerable of folders under the specified path. if nested is true, all the descendent folders will be listed. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nested">if true, return all descendents of the specified path.</param>
        /// <returns></returns>
        public IEnumerable<String> GetSubFolders(string path, bool nested)
        {
            // create the folder tree:
            Tree<String> folderTree = CreateFolderTree('\\');

            // check that the path exists:
            if (folderTree.Contains(path))
            {
                // yield all descendents:
                if (nested)
                    foreach (var node in folderTree[path].Descendents)
                        yield return node.Value;
                else
                    // yeild children only.
                    foreach (var node in folderTree[path].Children)
                        yield return node.Value;
            }
        }

        /// <summary>
        /// gets a list of directories out of the archive - includes "calculated" directories.
        /// 
        /// ie, if the path: Temp\List\Files\Mine exists within the dictionary, (as only 1 entry)
        /// this will render the following list:
        /// Temp
        /// Temp\List
        /// Temp\List\Files
        /// Temp\List\Files\Mine
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetAllPaths()
        {
            foreach (var node in CreateFolderTree('\\').GetAllNodesInHierarchyOrder())
            {
                yield return node.Value;
            }
        }

        /// <summary>
        /// populate a tree-view with the contents of the folder.
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="pathSep"></param>
        /// <returns></returns>
        public TreeView CreateFolderTreeView(TreeView tv, char pathSep)
        {

            tv.PathSeparator = pathSep.ToString();
            tv.Nodes.Clear();

            // maintain a dictionary of paths and nodes.
            Dictionary<String, TreeNode> nodes = new Dictionary<string, TreeNode>();

            foreach (string dir in GetDirectories())
            {
                string[] pathElements = dir.Split(pathSep);
                string path = "";

                TreeNode parent = null;

                foreach (var pathElement in pathElements)
                {
                    if (path.Length > 0)
                        path += pathSep;
                    path += pathElement;

                    if (parent != null)
                    {
                        if (nodes.ContainsKey(path))
                            parent = nodes[path];
                        else
                        {
                            parent = parent.Nodes.Add(path, pathElement);
                            nodes.Add(path, parent);
                        }
                    }
                    else
                    {
                        if (nodes.ContainsKey(path))
                            parent = nodes[path];
                        else
                        {
                            parent = tv.Nodes.Add(path, pathElement);
                            nodes.Add(path, parent);
                        }
                    }
                }

            }

            return tv;
        }


        /// <summary>
        /// generates a hierarchy of folder names from the archive.
        /// </summary>
        /// <returns></returns>
        public Tree<String> CreateFolderTree(char pathSeperator)
        {
            Tree<String> tree = new Tree<string>();
            TreeNode<String> parent = null;

            // enumerate the actual directories:
            foreach (string dir in GetDirectories())
            {
                // break the dir into folder-names:
                string[] elements = dir.Split(pathSeperator);

                // build the path folder at a time.
                string path = "";

                // enumerate the path elements
                foreach (var pathElement in elements)
                {
                    // append to the path variable
                    if (path.Length > 0)
                        path += pathSeperator;
                    path += pathElement;


                    if (parent != null)
                    {
                        // add the current path to the hierarchy under the parent.
                        if (!tree.Contains(path))
                            parent = tree.Add(path, parent);
                        else
                            parent = tree[path];
                    }
                    else
                    {
                        // add the current path to the top of the tree.
                        if (!tree.Contains(path))
                            parent = tree.Add(path);
                        else
                            parent = tree[path];
                    }
                }
            }

            // return the populated tree.
            return tree;
        }

        #endregion

        /// <summary>
        /// tests the process.
        /// </summary>
        public static void Test()
        {
            TinyFileArchive reloaded = TinyFileArchive.Open(@"C:\Temp\test.arc");

            Tree<String> hierarchy = reloaded.CreateFolderTree('\\');
            foreach (var node in hierarchy.GetAllNodesInHierarchyOrder())
            {
                string path = node.Value;
                foreach (var file in reloaded.GetDirectoryFiles(path))
                    Debug.Print(path + "\\" + file.Name);
            }

        }
    }

    /// <summary>
    /// event arguments for an ArchiveFile Event.
    /// </summary>
    public class FileEventArgs : EventArgs
    {
        public ArchiveFile File { get; set; }
    }

    /// <summary>
    /// event arguments for a Directory event.
    /// </summary>
    public class DirEventArgs : EventArgs
    {
        public String DirectoryName { get; set; }
    }

}
