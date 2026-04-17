// using System;
// using UnityEngine;

// public static class ProgressionEvents
// {
//     public static Action<string> OnSignalRaised;
//     public static Action<string> OnMilestoneCompleted;

//     public static void RaiseSignal(string signalId)
//     {
//         if (string.IsNullOrEmpty(signalId)) return;

//         Debug.Log("Progression signal: " + signalId);
//         OnSignalRaised?.Invoke(signalId);
//     }

//     public static void CompleteMilestone(string milestoneId)
//     {
//         if (string.IsNullOrEmpty(milestoneId)) return;

//         Debug.Log("Progression milestone complete: " + milestoneId);
//         OnMilestoneCompleted?.Invoke(milestoneId);
//     }
// }
