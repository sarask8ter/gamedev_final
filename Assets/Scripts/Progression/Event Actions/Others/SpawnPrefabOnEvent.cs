using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPrefab", menuName = "Event Actions/Others/Spawn Prefab")]
public class SpawnPrefab : EventAction
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool spawnOnPlayer;

    public override void OnEventStart()
    {
        Instantiate(prefab, spawnOnPlayer ? GameState.Player.transform : null);
    }
}