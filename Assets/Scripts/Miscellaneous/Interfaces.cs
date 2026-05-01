public interface IInteractable
{
    public bool IsInteractable { get; }
    public void Interact(PlayerInteractor player);
}

public interface IEventListener
{
    public abstract void OnEventStart();
}