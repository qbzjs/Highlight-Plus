using UnityEngine;

public class Fader : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
            anim.SetTrigger("In");
    }

    public void FadeOut()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FadeOut"))
            anim.SetTrigger("Out");
    }
}