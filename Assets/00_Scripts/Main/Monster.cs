using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    NavMeshAgent agent;


    [SerializeField] private float Range;

    Coroutine hit_Coroutine;

    Renderer renderer;
    Animator animator;

    Transform target;

    bool isAttack = false;
    bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        HP = MaxHP;
        renderer = transform.GetComponentInChildren<Renderer>();

        AnimationChange("IDLE", false);
        StartCoroutine(FindPlayer());
    }

    private void AnimationChange(string temp, bool isTrigger = false)
    {
        animator.SetBool("IDLE", false);
        animator.SetBool("WALK", false);

        if(isTrigger)
        {
            animator.SetTrigger(temp);
        }
        else
        {
            animator.SetBool(temp, true);
        }
    }

    private void Attack()
    {
        P_Movement.instance.GetDamage(15);
    }
    
    private void Update()
    {
        if (isDead) return;
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if(distance > 2.0f && distance <= 10.0f)
        {
            StopMovement(false);

            if(!animator.GetBool("WALK"))
            {
                AnimationChange("WALK", false);
            }

            agent.SetDestination(target.position);
        }
        else if (distance < 2.0f)
        {
            StopMovement(true);

            if(isAttack == false)
            {
                AttackPlayer();
            }
        }
        else if (distance > 10.0f)
        {
            StopMovement(false);
            AnimationChange("WALK", false);
            target = null;
        }
    }

    private void StopMovement(bool Can)
    {
        agent.isStopped = Can;
        if(Can)
        {
            agent.velocity = Vector3.zero;
        }
    }

    private void AttackPlayer()
    {
        isAttack = true;
        AnimationChange("ATTACK", true);
        Invoke("AttackReturn", 1.0f);
    }

    IEnumerator FindPlayer()
    {
        float distance = Vector3.Distance(transform.position, P_Movement.instance.transform.position);
        if (target == null)
        {
            if(!animator.GetBool("IDLE"))
            {
                AnimationChange("IDLE", false);
            }

            if (distance <= 5.0f)
            {
                target = P_Movement.instance.transform;
                AnimationChange("WALK", false);
            }
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FindPlayer());
    }

    private void AttackReturn() => isAttack = false;

    public void GetDamage(int dmg)
    {
        if (isDead) return;

        var playerPos = P_Movement.instance.transform.position;
        if(Vector3.Distance(transform.position, playerPos) <= Range)
        {
            Canvas_Holder.instance.GetText(dmg.ToString(), Color.yellow, transform.position);
            HP -= dmg;
            Canvas_Holder.instance.AddSlider(this);
            P_Movement.instance.GetComponent<Character>().GetHitParticle();

            if(hit_Coroutine != null) StopCoroutine(hit_Coroutine);
            hit_Coroutine = StartCoroutine(GetHitCoroutine());

            if(HP <= 0)
            {
                isDead = true;
                StopAllCoroutines();
                StopMovement(true);
                Canvas_Holder.instance.RemoveSlider(this);
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                AnimationChange("DIE", true);
                Destroy(this.gameObject, 1.5f);
            }
        }
    }

    IEnumerator GetHitCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        Color startColor = Color.white;
        Color endColor = Color.black;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color LerpColor = Color.Lerp(endColor, startColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor);
            yield return null;
        }

        current = 0.0f;
        percent = 0.0f;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color LerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor);
            yield return null;
        }
    }

    

}
