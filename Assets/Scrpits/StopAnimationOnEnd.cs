using UnityEngine;

public class StopAnimationOnEnd : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StopAnimation()
    {
        anim.enabled = false;  // Dừng animation khi đến frame cuối
    }
    public void Destroyit()
    {
        Destroy(gameObject);
    }
}
