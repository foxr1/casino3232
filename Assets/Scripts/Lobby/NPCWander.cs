using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    /* Adapted code from https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/
    to help with wander AI.*/

    public float wanderRadius;
    private float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    public Animator anim;

    public bool chasingPlayer = false, onDanceFloor = false;
    private GameObject player;

    private AudioSource speech;
    private float speechTimer = 0;
    private float rndSpeechTimer;

    private AudioSource lobbyMusic;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();

        speech = GetComponent<AudioSource>();
        rndSpeechTimer = Random.Range(0, 5);

        wanderTimer = Random.Range(2, 8); // NPC will walk around every 2-8 seconds
        timer = wanderTimer;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is not attached to " + gameObject.name);
        }

        lobbyMusic = GameObject.Find("Dome").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Wander around the lobby unless the NPC has caught the player pickpocketting
        if (!chasingPlayer && !onDanceFloor)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                wanderTimer = Random.Range(2, 8);
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        } 
        else if (chasingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }

        // If music is loud enough, set random position that is in area of the dance floor and just dance without moving
        if (lobbyMusic.volume > 0.9 && !onDanceFloor)
        {
            float rndXValue = Random.Range(-10f, 10f);
            float rndZValue = Random.Range(-10f, 10f);

            agent.SetDestination(new Vector3(rndXValue, 36, rndZValue));
            onDanceFloor = true;
        }
        else if (lobbyMusic.volume <= 0.9)
        {
            onDanceFloor = false;
        }

        if (agent.remainingDistance < 4f && onDanceFloor) // If NPC is roughly near set position and is meant to be on dance floor then dance
        {
            anim.SetInteger("Speed", 4);
        }
        else if (agent.remainingDistance > 0.1f) // If the NPC is walking, play walking animation
        {
            anim.SetInteger("Speed", 1);
        }
        else if (name.Equals("NPC(Clone)")) // Only apply dancing to NPCs, not Wardens
        {
            // Change intensity of dancing depending on how loud the music is
            if (lobbyMusic.volume > 0.75)
            {
                anim.SetInteger("Speed", 4);
            }
            else if (lobbyMusic.volume > 0.5)
            {
                anim.SetInteger("Speed", 3);
            }
            else if (lobbyMusic.volume > 0.3)
            {
                anim.SetInteger("Speed", 2);
            }
            else
            {
                anim.SetInteger("Speed", 0);
            }
        }
        else
        {
            anim.SetInteger("Speed", 0);
        }

        // Randomly play "speech" audio
        if (speechTimer > rndSpeechTimer)
        {
            rndSpeechTimer = Random.Range(0, 5);
            speech.pitch = Random.Range(-2, 2);
            speechTimer = 0;
            speech.Stop();
            speech.Play();
        }
        speechTimer += Time.deltaTime;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
