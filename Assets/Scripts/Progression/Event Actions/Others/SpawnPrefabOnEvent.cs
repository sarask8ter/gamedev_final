using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPrefab", menuName = "Event Actions/Others/Spawn Prefab")]
public class SpawnPrefab : EventAction
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool spawnOnPlayer;

    public override void OnEventStart()
    {
        GameObject obj;

        if (spawnOnPlayer)
        {
            obj = Instantiate(prefab, GameState.Player.transform.position, Quaternion.identity);
        }
        else
        {
            obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}