using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class PlayerAttack : MonoBehaviour
{
    public enum skill
    {
        Angel_2, Energy_1, EnergyStrike_1, FrontExplosion_2, LightRay_1, 
        Magic_1, Nature_1, Shine_1, Shine_2
    }

    public skill qSkill;
    public skill eSkill;
    public skill rSkill;
    bool skillBreak; // Stop skill

    [Space]
    [Header("Camera Shaker script")]
    public CameraManager cameraManager;

    // Check whether player using mp or not.
    private bool isUseMp;
    private bool isRecoverMp; 
    // Check whether player attack or not.
    private bool isAttack;

    [Header("Damage")]
    public static int normalDamage = 10; // player normal attack damage.

    [Header("Effects")]
    public GameObject targetMarker;
    public GameObject TargetMarker2;
    public Transform parentPlace;
   
    private int currNumber;
    
    [Space]
    public float skillsRange = 6f;
    public float[] castingTime; //If 0 - can loop, if > 0 - one shot time

    [Space]
    private bool useUlt = false;   
    private bool canUlt = false;
    private bool casting;
    private bool[] fastSkillrefresh;


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
    public bool canMove;
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
        cameraManager = Player.instance.playerCamera.GetComponent<CameraManager>();
        fastSkillrefresh = new bool[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            fastSkillrefresh[i] = false;
        }

        casting = false;
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

        ///// Skill /////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.Q) && Player.instance && Player.instance.mp > 0) // Use Q Skill
        {
            if (canMove)
                Skill(qSkill);
            else
            {
                skillBreak = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && Player.instance && Player.instance.mp > 0) // Use E Skill
        {
            if (canMove)
                Skill(eSkill);
            else
            {
                skillBreak = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && Player.instance && Player.instance.mp > 0) // Use R Skill
        {
            if (canMove)
                Skill(rSkill);
            else
            {
                skillBreak = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////

        if (!canMove)
            return;

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
            if (fireCountdown <= 0f && canMove)
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


    private void Skill(skill usedSkill)
    {
        switch(usedSkill)
        {
            case skill.Angel_2 : // Skill 1

                if (fastSkillrefresh[1] == false)
                    StartCoroutine(FastPlay(1, 0, 2.5f));
                
                break;

            case skill.EnergyStrike_1: // Skill 2

                var targetIndex = TargetIndex();
                Transform skillTarget = null;
                if (screenTargets.Count > targetIndex)
                    skillTarget = screenTargets[targetIndex];

                if (skillTarget)
                {
                    GameObject buff = Instantiate(prefabsCast[9], skillTarget.position, skillTarget.rotation);
                    buff.transform.parent = skillTarget;
                    ParticleSystem buffPS = buff.GetComponent<ParticleSystem>();
                    Destroy(buff, buffPS.main.duration);
                    effect = prefabs[9].GetComponent<ParticleSystem>();
                    effect.Play();
                    if (prefabs[9].GetComponent<AudioSource>())
                    {
                        soundComponent = prefabs[9].GetComponent<AudioSource>();
                        clip = soundComponent.clip;
                        soundComponent.PlayOneShot(clip);
                    }
                    StartCoroutine(cameraManager.Shake(0.15f, 2, 0.2f, 0));
                }

                break;

            case skill.Energy_1: // Skill 3

                canMove = false;
                if (canUlt)
                {
                    useUlt = true;
                }
                else
                    StartCoroutine(PreCast(6));
                
                break;

            case skill.FrontExplosion_2: // Skill 4

                canMove = false;
                StartCoroutine(FrontAttack(4));
                break;

            case skill.LightRay_1: // Skill 5

                if (canUlt)
                {
                    useUlt = true;
                }
                else
                    StartCoroutine(PreCast(0));
                break;

            case skill.Magic_1: // Skill 6

                if (fastSkillrefresh[7] == false)
                    StartCoroutine(FastPlay(7, 1.1f, 0.5f));
                break;

            case skill.Nature_1: // Skill 7

                if (fastSkillrefresh[2] == false)
                    StartCoroutine(FastPlay(2, 0.35f, 2.5f));

                break;

            case skill.Shine_1: // Skill 8

                if (fastSkillrefresh[3] == false)
                    StartCoroutine(FastPlay(3, 0, 5));

                break;

            case skill.Shine_2: // Skill 9

                if (fastSkillrefresh[5] == false)
                    StartCoroutine(FastPlay(5, 1.5f, 2.5f));

                break;
        }
        if (skillBreak)
        {
            skillBreak = false;
        }
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

    public IEnumerator FrontAttack(int EffectNumber)
    {
        if (TargetMarker2 && casting == false)
        {
            aim.enabled = false;
            TargetMarker2.SetActive(true);
            //Waiting for confirm or deny
            while (true)
            {
                
                var forwardCamera = Camera.main.transform.forward;
                forwardCamera.y = 0.0f;
                TargetMarker2.transform.rotation = Quaternion.LookRotation(forwardCamera);
                var vecPos = transform.position + forwardCamera * 4;

                if (Input.GetMouseButtonDown(0) && casting == false)
                {
                    Player.instance.movement.enabled = false;
                    cameraManager.stop = true;
                    casting = true;
                    canMove = false;
                    TargetMarker2.SetActive(false);
                    
                    StartCoroutine(cameraManager.Shake(0.4f, 7, 0.45f, 1f));

                    //Play sound FX if exist
                    if (prefabs[EffectNumber].GetComponent<AudioSource>())
                    {
                        soundComponent = prefabs[EffectNumber].GetComponent<AudioSource>();
                        MainSoundPlay();
                    }

                    yield return new WaitForSeconds(1);
                    foreach (var component in prefabs[EffectNumber].GetComponentsInChildren<FrontAttack>())
                    {
                        component.PrepeareAttack(vecPos);
                    }
                    
                    yield return new WaitForSeconds(castingTime[EffectNumber]);
                    StopCasting(EffectNumber);
                    aim.enabled = true;
                    Player.instance.movement.enabled = true;
                    cameraManager.stop = false;
                    yield break;
                }
                else if (Input.GetMouseButtonDown(1) || skillBreak)
                {
                    skillBreak = false;
                    canMove = true;
                    TargetMarker2.SetActive(false);
                    aim.enabled = true;
                    yield break;
                }
                yield return null;
            }
        }
    }

    public IEnumerator PreCast(int EffectNumber)
    {
        if (prefabsCast[EffectNumber] && targetMarker)
        {
            //Waiting for confirm or deny
            while (true)
            {
                aim.enabled = false;
                targetMarker.SetActive(true);
                var forwardCamera = Camera.main.transform.forward;
                forwardCamera.y = 0.0f;
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.transform.position + new Vector3(0, 2, 0), Camera.main.transform.forward);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, collidingLayer))
                {
                    targetMarker.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                    targetMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.LookRotation(forwardCamera);
                }
                else
                {
                    aim.enabled = true;
                    targetMarker.SetActive(false);
                }

                if (Input.GetMouseButtonDown(0) && casting == false)
                {
                    aim.enabled = true;
                    targetMarker.SetActive(false);
                    soundComponentCast = null;
                    
                    casting = true;
                    prefabsCast[EffectNumber].transform.position = hit.point;
                    prefabsCast[EffectNumber].transform.rotation = Quaternion.LookRotation(forwardCamera);
                    prefabsCast[EffectNumber].transform.parent = null;
                    currEffect = prefabsCast[EffectNumber].GetComponent<ParticleSystem>();
                    effect = prefabs[EffectNumber].GetComponent<ParticleSystem>();
                    effect.Play();
                    //Get Audiosource from Prefabs if exist
                    if (prefabs[EffectNumber].GetComponent<AudioSource>())
                    {
                        soundComponent = prefabs[EffectNumber].GetComponent<AudioSource>();
                    }
                    //Get Audiosource from PrefabsCast if exist
                    if (prefabsCast[EffectNumber].GetComponent<AudioSource>())
                    {
                        soundComponentCast = prefabsCast[EffectNumber].GetComponent<AudioSource>();
                    }
                    StartCoroutine(OnCast(EffectNumber));
                    StartCoroutine(Attack(EffectNumber));
                    yield break;
                }
                else if (Input.GetMouseButtonDown(1) || skillBreak)
                {
                    canMove = true;
                    skillBreak = false;
                    aim.enabled = true;
                    targetMarker.SetActive(false);
                    yield break;
                }
                yield return null;
            }
        }
        else if (casting == false)
        {
            effect = prefabs[EffectNumber].GetComponent<ParticleSystem>();
            ////Get Audiosource from prefab if exist
            if (prefabs[EffectNumber].GetComponent<AudioSource>())
            {
                soundComponent = prefabs[EffectNumber].GetComponent<AudioSource>();
            }
            casting = true;
            StartCoroutine(Attack(EffectNumber));
            yield break;
        }
        else
            yield break;
    }
    IEnumerator OnCast(int EffectNumber)
    {
        while (true)
        {
            if (casting)
            {
                if (castingTime[EffectNumber] == 0)
                {
                    //Play PrefabCast VFX
                    currEffect.Play();
                    if (soundComponentCast)
                    {
                        CastSoundPlay();
                    }
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    //Play PrefabCast VFX
                    currEffect.Play();
                    //Camera shake
                    if (EffectNumber == 0) StartCoroutine(cameraManager.Shake(0.2f, 5, 2, 1.5f));
                    if (EffectNumber == 3) StartCoroutine(cameraManager.Shake(0.6f, 6, 0.3f, 1.45f));
                    if (soundComponentCast)
                    {
                        CastSoundPlay();
                    }
                    yield return new WaitForSeconds(castingTime[EffectNumber]);
                    yield break;
                }
            }
            else yield break;
        }
    }
    public IEnumerator Attack(int EffectNumber)
    {
        //Block moving after using the skill
        canMove = false;
        //SetAnimZero;

        while (true)
        {
            if (casting)
            {
                if (castingTime[EffectNumber] == 0)
                {
                    //Activate animation
                    if (EffectNumber == 2)
                    {
                        //anim.SetTrigger("Attack1");
                        StartCoroutine(cameraManager.Shake(0.2f, 6, 1.5f, 0));
                    }
                    //Activate repeating casting on the character
                    effect.Play();
                    //Play sound FX if exist
                    if (soundComponent)
                    {
                        MainSoundPlay();
                    }
                    yield return new WaitForSeconds(0.9f);
                }
                else
                {
                    //Activate animation
                    if (EffectNumber == 0)
                    {
                        //anim.SetTrigger("Attack2");
                        StartCoroutine(cameraManager.Shake(0.3f, 6, 3, 0));
                    }
                    if (EffectNumber == 6)
                    {
                        //anim.SetTrigger("Attack2");
                        StartCoroutine(cameraManager.Shake(0.45f, 6, 0.5f, 0.8f));
                    }
                    if (EffectNumber == 3)
                    {
                        //anim.SetTrigger("Attack2");
                        StartCoroutine(cameraManager.Shake(0.2f, 5, 3, 0));
                    }
                    /*if (EffectNumber == 1)
                    {
                        anim.SetTrigger("Idle");
                    }*/
                    //Effect.Play();
                    //Play sound FX if exist
                    if (soundComponent)
                    {
                        MainSoundPlay();
                    }
                    yield return new WaitForSeconds(castingTime[EffectNumber]);
                    StopCasting(EffectNumber);
                    yield break;
                }
            }
            else
            {
                StopCasting(EffectNumber);
                yield break;
            }
            yield return null;
        }
    }
    public void MainSoundPlay()
    {
        clip = soundComponent.clip;
        soundComponent.PlayOneShot(clip);
    }

    public void CastSoundPlay()
    {
        soundComponentCast.Play(0);
    }

    public void StopCasting(int EffectNumber)
    {
        soundComponent = null;
        soundComponentCast = null;
        if (prefabsCast[EffectNumber])
        {
            prefabsCast[EffectNumber].transform.parent = parentPlace;
            prefabsCast[EffectNumber].transform.localPosition = new Vector3(0, 0, 0);
        }
        currNumber = EffectNumber;
        casting = false;
        canMove = true;
    }

    public IEnumerator FastPlayTimer(int EffectNumber) // Skill Effect Timer
    {
        fastSkillrefresh[EffectNumber] = true;
        yield return new WaitForSeconds(castingTime[EffectNumber]);
        fastSkillrefresh[EffectNumber] = false;
        yield break;
    }

    public IEnumerator FastPlay(int EffectNumber, float castDelay, float endDelay) // Skill Effect
    {
        //Delay after next use
        StartCoroutine(FastPlayTimer(EffectNumber));

        //Get Audiosource from Prefabs if exist
        if (prefabs[EffectNumber].GetComponent<AudioSource>())
        {
            soundComponent = prefabs[EffectNumber].GetComponent<AudioSource>();
            var clip = soundComponent.clip;
            soundComponent.PlayOneShot(clip);
        }

        //Shake camera and stop moving
        if (EffectNumber == 1 || EffectNumber == 3) 
        {
            StartCoroutine(cameraManager.Shake(0.3f, 5, 0.5f, 0));
        }
        if (EffectNumber == 2 || EffectNumber == 5 || EffectNumber == 7)
        {
            casting = true;
            canMove = false;
            //SetAnimZero;
            //anim.SetTrigger("Attack3");
            StartCoroutine(cameraManager.Shake(0.4f, 5, 1f, 0.4f));
            if (EffectNumber == 5) StartCoroutine(cameraManager.Shake(0.5f, 5, 2f, castDelay));
            if (EffectNumber == 7) yield return new WaitForSeconds(0.4f);
        }


        //Use FrontAttack script if exist (it has own settings)
        if (prefabs[EffectNumber].GetComponent<FrontAttack>() != null)
        {
            foreach (var component in prefabs[EffectNumber].GetComponentsInChildren<FrontAttack>())
            {
                component.playMeshEffect = true;
            }
            foreach (Transform skillTarget in screenTargets)
            {
                var dist = Vector3.Distance(skillTarget.position, transform.position);
                if (dist <= skillsRange)
                {
                    GameObject targetEffect = Instantiate(prefabsCast[EffectNumber], skillTarget.position, skillTarget.rotation);
                    targetEffect.transform.parent = skillTarget;
                    Destroy(targetEffect, castingTime[EffectNumber]);
                }
            }
            yield return new WaitForSeconds(castingTime[EffectNumber]);
        }
        else
        {
            effect = prefabs[EffectNumber].GetComponent<ParticleSystem>();
            effect.Play();

            if (EffectNumber == 7)
            {
                prefabs[EffectNumber].transform.parent = null;
                foreach (Transform skillTarget in screenTargets)
                {
                    var dist = Vector3.Distance(skillTarget.position, transform.position);
                    if (dist <= skillsRange)
                    {
                        GameObject targetEffect = Instantiate(prefabsCast[EffectNumber], skillTarget.position, skillTarget.rotation);
                        targetEffect.transform.parent = skillTarget;
                        Destroy(targetEffect, castingTime[EffectNumber]);
                    }
                }
                yield return new WaitForSeconds(castDelay);
                casting = false;
                canMove = true;
                yield return new WaitForSeconds(endDelay);
                prefabs[EffectNumber].transform.parent = parentPlace;
                prefabs[EffectNumber].transform.position = parentPlace.position;
            }
            else
            {
                //Delay before main skill activation
                yield return new WaitForSeconds(castDelay);

                prefabsCast[EffectNumber].transform.parent = null;
                currEffect = prefabsCast[EffectNumber].GetComponent<ParticleSystem>();
                currEffect.Play();

                //Get Audiosource from PrefabsCast if exist
                if (prefabsCast[EffectNumber].GetComponent<AudioSource>())
                {
                    soundComponentCast = prefabsCast[EffectNumber].GetComponent<AudioSource>();
                    var clip = soundComponentCast.clip;
                    soundComponentCast.PlayOneShot(clip);
                }

                //Stop fast move after endings
                if (EffectNumber == 2)
                {
                    yield return new WaitForSeconds(1);
                    //SetAnimZero;
                }

                casting = false;
                canMove = true;

                //Delay so that the effect returns to the character only after stopping
                yield return new WaitForSeconds(endDelay);
                prefabsCast[EffectNumber].transform.parent = parentPlace;
                prefabsCast[EffectNumber].transform.position = parentPlace.position;
            }
        }
        yield break;
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
