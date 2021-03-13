using System.IO;
using UnityEditor;

namespace CustomTemplates
{
    public class KeywordReplacer : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = TemplateUtils.CleanPathToFile(path);

            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))    return;

            string fileContent = System.IO.File.ReadAllText(path);

            fileContent = fileContent.Replace("#CREATIONDATE#", System.DateTime.Now + "");
            fileContent = fileContent.Replace("#PROJECTNAME#", PlayerSettings.productName);
            fileContent = fileContent.Replace("#COMPANYNAME#", PlayerSettings.companyName);

            string fileName = Path.GetFileNameWithoutExtension(path).Replace("CustomEditor", "");
            fileContent = fileContent.Replace("#TARGETCLASS#", fileName);

            System.IO.File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }
    }
}
