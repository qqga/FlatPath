using System;
using System.IO;
namespace FlatPath
{
    public class FlatPathManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileExistBehaviour">By default rename with index.</param>
        public FlatPathManager(FileExistBehaviourDelegate fileExistBehaviour = null)
        {
            _FileExistBehaviour = fileExistBehaviour ?? FileExistBehaviour_Rename;
        }

        public delegate void FileExistBehaviourDelegate(ref string fileName);
        FileExistBehaviourDelegate _FileExistBehaviour;


        public void FlatNested(string path, string prefix = null, bool useParentPathAsPrefix = false)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] nestedDirs  = dir.GetDirectories();
            for (int i = 0; i < nestedDirs.Length; i++)
            {
                var nestedDir = nestedDirs[i];
                Console.WriteLine($"--{i+1}/{nestedDirs.Length} -- '{nestedDir.Name}'");
                Flat(nestedDir.FullName, prefix, useParentPathAsPrefix);
            }
        }
        public void Flat(string path, string prefix = null, bool useParentPathAsPrefix = false)
        {
            DirectoryInfo rootdirectoryInfo = new DirectoryInfo(path);

            var directories = rootdirectoryInfo.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                var directory = directories[i];
                Console.WriteLine($"Dir {i+1}/{directories.Length}: {directory.FullName}");
                string combinedPrefix = (useParentPathAsPrefix ? directory.Name + "_" : String.Empty) + (prefix ?? string.Empty);
                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    FileInfo fileInfo = files[j];
                    string fileName = Path.Combine(path, combinedPrefix + fileInfo.Name);
                    if (File.Exists(fileName))
                        _FileExistBehaviour(ref fileName);
                    Console.WriteLine($"{i+1}/{directories.Length}.{j}/{files.Length} {fileInfo.Name} -> {fileName}");
                    fileInfo.MoveTo(fileName);
                }
                //directory.Delete();
            }
        }

        public static void FileExistBehaviour_Rename(ref string fileName)
        {
            string fileNamePostfix = fileName;
            int i = 0;
            while (File.Exists(fileNamePostfix))
                fileNamePostfix = fileName.Insert(fileName.LastIndexOf('.'), $"_{(++i)}");
            fileName = fileNamePostfix;
        }
    }


}