using System.Collections;
using UnityEngine;

public class QuitPanelController : MonoBehaviour
{
    public GameObject quitPanel;

    public AudioSource buttonAudio;

    public float closeDelay = 0.3f;

    public void OpenQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void CloseQuitPanel()
    {
        StartCoroutine(ClosePanelDelay());
    }

    IEnumerator ClosePanelDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        quitPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}