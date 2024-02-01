using MoreMountains.Feedbacks;
using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: MMF(play|stop|reset|revert|#, [subject])
    /// 
    /// Controls an MMF_Player on subject.
    /// </summary>
    public class SequencerCommandMMF : SequencerCommand
    {
        private void Awake()
        {
            var command = GetParameter(0);
            var subject = GetSubject(1, speaker);
            if (subject == null)
            {
                if (DialogueDebug.logWarnings)
                {
                    Debug.LogWarning("Dialogue System: MMF(" + GetParameters() + "): Can't find subject.");
                }
            }
            else
            {
                var mmfPlayer = subject.GetComponentInChildren<MMF_Player>();
                if (mmfPlayer == null)
                {
                    if (DialogueDebug.logWarnings)
                    {
                        Debug.LogWarning("Dialogue System: MMF(" + GetParameters() + "): No MMF_Player found on " + subject, subject);
                    }
                }
                else
                {
                    var isCommandValid = true;
                    int numRepeats;
                    if (int.TryParse(command, out numRepeats))
                    {
                        foreach (MMFeedback feedbacks in mmfPlayer.Feedbacks)
                        {
                            feedbacks.Timing.NumberOfRepeats = numRepeats;
                        }
                        mmfPlayer.Initialization();
                        mmfPlayer.PlayFeedbacks();
                    }
                    else if (string.Equals("play", command, System.StringComparison.OrdinalIgnoreCase))
                    {
                        mmfPlayer.PlayFeedbacks();
                    }
                    else if (string.Equals("stop", command, System.StringComparison.OrdinalIgnoreCase))
                    {
                        mmfPlayer.StopFeedbacks();
                    }
                    else if (string.Equals("reset", command, System.StringComparison.OrdinalIgnoreCase))
                    {
                        mmfPlayer.ResetFeedbacks();
                    }
                    else if (string.Equals("revert", command, System.StringComparison.OrdinalIgnoreCase))
                    {
                        mmfPlayer.Revert();
                    }
                    else
                    {
                        isCommandValid = false;
                        if (DialogueDebug.logWarnings)
                        {
                            Debug.LogWarning("Dialogue System: MMF(" + GetParameters() + "): Command must be play, stop, reset, or revert.");
                        }
                    }
                    if (isCommandValid && DialogueDebug.logInfo)
                    {
                        Debug.Log("Dialogue System: Sequencer: MMF(" + GetParameters() + ")", subject);
                    }
                }
            }
            Stop();
        }
    }
}
