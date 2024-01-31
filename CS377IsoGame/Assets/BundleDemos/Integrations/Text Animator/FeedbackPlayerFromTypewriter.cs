using MoreMountains.Feedbacks;
using UnityEngine;
using Febucci.UI.Core;
using Febucci.UI.Core.Parsing;

namespace Demo
{
    /// <summary>
    /// Given a Typewriter, starts relative Feedbacks (from the Feel asset) once a specific word event is shown in the typewriter
    /// </summary>
    class FeedbackPlayerFromTypewriter : MonoBehaviour
    {
        enum FeedbackAction
        {
            Play = 0,
            Stop = 1,
        }

        [System.Serializable]
        struct FeedbackPair
        {
            public string eventName;
            public MMF_Player player;
            public FeedbackAction action;
        }

        [SerializeField] FeedbackPair[] feedbacks;
        [SerializeField] TypewriterCore[] typewriters;

        private void OnEnable()
        {
            if (typewriters.Length == 0)
            {
                Debug.LogError("No TextAnimatorPlayer has been referenced in FeedbackPlayer. Please assign it from the inspector.", gameObject);
                return;
            }

            foreach(var typewriter in typewriters)
                typewriter.onMessage.AddListener(OnEvent);
        }

        private void OnEvent(EventMarker eventMarker)
        {
            foreach(var feedback in feedbacks)
            {
                if (feedback.eventName.Equals(eventMarker.name))
                {
                    switch (feedback.action)
                    {
                        case FeedbackAction.Play: feedback.player?.PlayFeedbacks(); break;
                        case FeedbackAction.Stop: feedback.player?.StopFeedbacks(); break;
                    }
                }    
            }
        }

        private void OnDestroy()
        {
            foreach (var typewriter in typewriters)
                if (typewriter) typewriter.onMessage.RemoveListener(OnEvent);
        }

    }
}