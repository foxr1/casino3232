using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadNPCs : MonoBehaviour
{
    public int noOfNPCs, noOfWardens;
    public GameObject npcObj, wardenObj;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn each NPC at a random location within the lobby
        for (int i = 0; i < noOfNPCs; i++)
        {
            SpawnNPC(npcObj);
        }

        // Spawn each Warden at a random location within the lobby
        for (int i = 0; i < noOfWardens; i++)
        {
            SpawnNPC(wardenObj);
        }
    }

    private void SpawnNPC(GameObject type)
    {
        GameObject npc = Instantiate(type, new Vector3(0, 36, 0), Quaternion.identity);
        npc.GetComponent<NavMeshAgent>().enabled = false;
        npc.transform.SetParent(transform);
        Vector3 newPos = RandomNavSphere(new Vector3(0, 36, 0), 25, -1);
        npc.transform.position = newPos;
        npc.GetComponent<NavMeshAgent>().enabled = true;
    }

    // Adapted code from https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
