using System;
using UnityEditor;
#if UNITY_2020_1_OR_NEWER
using UnityEditor.Compilation;
#endif

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

            try
            {
#if UNITY_2020_1_OR_NEWER
                string _namespace = CompilationPipeline.GetAssemblyRootNamespaceFromScriptPath(path);

                if(string.IsNullOrEmpty(_namespace))
                {
                    string _namespace = GenerateNamespaceFromFoldeStructure(path);
                }
#else
                string _namespace = GenerateNamespaceFromFoldeStructure(path);
#endif

                fileContent = fileContent.Replace("#NAMESPACE#", _namespace);
            }
            catch(ArgumentOutOfRangeException e)
            {
#if UNITY_2020_1_OR_NEWER
                UnityEngine.Debug.LogError("No Assembly definition reference found or set and No Folder Structure found to extract namespace.");
#else
                UnityEngine.Debug.LogError("No Folder Structure found to extract namespace");
                UnityEngine.Debug.LogError($"{e.Message}\n\n{e.StackTrace}");
#endif
                fileContent = fileContent.Replace("#NAMESPACE#", "Assets");
            }
            finally
            {
                System.IO.File.WriteAllText(path, fileContent);
                AssetDatabase.Refresh();
            }
        }

        private static string GenerateNamespaceFromFoldeStructure(string path)
        {
            string lastPart = path.Substring(path.IndexOf("Assets") + @"Assets/".Length);
            string ns = lastPart.Substring(0, lastPart.LastIndexOf(System.IO.Path.DirectorySeparatorChar));
            return ns.Replace(System.IO.Path.DirectorySeparatorChar, '.').Replace(" ", "");
        }
    }
}
