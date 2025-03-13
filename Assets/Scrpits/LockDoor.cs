using UnityEngine;

public class LockDoor : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Hàm chạy animation
    public void PlayAnimation()
    {
        if (anim != null)
        {
            anim.Play("LockDoorDisappear"); // Đảm bảo animation có parameter trigger "Open"
        }
        else
        {
            Debug.LogWarning("Animator chưa được gán trên object này!");
        }
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
    public void Actice()
    {
        gameObject.SetActive(true);
    }
    public void DoneAnim()
    {
        Destroy(gameObject);
    }
}
