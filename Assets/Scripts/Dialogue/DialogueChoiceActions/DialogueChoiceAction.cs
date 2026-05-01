using UnityEngine;

public abstract class DialogueChoiceAction : MonoBehaviour
{
    [SerializeField] DialogueChoiceId[] choices;

    void Start()
    {
        foreach (var choice in choices) DialogueChoiceManager.Subscribe(choice, Execute);
    }

    protected abstract void Execute();
}