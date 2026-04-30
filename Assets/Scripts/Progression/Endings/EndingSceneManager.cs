using UnityEngine;
using UnityEngine.UI;

public class EndingSceneManager : MonoBehaviour
{
    [SerializeField] private Image newspaperImage;
    [SerializeField] private Sprite[] newspaperSprites; // Index 0: DeathByTea, 1: LeaveWithNoEvidence, 2: LeaveAndCallPolice, 3: SolveCase

    void Start()
    {
        Debug.Log("EndingSceneManager Start: Setting newspaper image based on chosen ending.");
        Ending chosen = EndingState.ChosenEnding;
        if (chosen != Ending.None && (int)chosen <= newspaperSprites.Length)
        {
            newspaperImage.sprite = newspaperSprites[(int)chosen - 1];
        }
        else
        {
            Debug.LogError("Invalid or None ending chosen.");
        }
    }
}
