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
    public float offset;
    public Vector3 rotationOffset = Vector3.zero;

    [Space]
    [Header("0n Particle Collision Setting")]

    public bool destroyMainEffect = true;

    // Personal setting
    public bool useWorldSpacePosition;     
    public bool useOnlyRotationOffset = true; 
    public bool useFirePointRotation; 

    private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents = new();

    public void Start()
    {
        particle = GetComponent<ParticleSystem>();

        Destroy(gameObject, destroyTimeDelay + 0.5f);
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.layer == targetLayerNumber)
        {
            int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < numCollisionEvents; i++)
            {
                foreach (var effect in effectsOnCollision) // Check All Collision.
                {
                    var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * offset, new Quaternion());
                    instance.GetComponent<SkillAttack>().skillDamageValue = PlayerAttack.normalDamage; // Skill damage setting.

                    // Apply setting value.
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

            // Apply setting value.
            if (destroyMainEffect)
            {
                Destroy(gameObject, destroyTimeDelay + 0.5f);
            }
        }
    }
}
