using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using PixelCrushers.DialogueSystem;
using Random = UnityEngine.Random;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will run Lua code.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will run Lua code.")]
    [FeedbackPath("Dialogue System/Run Lua Code")]
    public class MMF_RunLuaCode: MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.EventsColor; } }
        public override bool EvaluateRequiresSetup() { return string.IsNullOrEmpty(LuaCode); }
        public override string RequiredTargetText { get { return !string.IsNullOrEmpty(LuaCode) ? LuaCode : ""; } }
        public override string RequiresSetupText { get { return "This feedback requires that Lua Code be set. You can set it below."; } }
#endif

        [MMFInspectorGroup("Lua Code Settings", true, 76, true)]
        [LuaScriptWizard]
        public string LuaCode;

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (!string.IsNullOrEmpty(LuaCode))
            {
                Lua.Run(LuaCode);
            }
        }

    }
}
