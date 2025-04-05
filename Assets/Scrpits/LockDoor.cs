using UnityEngine;

public class LockDoor : MonoBehaviour
{
    private Animator anim;

    [Header("SoundEF")]
    public AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        LockDoorManager.Instance.RegisterDoor(this); // Đăng ký vào Manager
    }

    public void PlayAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Open"); // Đảm bảo Animation Controller có parameter "Open"
            
        }
        else
        {
            Debug.LogWarning("Animator không tìm thấy trên " + gameObject.name);
        }
    }

    public void ONsounf()
    {
        audioSource.PlayOneShot(jumpSound);
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
