using UnityEngine;
using System;
using VRTK;

[RequireComponent(typeof(Animator))]

public class Monsters : MonoBehaviour
{
    Animator anim;
    public int AC = 10;
    public int HP = 10;
    public Transform player;
    public float RegionOfSight = 10f;
    public float AttackSpeed = 1f;
    float attackTimeLeft;
    public float HitRate = 1f;
    float hitDelay;
    Boolean canBeHit;
    public GameObject PlayerHPBar;
    public HealthBar playerHP;
    private float deathTimer;
    public int MeleeModifer = 0;
    public int RangedModifer = 0;
    public int MagicModifer = 0;
    public float AttackRange = 5f;
    public int score = 25;
    public int attackDice = 8;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attackTimeLeft = 1 / AttackSpeed;
        hitDelay = HitRate;
        canBeHit = true;
        playerHP = PlayerHPBar.GetComponent<HealthBar>();
        deathTimer = 5f;
    }

    void Update()
    {
        if (HP <= 0)
        {
            anim.SetBool("Alive", false);
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(gameObject);
                playerHP.score += score;
            }
        }
        else
        {
            float t = Time.deltaTime;
            attackTimeLeft -= t;
            hitDelay -= t;
            if (hitDelay <= 0)
            {
                if (!canBeHit)
                {
                    canBeHit = true;
                }
                hitDelay = HitRate;

            }
            anim.SetBool("Alive", true);
            if(Vector3.Distance(player.position, this.transform.position) < RegionOfSight)
            {
                Vector3 direction = player.position - this.transform.position;
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.05f);
                float speed;
                if (direction.magnitude > AttackRange)
                {
                    speed = (direction.magnitude - AttackRange) / RegionOfSight;
                    anim.SetFloat("Forward", speed);
                }
                else
                {
                    if (attackTimeLeft <= 0)
                    {
                        attackTimeLeft = 1 / AttackSpeed;
                        anim.SetInteger("Action", UnityEngine.Random.Range(0, 6));
                        anim.SetTrigger("Attack");
                        if (UnityEngine.Random.Range(1, 20) >= playerHP.AC)
                        {
                            playerHP.CurrentHP = playerHP.CurrentHP - UnityEngine.Random.Range(1, attackDice);
                        }
                    }
                    anim.SetFloat("Forward", 0f);
                }
            }
            else
            {
                anim.SetFloat("Forward", 0f);
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Particles particle = other.GetComponent<Particles>();
        float toHit = UnityEngine.Random.Range(1, 20) + MagicModifer;
        if (HP >= 0 && canBeHit)
        {
            if (toHit >= AC)
            {
                anim.SetTrigger("GotHit");
                int damage = UnityEngine.Random.Range(1, particle.dice);
                HP = HP - damage;
            }
            else
            {
                int action = UnityEngine.Random.Range(0, 2);
                if (action != 2)
                {
                    anim.SetInteger("Action", action);
                    anim.SetTrigger("Dodging");
                }
            }
            canBeHit = false;
        }
        Destroy(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sword" && canBeHit)
        {
            VRTK.Examples.Sword sword = collision.gameObject.GetComponent<VRTK.Examples.Sword>();
            float toHit = UnityEngine.Random.Range(1, 20) + MeleeModifer;
            if (toHit >= AC)
            {
                anim.SetTrigger("GotHit");
                int damage = UnityEngine.Random.Range(1, 10);
                if (sword.glowing)
                {
                    Debug.Log("Extra Damage!");
                    damage += UnityEngine.Random.Range(1, 4);
                }
                float hapticStrength = damage / 10f;
                sword.Rumble(hapticStrength);
                HP = HP - damage;
                
            }
            else
            {
                int action = UnityEngine.Random.Range(0, 2);
                if (action != 2)
                {
                    anim.SetInteger("Action", action);
                    anim.SetTrigger("Dodging");
                }
            }
            canBeHit = false;
        }
        if (collision.collider.tag == "Bow" && canBeHit)
        {
            float toHit = UnityEngine.Random.Range(1, 20)+RangedModifer;
            if (toHit >= AC)
            {
                anim.SetTrigger("GotHit");
                int damage = UnityEngine.Random.Range(1, 10);
                HP = HP - damage;

            }
            else
            {
                int action = UnityEngine.Random.Range(0, 2);
                if (action != 2)
                {
                    anim.SetInteger("Action", action);
                    anim.SetTrigger("Dodging");
                }
            }
            canBeHit = false;
        }
    }
}

