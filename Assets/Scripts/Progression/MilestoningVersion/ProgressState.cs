// using System.Collections.Generic;

// public class ProgressState
// {
//     private readonly HashSet<string> signals = new HashSet<string>();
//     private readonly HashSet<string> completedMilestones = new HashSet<string>();
//     private readonly Dictionary<string, int> signalCounts = new Dictionary<string, int>();

//     public bool HasSignal(string id)
//     {
//         return signals.Contains(id);
//     }

//     public bool HasMilestone(string id)
//     {
//         return completedMilestones.Contains(id);
//     }

//     public int GetSignalCount(string id)
//     {
//         return signalCounts.TryGetValue(id, out int value) ? value : 0;
//     }

//     public void AddSignal(string id)
//     {
//         if (string.IsNullOrEmpty(id)) return;

//         signals.Add(id);

//         if (!signalCounts.ContainsKey(id)) signalCounts[id] = 0;
//         signalCounts[id]++;
//     }

//     public bool CompleteMilestone(string id)
//     {
//         if (string.IsNullOrEmpty(id)) return false;
//         return completedMilestones.Add(id);
//     }
// }
