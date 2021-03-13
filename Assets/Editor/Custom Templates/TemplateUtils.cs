using System.IO;
using UnityEngine;

namespace CustomTemplates
{
    public static class TemplateUtils
    {
        public static string CleanPathToFile(string metaFilePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(metaFilePath);

            if (!fileName.EndsWith(".cs")) return string.Empty;

            return $"{Path.GetDirectoryName(metaFilePath)}{Path.DirectorySeparatorChar}{fileName}";
        }
    }
}

