using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This editor script runs when you load the Unity editor.
    /// It adds the Scripting Define Symbol TMP_PRESENT if needed
    /// to enable the Dialogue System's TextMesh Pro support since
    /// the demo uses TextMesh Pro.
    /// </summary>
    [InitializeOnLoad]
    public class BundleDemoEnableTMProSupport
    {
        private const string TMP_PRESENT = "TMP_PRESENT";

        static BundleDemoEnableTMProSupport()
        { 
            if (!MoreEditorUtility.DoesScriptingDefineSymbolExist(TMP_PRESENT))
            {
                Debug.Log("Dialogue System: Adding Scripting Define Symbol 'TMP_PRESENT' to enable TextMesh Pro support.");
                MoreEditorUtility.TryAddScriptingDefineSymbols(TMP_PRESENT);
            }
        }
    }

}
