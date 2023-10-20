using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notify : MonoBehaviour
{
    public GameObject noticeUI;
    public TextMeshProUGUI noticeText;
    public static Notify instance;

    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<Notify>();
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false && noticeUI == enabled)
        {
            noticeUI.SetActive(false);
        }
    }

    public void Message(string message, float time)
    {
        noticeUI.SetActive(true);
        noticeText.text = message;
        StartCoroutine(Delay(time));
    }

    IEnumerator Delay(float time)
    {
        paused = true;
        yield return new WaitForSeconds(time);
        paused = false;
    }
}
