using System.Collections;
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
        StartCoroutine(DestroyAfterAnimation(gameObject));

    }

    IEnumerator DestroyAfterAnimation(GameObject obj)
    {
        Animator anim = obj.GetComponent<Animator>();
        if (anim != null)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(obj);
    }

}
