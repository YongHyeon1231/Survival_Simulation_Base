using UnityEngine;

public class UI_Animation_Handler : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // private void Start()
    // {
    //     animator = GetComponent<Animator>();
    // }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    public void Deactive() => gameObject.SetActive(false);

    public void Destroy_Object() => Destroy(gameObject);
}
