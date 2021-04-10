using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Interlocutor
{
    None,
    Character1,
    Character2
}

public class DialogDisplayer : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text descriptionText, characterText, character1Name, character2Name;
    [SerializeField] Image p1Image, p2Image;
    [SerializeField] private List<ImageLinker> sos = new List<ImageLinker>();

    private Interlocutor interlocutor = (Interlocutor)(-1);

    public event Action Continue;
    public event Action NextScene;


    private void Start()
    {
        SetInterlocutor(Interlocutor.None, "");
        SetText("");
    }

    public void OnClick_Continue()
    {
	    Continue?.Invoke();
    }

	public void OnClick_NextScene()
	{
		NextScene?.Invoke();
	}

public void SetText(string text)
    {
        switch (interlocutor)
        {
            case Interlocutor.None:
                descriptionText.text = text;

                break;
            case Interlocutor.Character1:
            case Interlocutor.Character2:
                characterText.text = text;
                break;
        }
    }

    public void SetName(string name)
    {
        switch (interlocutor)
        {
            case Interlocutor.Character1:
                character1Name.text = name;
                character1Name.fontStyle = FontStyles.Bold;
                break;

            case Interlocutor.Character2:
                character2Name.text = name;
                character2Name.fontStyle = FontStyles.Bold;
                break;
        }
    }

    public void SetInterlocutor(Interlocutor i, string name)
    {
        if (interlocutor == i)
            return;

        interlocutor = i;

        switch (i)
        {
            case Interlocutor.None:
                character1Name.enabled = false;
                character2Name.enabled = false;
                p1Image.enabled = false;
                p2Image.enabled = false;
                characterText.enabled = false;
                descriptionText.enabled = true;
                break;

            case Interlocutor.Character1:
                character1Name.enabled = true;
                character2Name.enabled = false;
                p1Image.enabled = true;
                p2Image.enabled = false;
                characterText.enabled = true;
                descriptionText.enabled = false;

                foreach (var so in sos)
                {
	                if (name.Contains(so.name))
	                {
		                p1Image.material.mainTexture = so.img;
		                break;
	                }
	                else
	                {
		                p2Image.material.mainTexture = null;
	                }
                }
                break;

            case Interlocutor.Character2:
                character1Name.enabled = false;
                character2Name.enabled = true;
                p1Image.enabled = false;
                p2Image.enabled = true;
                characterText.enabled = true;
                descriptionText.enabled = false;

                foreach (var so in sos)
                {
	                if (name.Contains(so.name))
	                {
		                p2Image.material.mainTexture = so.img;
		                break;
	                }
	                else
	                {
		                p2Image.material.mainTexture = null;
                    }

                }
                break;
        }
    }
}
