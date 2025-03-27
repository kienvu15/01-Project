using UnityEngine;

public class LockDoor : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        LockDoorManager.Instance.RegisterDoor(this); // Đăng ký vào Manager
    }

    public void PlayAnimation()
    {
        if (anim != null)
        {
            anim.Play("LockDoorDisappear");
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Actice()
    {
        gameObject.SetActive(true);
    }
}
