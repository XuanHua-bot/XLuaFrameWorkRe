using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class FileUtil
{
    /// <summary>
    /// 检测文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        FileInfo file = new FileInfo(path);
        return file.Exists;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteFile(string path,byte[]data)
    {
        //获取标准路径  把 斜杠转换
        path = PathUtil.GetStardardPath(path);

        //文件夹的路径
        string dir = path.Substring(0, path.LastIndexOf("/"));

        //判断文件夹是否存在 无则创建
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileInfo file = new FileInfo(path);
        //判断文件是否存在
        if (file.Exists)//因为无法直接覆盖文件 所以需要进行删除
        {
            file.Delete();
        }
        try
        {
            using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        catch(IOException e)
        {
            Debug.LogError(e.Message);
        }
       

    }
}

