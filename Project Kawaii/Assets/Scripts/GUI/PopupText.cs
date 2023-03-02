using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PopupText : MonoBehaviour
{
    public float TimerForMessages;
    private bool cStarted;
    private TextMeshProUGUI TextMesh;
    private List<string> MessagesToDo;
    public static string ScreenText = "";
    public Image Main;
    public Image BG;

    void Start()
    {
        MessagesToDo = new List<string>(new string[0]);
        TextMesh = GetComponent<TextMeshProUGUI>();
        TextMesh.text = "";
    }

    void Update()
    {
        HandleText();
    }

    private void HandleText()
    {
        if (ScreenText != "")
        {
            MessagesToDo.Add(ScreenText);
            ScreenText = "";
        }
        if (cStarted == false && MessagesToDo.Count > 0)
        {
            StartCoroutine(SetText());
            if (Main != null)
                Main.enabled = true;
            if (BG != null)
                BG.enabled = true;
        }
        else if (MessagesToDo.Count <= 0)
        {
            if (Main != null)
                Main.enabled = false;
            if (BG != null)
                BG.enabled = false;
        }

        if(TextMesh.text != "")
        {
            if (Main != null)
                Main.enabled = true;
            if (BG != null)
                BG.enabled = true;
        }
        else
        {
            if (Main != null)
                Main.enabled = false;
            if (BG != null)
                BG.enabled = false;
        }

    }

    IEnumerator SetText()
    {
        //Prevent Multiple Coroutines from running
        cStarted = true;
        for (int i = 0; i < MessagesToDo.Count; i++)
        {
            //Setting text to oldest (first) message
            TextMesh.text = MessagesToDo[0];
            //Wait time between messages
            yield return new WaitForSeconds(TimerForMessages);
            //Removes Shown Message
            MessagesToDo.Remove(MessagesToDo[0]);
            //Check if there's any messages left
            if (MessagesToDo.Count > 0)
            {
                TextMesh.text = MessagesToDo[0];
            }
            //Checking if last message and making sure it has a timer before breaking out of loop
            //if (i == MessagesToDo.Count - 1 && MessagesToDo.Count > 0)
            //{
            //    yield return new WaitForSeconds(TimerForMessages);
            //}
        }
        //Setting the text back to blank
        TextMesh.text = "";
        //Allows the next to start
        cStarted = false;
    }
}
