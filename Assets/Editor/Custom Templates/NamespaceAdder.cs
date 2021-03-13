using System;
using UnityEditor;

namespace CustomTemplates
{
    public class NamespaceAdder : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = TemplateUtils.CleanPathToFile(path);

            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)) return;

            string fileContent = System.IO.File.ReadAllText(path);

            if (!fileContent.Contains("#NAMESPACE#")) return;

            string lastPart = path.Substring(path.IndexOf("Assets") + @"Assets/".Length);

            try
            {
                string _namespace = lastPart.Substring(0, lastPart.LastIndexOf(System.IO.Path.DirectorySeparatorChar));
                _namespace = _namespace.Replace(System.IO.Path.DirectorySeparatorChar, '.');
                _namespace = _namespace.Replace(" ", "");

                fileContent = fileContent.Replace("#NAMESPACE#", _namespace);
            }
            catch(ArgumentOutOfRangeException _)
            {
                UnityEngine.Debug.LogError("No Folder Structure found to extract namespace");
                //UnityEngine.Debug.LogError(e.Message);
                fileContent = fileContent.Replace("#NAMESPACE#", "Assets");
            }
            finally
            {
                System.IO.File.WriteAllText(path, fileContent);
                AssetDatabase.Refresh();
            }
        }
    }
}
