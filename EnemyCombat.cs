using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Examples;

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


    void Start()
    {
        afc = GetComponent<AlwaysFaceCamera>();
        an = GetComponent<Animation>();
        en = GetComponent<EnemyNav>();
        origin = transform.position;
        origin.y = origin.y + 1;
    }

    void Update()
    { 
        origin = transform.position;
        origin.y = origin.y + 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.Raycast(origin, transform.forward, out hit, maxdist))
        {
            if (en != null)
            {
                en.enabled = false;
            }
            origRot = transform.rotation;
            if (!firing)
            {
                Debug.Log(hit.collider.gameObject);
                StartCoroutine(Shoot());

            }
        }
        else {
            if (!isStationary)
            {
                en.enabled = true;
            }
        }
    }

        

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


    void OnParticleCollision(GameObject other)
    {
        an.Stop();
        ChangeColors();
        if (afc != null)
        {
            afc.enabled = false;
        }
        if (en != null)
        {
            en.enabled = false;
        }
        enabled = false;
        if (QuestLog.GetQuestState("Clear The Guard") == QuestState.Active)
        {
            DialogueLua.SetVariable("EnemyFrozen", true);
        }
    }

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
