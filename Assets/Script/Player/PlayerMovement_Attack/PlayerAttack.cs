using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class PlayerAttack : MonoBehaviour
{
    // Check whether player using mp or not.
    bool isUseMp = false; 
    bool isNotUseMp = false;

    // Check whether player attack or not.
    bool isAttack = false;

    [Header("Damage")]
    [SerializeField] public static float normalDamage = 10; // player normal attack damage.

    [Header("Effects")]
    public GameObject targetMarker;

    [Space]
    public GameObject[] prefabs;
    public GameObject[] prefabsCast;

    private ParticleSystem currEffect;
    private ParticleSystem effect;

    [Space]
    [Header("Layer")]
    public LayerMask obstacle;
    public LayerMask Monster;
    public LayerMask player;
    public LayerMask collidingLayer = ~0; // Target marker can only collide with scene layer

    [Space]
    [Header("UI")]
    public Image aim; // Aiming Cross
    public Vector2 uiOffset;

    [Space]
    [Header("Attack")]
    Transform target;
    private bool activeTarger = false;
    public Transform firePoint;
    private float fireCountdown = 0f;
    [Range(0,1)]public float fireRate = 1;
    public float fieldOfView = 60;
    public float viewDistance = 20f;

    [Space]
    [Header("Tagets")]
    public List<Transform> screenTargets = new List<Transform>();

    [Space]
    [Header("Sound effects")]
    private AudioSource soundComponent; //Play audio from Prefabs
    private AudioClip clip;
    private AudioSource soundComponentCast; //Play audio from PrefabsCast
    private AudioSource soundComponentUlt; //Play audio from PrefabsCast

    private void Start()
    {
        aim = Player.instance.ui.aim;
    }

    private void FixedUpdate()
    {
        if (isNotUseMp == false && isAttack == false)
        {
            isNotUseMp = true;
            StartCoroutine(Mp_Revert());
        } // Mp Recover

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Mouse Fix.

        if (fireCountdown > 0)
        {
            fireCountdown -= Time.deltaTime;
        }
        if (target == null) // End Attack when target is null.
        {
            Player.instance.animationController.AttackEnd();
            activeTarger = false;
        }

        // View Distance //////////////////////////
        Collider[] col = Physics.OverlapSphere(transform.position, viewDistance, Monster);
        screenTargets.Clear();
        target = null;

        for (int i = 0; i < col.Length; i++)
        {
            Vector3 TargetAngle = col[i].transform.position - transform.position;
            if (Vector3.Angle(transform.forward, TargetAngle) < fieldOfView)
                screenTargets.Add(col[i].transform);
        }
        //////////////////////////////////////////


        if (Input.GetMouseButton(1)) // Targeting
        {
            if (screenTargets.Count > TargetIndex())
                target = screenTargets[TargetIndex()];
        }
        UserInterface();

        if (Input.GetMouseButton(0) && Player.instance && Player.instance.mp > 0) // Attack
        {
            isAttack = true;
            if (isUseMp == false)
            {
                isUseMp = true;
                StartCoroutine(Mp_Use());
            }
            if (aim.enabled == true && activeTarger == true) // Target Attack
            {
                Player.instance.animationController.Attack();
                if (fireCountdown <= 0f)
                {
                    GameObject projectile = Instantiate(prefabsCast[8], firePoint.position, firePoint.rotation);
                    projectile.GetComponent<TargetProjectile>().UpdateTarget(target, (Vector3)uiOffset);
                    effect = prefabs[8].GetComponent<ParticleSystem>();
                    effect.Play();
                    //Get Audiosource from Prefabs if exist
                    if (prefabs[8].GetComponent<AudioSource>())
                    {
                        soundComponent = prefabs[8].GetComponent<AudioSource>();
                        clip = soundComponent.clip;
                        soundComponent.PlayOneShot(clip);
                    }
                    fireCountdown = fireRate;
                }
            }
            else // Non Target Attack
            {
                Player.instance.animationController.Attack();
                if (fireCountdown <= 0f)
                {
                    Transform targets;
                    RaycastHit hit;
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                    if (Physics.Raycast(ray, out hit, viewDistance,~player))
                    {
                        targets = new GameObject().transform;
                        targets.position = hit.point;
                        GameObject projectile = Instantiate(prefabsCast[8], firePoint.position, firePoint.rotation);
                        projectile.GetComponent<TargetProjectile>().UpdateTarget(targets, (Vector3)uiOffset);
                        effect = prefabs[8].GetComponent<ParticleSystem>();
                        effect.Play();
                        //Get Audiosource from Prefabs if exist
                        if (prefabs[8].GetComponent<AudioSource>())
                        {
                            soundComponent = prefabs[8].GetComponent<AudioSource>();
                            clip = soundComponent.clip;
                            soundComponent.PlayOneShot(clip);
                        }
                        fireCountdown = fireRate;
                        Destroy(targets.gameObject, 2f);
                        
                    }
                    else if (!Physics.Raycast(ray, out hit, viewDistance, ~player))
                    {
                        targets = new GameObject().transform;
                        targets.position = (ray.origin + 100 * ray.direction);
                        GameObject projectile = Instantiate(prefabsCast[8], firePoint.position, firePoint.rotation);
                        projectile.GetComponent<TargetProjectile>().UpdateTarget(targets, (Vector3)uiOffset);
                        effect = prefabs[8].GetComponent<ParticleSystem>();
                        effect.Play();
                        //Get Audiosource from Prefabs if exist
                        if (prefabs[8].GetComponent<AudioSource>())
                        {
                            soundComponent = prefabs[8].GetComponent<AudioSource>();
                            clip = soundComponent.clip;
                            soundComponent.PlayOneShot(clip);
                        }
                        fireCountdown = fireRate;
                        Destroy(targets.gameObject, 2f);

                    }
                }
            }
        }
        else
        {
            isAttack = false;

        }
    }

    public void CastSoundPlay()
    {
        soundComponentCast.Play(0);
    }

    private void UserInterface()
    {
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);
            Vector3 CornerDistance = screenPos - screenCenter;
            Vector3 absCornerDistance = new Vector3(Mathf.Abs(CornerDistance.x), Mathf.Abs(CornerDistance.y), Mathf.Abs(CornerDistance.z));

            //This way you can find target on the full screen
            //if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)

            // {screenPos.x > 0 && screenPos.y > 0 && screenPos.z > 0} - disable target if enemy backside

            //Find target near center of the screen
            //
            if (absCornerDistance.x < screenCenter.x  && absCornerDistance.y < screenCenter.y && screenPos.x > 0 && screenPos.y > 0 && screenPos.z > 0 //If target is in the middle of the screen
                && !Physics.Linecast(transform.position + (Vector3)uiOffset, target.position + (Vector3)uiOffset * 2, obstacle)) //If player can see the target
            {
                aim.transform.position = Vector3.MoveTowards(aim.transform.position, screenPos, Time.deltaTime * 3000);
                if (!activeTarger)
                    activeTarger = true;
            }
            else
            {
                //Another way
                //aim.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                aim.transform.position = Vector3.MoveTowards(aim.transform.position, screenCenter, Time.deltaTime * 3000);
                if (activeTarger)
                    activeTarger = false;
            }
        }
        if (target == null)
        {
            aim.transform.position = Vector3.MoveTowards(aim.transform.position, screenCenter, Time.deltaTime * 3000);
        }
    }

    public int TargetIndex()
    {
        float[] distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position), new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }
        return index;
    }

    IEnumerator Mp_Use()
    {
        yield return new WaitForSeconds(1f);
        if(Player.instance)
            Player.instance.mp -= 1;
        isUseMp = false;
        
    }

    IEnumerator Mp_Revert()
    {
        yield return new WaitForSeconds(1f);

        if (Player.instance)
        {
            Player.instance.mp += 2;
            if (Player.instance.mp > 20)
                Player.instance.mp = 20;
        }
        isNotUseMp = false;
    }


#if UNITY_EDITOR
    // You can see this in unity editor.
    private void OnDrawGizmosSelected()
    {
        // Draw Player View Area
        var angle = Quaternion.AngleAxis(-fieldOfView, transform.up);
        var angleAxis = angle * transform.forward;
        Handles.color = new Color(0, 0, 0, 0.1f);
        Handles.DrawSolidArc(transform.position, transform.up, angleAxis, fieldOfView * 2f, viewDistance);
    }
#endif

}
