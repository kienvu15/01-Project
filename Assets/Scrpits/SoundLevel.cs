using UnityEngine;
using UnityEngine.UI;

public class SoundLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SoundManager Instance;
    public AudioSource audioSource;
    public Button muteButton;
    public Sprite soundOnIcon, soundOffIcon;

    private bool isMuted = false;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Start()
    {
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        UpdateSound();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSound();
    }

    private void UpdateSound()
    {
        if (audioSource != null)
            audioSource.mute = isMuted;

        if (muteButton != null)
            muteButton.image.sprite = isMuted ? soundOffIcon : soundOnIcon;
    }

    public void SetMuteButton(Button button)
    {
        muteButton = button;
        UpdateSound();
        muteButton.onClick.AddListener(ToggleSound);
    }
}
