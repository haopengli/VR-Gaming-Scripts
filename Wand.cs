using UnityEngine;
using VRTK;

public class Wand : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    private VRTK_ControllerReference cref;
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    private Particles proj;
    float timeDelay;
    private float charge;
    private float timer;
    private bool countdown;

    private void Start()
    {
        charge = 100f;
        cref = VRTK_ControllerReference.GetControllerReference(controller.gameObject);
        timer = 0f;
        countdown = false;
    }

    private void Update()
    {
        if (countdown)
        {
            timer += Time.deltaTime;
        }
        else
        {
            charge += Time.deltaTime * 0.25f;
        }
        cref = VRTK_ControllerReference.GetControllerReference(controller.gameObject);
        controller.TriggerPressed += DoTriggerPressed;
        controller.TriggerReleased += DoTriggerReleased;
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        countdown = true;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (charge >= 2)
        {
            int d = 4;
            if (charge >= 6)
            {
                if (timer < 0.5)
                {
                    d = 4;
                }
                else if (timer < 1)
                {
                    d = 6;
                }
                else if (timer < 1.5)
                {
                    d = 8;
                }
                else if (timer < 2)
                {
                    d = 10;
                }
                else
                {
                    d = 12;
                }
            }
            else if (charge >= 5)
            {
                if (timer < 0.5)
                {
                    d = 4;
                }
                else if (timer < 1)
                {
                    d = 6;
                }
                else if (timer < 1.5)
                {
                    d = 8;
                }
                else
                {
                    d = 10;
                }
            }
            else if (charge >= 4)
            {
                if (timer < 0.5)
                {
                    d = 4;
                }
                else if (timer < 1)
                {
                    d = 6;
                }
                else
                {
                    d = 8;
                }
            }
            else if (charge >= 3)
            {
                if (timer < 0.5)
                {
                    d = 4;
                }
                else
                {
                    d = 6;
                }
            }
            else
            {
                d = 4;
            }
            
            FireProjectile(d);
        }
        timer = 0f;
        countdown = false;
    }

    protected virtual void FireProjectile(int damage)
    {
        if (projectile != null && projectileSpawnPoint != null && countdown)
        {
            proj = projectile.GetComponent<Particles>();
            proj.dice = damage;
            Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            VRTK_ControllerHaptics.TriggerHapticPulse(cref, 65*damage, 0.1f, 0.01f);
            charge -= (float)(damage * 0.5);
        }
    }
}