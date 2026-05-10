using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonFeedback : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    public Image buttonImage;

    public Sprite normalSprite;
    public Sprite pressedSprite;

    public AudioSource audioSource;

    public string sceneToLoad = "GameScene";

    private Vector3 originalScale;

    private bool isPressed = false;

    public float clickDelay = 0.3f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressed) return;

        isPressed = true;

        buttonImage.sprite = pressedSprite;

        transform.localScale = originalScale * 1.1f;

        audioSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(ButtonRelease());
    }

    IEnumerator ButtonRelease()
    {
        // 恢复原图
        buttonImage.sprite = normalSprite;

        // 恢复大小
        transform.localScale = originalScale;

        // 等动画感觉结束
        yield return new WaitForSeconds(clickDelay);

        // 切换场景
        SceneManager.LoadScene(sceneToLoad);
    }
}