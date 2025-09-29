using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum GameMode
{
    EditorMode,
    PackageBundle,
    UpdateMode,
}
    public class AppConst
    {
        public const string BundleExtension = ".ab";
        public const string FileListName = "filelist.txt";

        public static GameMode Gamemode = GameMode.EditorMode;

        //热更新资源地址
        public const string ResourceUrl = "Http://127.0.0.1/AssetBundles";
    }

