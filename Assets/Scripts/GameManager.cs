using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerSheep, aiSheep, wolf;
    [SerializeField] private float wolfLifetime = 1;
    [SerializeField] private int aiCount;
    [SerializeField] private float spawnRadius, spawnY;

    [Header("Platform settings")]
    [SerializeField] private GameObject platform;
    [SerializeField] private Vector3 platformSpawnBounds;
    [SerializeField] private Vector2 platformSpawnTimeRange;
    [SerializeField] private int platformsCount = 3;
    [SerializeField] private float platformsFallof = .5f;
    [SerializeField] private float wolvesWaitAfterPlatform = 2.5f;

    private List<GameObject> sheeps = new List<GameObject>();

    public List<GameObject> GetSheeps() { return sheeps; }
    public void RemoveSheep(GameObject go)
    {
        PlayerController pc = go.GetComponent<PlayerController>();
        if (pc) Messenger.Broadcast("Loose");

        sheeps.Remove(go);
        Destroy(go);

        if (sheeps.Count == 1 && sheeps[0].GetComponent<PlayerController>())
            Messenger.Broadcast("Win");
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(platformSpawnTimeRange.x, platformSpawnTimeRange.y));

            for (int i = 0; i < platformsCount * Mathf.Exp(-platformsFallof * Time.time * .01f); ++i)
            {
                Vector3 pos = Random.insideUnitSphere * platformSpawnBounds.z + new Vector3(platformSpawnBounds.x, 0, platformSpawnBounds.y);
                pos.y = 0;
                Instantiate(platform, pos, Quaternion.identity);
            }

            yield return new WaitForSeconds(wolvesWaitAfterPlatform);
            for (int i = 0; i < Mathf.Sqrt(sheeps.Count); ++i)
            {
                Vector3 pos = Random.insideUnitSphere * spawnRadius;
                pos.y = spawnY;

                WolfAIController wolfAI = Instantiate(wolf, pos, Quaternion.identity).GetComponent<WolfAIController>();
                wolfAI.GameManager = this;
                wolfAI.Lifetime = wolfLifetime;
            }
        }
    }

    private void Awake()
    {
        Vector3 pos = Random.insideUnitSphere * spawnRadius;
        pos.y = spawnY;
        sheeps.Add(Instantiate(playerSheep, pos, Quaternion.identity));        

        for (int i = 0; i < aiCount; ++i)
        {
            pos = Random.insideUnitSphere * spawnRadius;
            pos.y = spawnY;
            sheeps.Add(Instantiate(aiSheep, pos, Quaternion.identity));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(platformSpawnBounds.x, 0, platformSpawnBounds.y), platformSpawnBounds.z);
    }
}
