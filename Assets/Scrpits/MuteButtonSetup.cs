using UnityEngine;
using UnityEngine.UI;

public class MuteButtonSetup : MonoBehaviour
{
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMuteButton(GetComponent<Button>());
        }
    }
}
