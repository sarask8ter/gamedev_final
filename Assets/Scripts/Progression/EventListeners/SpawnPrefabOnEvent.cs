using UnityEngine;

public class SpawnPrefab : EventAction
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    public override void OnEventStart()
    {
        Instantiate(prefab, parent);
    }
}