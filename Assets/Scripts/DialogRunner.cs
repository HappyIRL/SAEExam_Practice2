using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

public class DialogRunner : MonoBehaviour
{
    [SerializeField] DialogDisplayer dialogDisplayer;

    public List<string> filesToImport = new List<string>();
    private List<TextAsset> texts = new List<TextAsset>();
    private List<List<string[]>> sceneDialog = new List<List<string[]>>();

	private bool char1Talking = true;

	private int continueCounter = -1;

	private int scene = 0;

    private void Start()
    {
	    if (filesToImport.Count <= 0)
	    {
			Debug.LogError("NO TEXT FILES at: " + this);
			return;
	    }

	    foreach (string file in filesToImport)
	    {
		    List<string[]> charDialog = new List<string[]>();

		    TextAsset textFile = (TextAsset)Resources.Load(file, typeof(TextAsset));
		    texts.Add(textFile);

		    string text = textFile.text;

		    string[] textLines = Regex.Split(text, "</b>|<b>");

		    for (int i = 1; i < textLines.Length; i += 2)
		    {
			    string[] charTxt = new string[2];

			    charTxt[0] = textLines[i].Replace("  ", "");
			    charTxt[1] = textLines[i + 1].Replace("  ", "");

			    charDialog.Add(charTxt);
		    }

			sceneDialog.Add(charDialog);
		}
    }
	private  void OnEnable()
    {
	    dialogDisplayer.Continue += OnContinue;
	    dialogDisplayer.NextScene += OnNextScene;

    }

    private void OnDisable()
    {
	    dialogDisplayer.Continue -= OnContinue;
	    dialogDisplayer.NextScene -= OnNextScene;
    }

	private void OnNextScene()
    {
	    scene++;

	    if (scene >= sceneDialog.Count)
	    {
		    return;
	    }
			
	    continueCounter = 0;

	    dialogDisplayer.SetInterlocutor(Interlocutor.Character1, sceneDialog[scene][continueCounter][0]);
	    dialogDisplayer.SetName(sceneDialog[scene][continueCounter][0]);
	    dialogDisplayer.SetText(sceneDialog[scene][continueCounter][1]);
	    char1Talking = false;
	}

    private void OnContinue()
    {
	    continueCounter++;

	    if (continueCounter >= sceneDialog[scene].Count)
	    {
		    return;
	    }

	    if (char1Talking)
	    {
		    dialogDisplayer.SetInterlocutor(Interlocutor.Character1, sceneDialog[scene][continueCounter][0]);
		    dialogDisplayer.SetName(sceneDialog[scene][continueCounter][0]);
		    dialogDisplayer.SetText(sceneDialog[scene][continueCounter][1]);
		    char1Talking = false;
	    }
	    else
	    {
		    dialogDisplayer.SetInterlocutor(Interlocutor.Character2, sceneDialog[scene][continueCounter][0]);
		    dialogDisplayer.SetName(sceneDialog[scene][continueCounter][0]);
		    dialogDisplayer.SetText(sceneDialog[scene][continueCounter][1]);
		    char1Talking = true;
	    }
    }
}
