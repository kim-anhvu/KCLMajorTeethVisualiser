using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * This class controls the help dialogue which introduces the user
 * to the different functionalities of the application.
 * It displays sentences in the help box and runs Coroutines which 
 * execute different actions in the Scene in order to exemplify the features.
 * At the start of the dialogue, the Scene will be reset.
 * During the dialogue, the scene will not respond to most user inputs.
 * After the dialogue, control will return to the user.
 */
public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences; // Queue for storing the next sentences to be displayed
    private int sentenceIndex = 0; // Used to decide which event to trigger 
    private bool[] cr_status = new bool[11]; // Used to determin if the events for a certain demonstration
                                             // Have been completed

    private IEnumerator currSentenceType;

    public TextMeshProUGUI dialogueText; //Where to display the sentences

    public Animator animator; // animator of the help window, used to make the window pop up and down

    // Different objects from the scene, 
    // used to demonstarte functionality and limit user interaction during the dialogue
    public Button slideButton;
    public Button slideButtonColour;
    public Toggle selectAllToggle;
    public Slider zoomSlider;
    public ThirdPersonCamera cam;
    public Button loadButton;
    public Button patientDisplayButton;
    public InputField fromRad;
    public InputField toRad;
    public Button red;
    public Button green;
    public Button apply;
    public Button reset;
    public Button graphButton;
    public Button CSVbutton;
    public Button helpButton;
    public Button f_reset;
    public Button f_filter;
    public Button f_load;
    public Button f_compare;
    public GameObject graphFilterPanel;

    //Create a new Queue
    void Start() {
        sentences = new Queue<string>();
    }

    // Setup up the scene for the Dialogue
    // If there's no CSV loaded, load one in
    // Make object not interactable in order to dissalow the user from breaking the dialogue
    // Reset the sentences
    // Show the help window
    public void StartDialogue (Dialogue dialogue) {
        CSVParser csvParser = new CSVParser();
        // If there are no CSVs already loaded, load the default one
        // for demonstration purposes
        System.IO.FileInfo[] fileInfos = csvParser.GetAllFilesInDirectory();
        if(fileInfos.Length < 1) {
          LoadNewCSV.Load("data.csv");
        }
        GameObject loader = GameObject.Find("FileBrowserUI");
        if(loader != null)
        {
            GameObject.Destroy(loader);
        }
        reset.onClick.Invoke();
        f_reset.onClick.Invoke();
        graphFilterPanel.SetActive(false);
        slideButton.interactable = false;
        slideButtonColour.interactable = false;
        graphButton.interactable = false;
        helpButton.interactable = false;
        CSVbutton.interactable = false;
        patientDisplayButton.interactable = false;

        f_reset.interactable = false;
        f_filter.interactable = false;
        f_load.interactable = false;
        f_compare.interactable = false;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    
        NextSentence();

        animator.SetBool("IsOpen", true);
    }

    // Handle the passage from one sentence to the other.
    // If there'snothing left to do, close the dialogue.
    // Else handle the necessary events for that certain centence.
    public void NextSentence() {

        //If we're running coroutines we don't want to progress
        if(!DialogueLogic.IsRunningCoroutines(cr_status))
        {
            if(currSentenceType != null)
            {
                StopCoroutine(currSentenceType);
            }
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            sentenceIndex++;
            currSentenceType = TypeSentence(sentence);
            StartCoroutine(currSentenceType);
            EventTriggering();
        }
    }

    //Based on the current sentence, run the appropiate events
    public void EventTriggering() {

        if(sentenceIndex == 5){
            PanDemo();
        }
        else if(sentenceIndex == 6) {
            StartCoroutine("ZoomDemo1");
        }

        else if(sentenceIndex == 7) {
            StartCoroutine("NavBoxDemo");
        }
         else if(sentenceIndex == 10) {
            slideButton.onClick.Invoke();

         }
         else if(sentenceIndex == 13) {
            StartCoroutine("SelectAllDemo");
         }
         else if(sentenceIndex == 14) {
            patientDisplayButton.onClick.Invoke();
         }
        else if(sentenceIndex == 15)
        {
            patientDisplayButton.onClick.Invoke();
        }
        else if(sentenceIndex == 16){
            StartCoroutine("ColourDemo");
        }
        else if(sentenceIndex == 17){
            PanDemo();
        }

    }

    private void PanDemo()
    {
        StartCoroutine("PanDemoX");
        StartCoroutine("PanDemoY1");
    }

    //Each IEnumerator is a Coroutine which scripts an event for demonstration
    IEnumerator TypeSentence(string sentence)
    {
        // Text type coroutine should be skippabe
        cr_status[0] = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    IEnumerator ColourDemo()
    {
        cr_status[1] = true;
        slideButtonColour.onClick.Invoke();
        yield return new WaitForSeconds(1);
        fromRad.text = "20";
        toRad.text = "40";
        green.onClick.Invoke();
        yield return new WaitForSeconds(1);
        red.onClick.Invoke();
        yield return new WaitForSeconds(1);
        apply.onClick.Invoke();
        cr_status[1] = false;
        yield break;
    }

    IEnumerator SelectAllDemo()
    {
        cr_status[2] = true;
        selectAllToggle.isOn = false;
        yield return new WaitForSeconds(1);
        selectAllToggle.isOn = true;
        yield return new WaitForSeconds(1);
        loadButton.onClick.Invoke();
        yield return new WaitForSeconds(1);
        slideButtonColour.onClick.Invoke();
        cr_status[2] = false;
        yield break;
    }


    IEnumerator PanDemoX()
    {
        cr_status[3] = true;
        float elapsedTime = 0;
        float time = 50f;
        cam.currentX = 0f;
        float finishX = 360f;
        while (elapsedTime < time)
        {
            cam.currentX = Mathf.Lerp(cam.currentX, finishX, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if(cam.currentX >= 359.9f)
            {
                cam.currentX = 0f;
                elapsedTime = time;
                cr_status[3] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PanDemoY1()
    {
        cr_status[4] = true;
        float elapsedTime = 0;
        float time = 10f;
        float finishY = 89;
        while (elapsedTime < time)
        {
            cam.currentY = Mathf.Lerp(cam.currentY, finishY, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (cam.currentY >= 88.9f)
            {
                elapsedTime = time;
                StartCoroutine("PanDemoY2");
                cr_status[4] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PanDemoY2()
    {
        cr_status[5] = true;
        float elapsedTime = 0;
        float time = 30f;
        float finishY = -89f;
        while (elapsedTime < time)
        {
            cam.currentY = Mathf.Lerp(cam.currentY, finishY, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (cam.currentY <= -88.9f)
            {
                elapsedTime = time;
                StartCoroutine("PanDemoY3");
                cr_status[5] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PanDemoY3()
    {
        cr_status[6] = true;
        float elapsedTime = 0;
        float time = 10f;
        float finishY = 0f;
        while (elapsedTime < time)
        {
            cam.currentY = Mathf.Lerp(cam.currentY, finishY, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (cam.currentY >= -0.01f && cam.currentY <= 0.01f)
            {
                elapsedTime = time;
                cr_status[6] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    IEnumerator ZoomDemo1() {
        cr_status[7] = true;
        float elapsedTime = 0f;
        float time = 10f;
        float startDistance = zoomSlider.value;
        float endDistance = 0f;
        while (elapsedTime <= time)
        {
            zoomSlider.value = Mathf.Lerp(zoomSlider.value, endDistance, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (zoomSlider.value < 0.5f)
            {
                elapsedTime = time;
                StartCoroutine("ZoomDemo2");
                cr_status[7] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ZoomDemo2()
    {
        cr_status[8] = true;
        float elapsedTime = 0f;
        float time = 10f;
        float startDistance = zoomSlider.value;
        float endDistance = 1000f;
        while (elapsedTime < time)
        {
            zoomSlider.value = Mathf.Lerp(zoomSlider.value, endDistance, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (zoomSlider.value >= 999.99f)
            {
                elapsedTime = time;
                cr_status[8] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ZoomDemo3()
    {
        cr_status[9] = true;
        float elapsedTime = 0f;
        float time = 10f;
        float startDistance = zoomSlider.value;
        float endDistance = 500f;
        while (elapsedTime < time)
        {
            zoomSlider.value = Mathf.Lerp(zoomSlider.value, endDistance, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (zoomSlider.value <= 500.01f && zoomSlider.value >= 499.99f)
            {
                elapsedTime = time;
                cr_status[9] = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator NavBoxDemo()
    {
        cr_status[10] = true;
        cam.currentX = 0f;
        cam.currentY = 89f;
        yield return new WaitForSeconds(1);
        cam.currentX = 180f;
        cam.currentY = 0f;
        yield return new WaitForSeconds(1);
        cam.currentX = 360f;
        cam.currentY = 0f;
        yield return new WaitForSeconds(1);
        cam.currentX = 270f;
        cam.currentY = 0f;
        yield return new WaitForSeconds(1);
        cam.currentX = 90f;
        cam.currentY = 0f;
        yield return new WaitForSeconds(1);
        cam.currentX = 0f;
        cam.currentY = -89f;
        yield return new WaitForSeconds(1);
        cam.currentX = 360f;
        cam.currentY = 0f;
        cr_status[10] = false;
        yield break;
    }

    // End the dialogue. Make everything interactable again.
    // Hide the Help Dialogue Window
    public void EndDialogue() {
        slideButton.interactable = true;
        slideButtonColour.interactable = true;
        graphButton.interactable = true;
        helpButton.interactable = true;
        CSVbutton.interactable = true;
        patientDisplayButton.interactable = true;

        f_reset.interactable = true;
        f_filter.interactable = true;
        f_load.interactable = true;
        f_compare.interactable = true;

        animator.SetBool("IsOpen", false);
        sentenceIndex = 0;
    }
}
