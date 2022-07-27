/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutomaticTargetingAttack : MonoBehaviour
{
    [Header("Play on collision")]

    [Space]
    public GameObject[] effectsOnCollision;

    [Space]
    [Header("Target")]
    public float targetLayerNumber;
    public float destroyTimeDelay = 5;

    [Space]
    [Header("Target Aim")]
    public float offset = 0;
    public Vector3 rotationOffset = new Vector3(0,0,0);

    [Space]
    [Header("0n Particle Collision Setting")]

    public bool destoyMainEffect = true;

    // Personal setting
    public bool useWorldSpacePosition;     
    public bool useOnlyRotationOffset = true; 
    public bool useFirePointRotation; 

    private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        particle = GetComponent<ParticleSystem>();

        Destroy(gameObject, destroyTimeDelay + 0.5f);
    }
    void OnParticleCollision(GameObject other)
    {

        if (other.layer == targetLayerNumber)
        {
            // 
            int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < numCollisionEvents; i++)
            {
                foreach (var effect in effectsOnCollision) // Check All Collision.
                {
                    var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * offset, new Quaternion()) as GameObject;
                    instance.GetComponent<SkillAttack>().skillDamageValue = PlayerAttack.normalDamage; // Skill damage setting.

                    if (!useWorldSpacePosition)
                        instance.transform.parent = transform;

                    if (useFirePointRotation)
                    {
                        instance.transform.LookAt(transform.position);
                    }
                    else if (rotationOffset != Vector3.zero && useOnlyRotationOffset)
                    {
                        instance.transform.rotation = Quaternion.Euler(rotationOffset);
                    }
                    else
                    {
                        instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                        instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                    }
                    Destroy(instance, destroyTimeDelay);
                }
            }
            if (destoyMainEffect == true)
            {
                Destroy(gameObject, destroyTimeDelay + 0.5f);
            }
        }
    }
}
