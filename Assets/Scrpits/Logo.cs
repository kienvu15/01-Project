using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public Animator logoanimator;
    public Animator leftanimator;
    public Animator rightanimator;

    public FlashEffect flashEffect;

    public Button playbutton;
    public Button soundbutton;
    public Button settingbutton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logoanimator.Play("logoappear");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void animLogo()
    {
        logoanimator.Play("Logo");
        StartCoroutine(flashEffect.StartFlash());
        playbutton.gameObject.SetActive(true);
        soundbutton.gameObject.SetActive(true);
        settingbutton.gameObject.SetActive(true);
    }
    public void PauseAnimation()
    {
        leftanimator.Play("stay");
    }
    public void Pause2Animation()
    {
        rightanimator.Play("stay2");
    }
}
