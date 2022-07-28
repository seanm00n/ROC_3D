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
    private bool isUseMp;
    private bool isRecoverMp; 
    // Check whether player attack or not.
    private bool isAttack;

    [Header("Damage")]
    public static int normalDamage = 10; // player normal attack damage.

    [Header("Effects")]
    public GameObject targetMarker;

    [Space]
    public GameObject[] prefabs;
    
    // prefabs Index.
    // 8 : Normal attack invoke effect.
    
    public GameObject[] prefabsCast;

    // prefabsCast Index.
    // 8 : Normal attack invoke effect.

    private ParticleSystem currEffect;
    private ParticleSystem effect;

    [Space]
    [Header("Layer")]
    public LayerMask obstacle;
    public LayerMask monster; 
    public LayerMask player;
    public LayerMask collidingLayer = ~0; // Target marker can only collide with scene layer

    [Space]
    [Header("UI")]
    public Image aim; // Aiming Cross
    public Vector2 uiOffset;

    [Space]
    [Header("Attack")]
    Transform target;
    public Transform firePoint;
    
    private bool targetIsActive;
    private float fireCountdown;
    
    [Range(0,1)]public float fireRate = 1;
    public float fieldOfView = 60;
    public float viewDistance = 20f;

    [Space]
    [Header("Targets")]
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
        if (!isRecoverMp && !isAttack)
        {
            isRecoverMp = true;
            StartCoroutine(RevertMp());
        } // Mp Recover

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Mouse Fix.

        if (fireCountdown > 0)
        {
            fireCountdown -= Time.deltaTime;
        }
        if (!target) // End Attack when target is null.
        {
            Player.instance.animationController.AttackEnd();
            targetIsActive = false;
        }

        #region View Distance
        // VIEW DISTANCE ----------------------------------------
        var col = Physics.OverlapSphere(transform.position, viewDistance, monster);
        
        screenTargets.Clear();
        target = null;

        for (int i = 0; i < col.Length; i++)
        {
            Vector3 targetAngle = col[i].transform.position - transform.position;
            if (Vector3.Angle(transform.forward, targetAngle) < fieldOfView)
                screenTargets.Add(col[i].transform);
        }
        #endregion
        
        if (Input.GetMouseButton(1)) // Targeting
        {
            var targetIndex = TargetIndex();
            if (screenTargets.Count > targetIndex)
                target = screenTargets[targetIndex];
        }
        
        UserInterface();

        if (Input.GetMouseButton(0) && Player.instance && Player.instance.mp > 0) // Attack
        {
            isAttack = true;
            if (isUseMp == false)
            {
                isUseMp = true;
                StartCoroutine(UseMp());
            }
            
            Player.instance.animationController.Attack();

            // Wait for countdown
            if (fireCountdown <= 0f)
            {
                var shouldAimObject = aim.enabled && targetIsActive;
                var actualTarget = target;
                
                if (!shouldAimObject)
                {
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                    var rayIntersectsCollider = Physics.Raycast(ray, out var hit, viewDistance, ~player);
                    
                    // several amounts of target
                    actualTarget = new GameObject().transform;
                    actualTarget.position = rayIntersectsCollider ? hit.point : (ray.origin + 100 * ray.direction);
                }

                GameObject projectile = Instantiate(prefabsCast[8], firePoint.position, firePoint.rotation);
                projectile.GetComponent<TargetProjectile>().UpdateTarget(actualTarget, uiOffset);

                // Play that particle
                effect = prefabs[8].GetComponent<ParticleSystem>();
                effect.Play();

                // Get AudioSource from Prefabs if exist
                var audioSource = prefabs[8].GetComponent<AudioSource>();
                if (audioSource)
                {
                    soundComponent = audioSource;
                    clip = soundComponent.clip;
                    soundComponent.PlayOneShot(clip);
                }
                
                fireCountdown = fireRate;
                
                if (shouldAimObject)
                    Destroy(actualTarget.gameObject, 2f);
            }
        }
        else isAttack = false;
    }


    private void UserInterface() // Find target on screen.
    {
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        var targetPos = screenCenter;
        if (target)
        {
            // Other way you can find target on the full screen
            // if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)

            var screenPos = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);
            var cornerDist = screenPos - screenCenter;
            var absCornerDist = new Vector3(Mathf.Abs(cornerDist.x), Mathf.Abs(cornerDist.y), Mathf.Abs(cornerDist.z));

            // If you want find target near center of the screen => screenCenter.x / 3, screenCenter.y / 3
            if (absCornerDist.x < screenCenter.x && absCornerDist.y < screenCenter.y
                    
                    
                    && screenPos.x > 0 && screenPos.y > 0 && screenPos.z > 0 // disable target if enemy backside

                    // If player can see the target
                    && !Physics.Linecast(transform.position + (Vector3)uiOffset, target.position + (Vector3)uiOffset * 2, obstacle))
            {
                // change targetPos to screenPos here (very important)
                targetPos = screenPos;
                
                if (!targetIsActive)
                    targetIsActive = true;
            }
            else
            {
                // Another way
                // aim.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                
                if (targetIsActive)
                    targetIsActive = false;
            }
        }
        
        aim.transform.position = Vector3.MoveTowards(aim.transform.position, targetPos, Time.deltaTime * 3000);
    }

    private int TargetIndex() // Targeting according to distance.
    {
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(
                Camera.main.WorldToScreenPoint(screenTargets[i].position),
                screenCenter);
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

    private IEnumerator UseMp()
    {
        yield return new WaitForSeconds(1f);
        if(Player.instance)
            Player.instance.mp -= 1;
        
        isUseMp = false;
    }

    private IEnumerator RevertMp()
    {
        yield return new WaitForSeconds(1f);

        if (Player.instance)
        {
            Player.instance.mp += 2;
            if (Player.instance.mp > 20)
                Player.instance.mp = 20;
        }
        
        isRecoverMp = false;
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
