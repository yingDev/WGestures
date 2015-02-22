using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NativeMultiFileArchiveLib;
using WGestures.Common.Config;
using WGestures.Common.Config.Impl;
using WGestures.Core.Persistence;
using WGestures.Core.Persistence.Impl;

namespace WGestures.App.Migrate
{
    internal static class MigrateService
    {
        public static ConfigAndGestures ImportPrevousVersion()
        {
            var possibleDirs = Directory.GetDirectories(Directory.GetParent(AppSettings.UserDataDirectory).FullName);
            possibleDirs = possibleDirs.Where(s =>
            {
                var dirName = Path.GetFileName(s);
                int num;
                return dirName.Split('.').Length == 4 && int.TryParse(dirName.Replace(".", string.Empty), out num);
            }).ToArray();

            if (possibleDirs.Length < 2) return null;

            Func<string, int[]> splitToInts = s => Path.GetFileName(s).Split('.').Select(i => int.Parse(i)).ToArray();

            Array.Sort(possibleDirs, (a, b) =>
            {
                var aNums = splitToInts(a);
                var bNums = splitToInts(b);

                var compareResult = 0;
                for (var i = 0; i < aNums.Length; i++)
                {
                    if (aNums[i] == bNums[i]) continue;

                    compareResult = aNums[i].CompareTo(bNums[i]);
                    break;
                }

                return compareResult;
            });

            //获得最近的那个版本的数据目录
            var recent = possibleDirs[possibleDirs.Length - 2];

            var gestures = null as JsonGestureIntentStore;
            var config = null as PlistConfig;

            var gesturesFileV1 = recent + @"\gestures.json";
            var gestureFileV2 = recent + @"\gestures.wg";
            var configFile = recent + @"\config.plist";

            try
            {
                if (File.Exists(gesturesFileV1))
                {
                    gestures = new JsonGestureIntentStore(gesturesFileV1, "1");
                }else if (File.Exists(gestureFileV2))
                {
                    gestures = new JsonGestureIntentStore(gestureFileV2, "2");
                }

                if (File.Exists(configFile))
                {
                    config = new PlistConfig(configFile);
                }
            }
            catch (Exception e)
            {

                throw new MigrateException(e.Message, e);
            }


            return new ConfigAndGestures(config, gestures);

        }

        /// <summary>
        /// 根据文件类型自动导入wgb或json
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static ConfigAndGestures Import(string from)
        {
            if (from.EndsWith(".wgb") || from.EndsWith(".WGB"))
            {
                return ImportWgb(from);
            }
            else if (from.EndsWith(".json") || from.EndsWith(".JSON"))
            {
                return ImportJsonGestures(from, "1");
            }else if (from.EndsWith(".wg") || from.EndsWith(".WG"))
            {
                return ImportJsonGestures(from, "2");
            }

            throw new MigrateException("未识别的文件类型");
        }

        public static ConfigAndGestures ImportJsonGestures(string jsonPath, string version)
        {
            if (!File.Exists(jsonPath)) throw new MigrateException("文件不存在:" + jsonPath);

            var intentStore = new JsonGestureIntentStore(jsonPath, version);
            return new ConfigAndGestures(null, intentStore);
        }

        public static ConfigAndGestures Combine(ConfigAndGestures old, ConfigAndGestures current, MergeOption mergeOption)
        {
            var config = null as PlistConfig;
            var gestures = null as JsonGestureIntentStore;

            if (MergeOption.Config == (mergeOption & MergeOption.Config))
            {
                config = new PlistConfig();
                //旧的应该覆盖新的
                config.Import(current.Config, old.Config);
            }

            if (MergeOption.Gestures == (mergeOption & MergeOption.Gestures))
            {
                gestures = current.GestureIntentStore.Clone();

                gestures.Import(old.GestureIntentStore);
            }

            return new ConfigAndGestures(config, gestures);
        }

        public static ConfigAndGestures ImportWgb(string wgbFilePath)
        {
            if (!File.Exists(wgbFilePath))
            {
                throw new MigrateException("文件不存在:" + wgbFilePath);
            }

            var config = null as PlistConfig;
            var gestures = null as JsonGestureIntentStore;

            var cofnigFileName = Path.GetFileName(AppSettings.ConfigFilePath);
            var gesturesFileName = Path.GetFileName(AppSettings.GesturesFilePath);

            var arcFile = new StreamingArchiveFile(wgbFilePath);
            var files = arcFile.FileIndex.IndexedFileNames.ToArray();

            var fileShortNames = (from f in files select Path.GetFileName(f)).ToArray();
            if (!fileShortNames.Contains(cofnigFileName) || !fileShortNames.Contains(gesturesFileName))
                throw new MigrateException("文件内容不正确（未找到需要的部分）: " + wgbFilePath);

            try
            {
                // iterate the files in the archive:
                foreach (var fileName in files)
                {
                    // write the name of the file
                    Debug.Print("File: " + fileName);

                    // extract the file:
                    var file = arcFile.GetFile(fileName);
                    //file.SaveAs("text.txt");

                    //config file
                    if (Path.GetFileName(fileName) == cofnigFileName)
                    {
                        config = new PlistConfig(file.GetStream(), closeStream: true);

                    }
                    else
                    {
                        var version = "1";
                        if (fileName.EndsWith(".json"))
                        {
                            version = "1";
                        }
                        else if (fileName.EndsWith(".wg"))
                        {
                            version = "2";
                        }
                        gestures = new JsonGestureIntentStore(file.GetStream(), true, version);
                    }

                }


                return new ConfigAndGestures(config, gestures);
            }
            catch (Exception e)
            {
                if (e is SystemException) throw;
                throw new MigrateException("导入错误", e);
            }

        }

        public static void ExportTo(string filePath)
        {
            //如果已经存在，则先删掉。否则Archive会添加文件进去
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var arcFile = new StreamingArchiveFile(filePath);
            arcFile.AddFile(new ArchiveFile(AppSettings.ConfigFilePath));
            arcFile.AddFile(new ArchiveFile(AppSettings.GesturesFilePath));

        }

        [Flags]
        internal enum MergeOption
        {
            Config = 1, Gestures = 2, Both = Config | Gestures
        }
    }
}
