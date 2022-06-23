using UnityEngine;

public class Fader : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        anim.SetTrigger("In");
    }

    public void FadeOut()
    {
        anim.SetTrigger("Out");
    }
}