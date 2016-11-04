using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Examples;

/*Simple script for enemy combat. If the player is in the
line of sight, start shooting. Otherwise, do nothing.*/
public class EnemyCombat : MonoBehaviour {

    private RaycastHit hit;
    private EnemyNav en;
    private Vector3 origin;
    private AlwaysFaceCamera afc;
    private Quaternion origRot;
    private Animation an;
    private Usable use;
    private ConversationTrigger ct;
    private EnemyCombat ec;
    private bool firing = false;
    private bool fired = false;
    private GameObject bullets;
  
    public float maxdist = 5f;
    public AnimationClip shoot;
    public GameObject barrel;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float shotDelay = 3f;
    public bool isStationary;
    public Material FreezeColor;

    /*Get components for use in other methods*/
    void Start()
    {
        afc = GetComponent<AlwaysFaceCamera>();
        an = GetComponent<Animation>();
        en = GetComponent<EnemyNav>();
        origin = transform.position;
        origin.y = origin.y + 1;
    }

    /*Move origin. Actual movement of position is done in 
    another script*/
    void Update()
    { 
        origin = transform.position;
        origin.y = origin.y + 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   /*Is the player in sight?*/
        if (Physics.Raycast(origin, transform.forward, out hit, maxdist))
        {   /*Stop moving*/
            if (en != null)
            {
                en.enabled = false;
            }
            /*Save facing angle for when we return to movement*/
            origRot = transform.rotation;
            /*Are we already shooting? If not...*/
            if (!firing)
            {
                /*Start shooting*/
                StartCoroutine(Shoot());

            }
        }
        else {
            /*Some enemies are stationary, and thus don't need to return to 
            movement once the player leaves sight*/
            if (!isStationary)
            {
                en.enabled = true;
            }
        }
    }

        
    /*Shooting coroutine to start*/
    IEnumerator Shoot()
    {
       an.CrossFade(shoot.name);
       firing = true;
       yield return new WaitForSeconds(1f);
       Rigidbody rb;
       bullets = Instantiate(bulletPrefab, barrel.transform.position, barrel.transform.rotation) as GameObject;
       rb = bullets.GetComponent<Rigidbody>();
       rb.AddForce(bulletForce * transform.forward);
       firing = false;
    }

    /*When the player lands a shot...*/
    void OnParticleCollision(GameObject other)
    {   /*Stop movement and change color.*/
        an.Stop();
        ChangeColors();
        /*Some enemies have a face camera script to that they always
        face the player. If this is true, set it as inactive*/
        if (afc != null)
        {
            afc.enabled = false;
        }
        if (en != null)
        {
            en.enabled = false;
        }
        enabled = false;
        /*THis would be moved elsewhere had the game been larger, but for the scope of 
        the research, was left in. A quest required the freezing of a single enemy,
        and would trigger another event once this was accomplished*/
        if (QuestLog.GetQuestState("Clear The Guard") == QuestState.Active)
        {
            DialogueLua.SetVariable("EnemyFrozen", true);
        }
    }

    /*Change visuals of model*/
    void ChangeColors()
    {
        GameObject head;
        GameObject rest;
        head = transform.Find("ReconTroopHelmet").gameObject;
        rest = transform.Find("ReconTroop").gameObject;
        head.GetComponent<SkinnedMeshRenderer>().material = FreezeColor;
        rest.GetComponent<SkinnedMeshRenderer>().material = FreezeColor;
    }
}
