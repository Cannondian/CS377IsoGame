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
    /// This feedback will play a bark.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will play a bark.")]
    [FeedbackPath("Dialogue System/Bark")]
    public class MMF_Bark : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.EventsColor; } }
        public override bool EvaluateRequiresSetup() { return string.IsNullOrEmpty(BarkConversation) && string.IsNullOrEmpty(BarkText); }
        public override string RequiredTargetText { get { return !string.IsNullOrEmpty(BarkConversation) ? BarkConversation : (!string.IsNullOrEmpty(BarkText) ? BarkText : ""); } }
        public override string RequiresSetupText { get { return "This feedback requires that a Bark Conversation be selected or Bark Text be set. You can set either one below."; } }
#endif

        [MMFInspectorGroup("Bark Settings", true, 76, true)]
        [ConversationPopup(true)]
        public string BarkConversation;
        [Tooltip("If Bark Conversation is not set, bark this text instead")]
        public string BarkText;
        [Tooltip("If Bark Conversation is not set, play this sequence with Bark Text")]
        public string BarkTextSequence;
        [Tooltip("The transform on which to play the bark")]
        public Transform BoundBarker;

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (string.IsNullOrEmpty(BarkConversation))
            {
                DialogueManager.BarkString(BarkText, BoundBarker, null, BarkTextSequence);
            }
            else
            {
                DialogueManager.Bark(BarkConversation, BoundBarker);
            }
        }

    }
}
