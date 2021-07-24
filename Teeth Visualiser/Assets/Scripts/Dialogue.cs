using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This class is a wrapper 
 * around a list of strings for the dialogue system.
 * Each string represents a sentence of the dialouge
 */
[System.Serializable]
public class Dialogue
{

    public string[] sentences = {
            "Hello and welcome to this interactive tutorial! Please click 'Continue' ...",
            "During this tutorial you will learn how to interact with the application ...",
            "The purpose of the application is to help you visualize and discover patterns in radiation levels of different tooth cancer patients...",
            "Right now, you can see a model of a mouth. It contains all 36 teeth a human being normally has: 16 on the top, 16 on the bottom...",
            "As you can see, you can interact freely with the model. You can move it either by dragging the screen around\n" +
            " or by dragging the navigational cube in the bottom left corner...",
            "You can chose a different zoom setting by scrolling or by dragging the scroll bar at the bottom...",
            "If you touch a side of the navigation cube, it will snap your camera to that position...",
            "Great! You have mastered the skill of navigating your model!",
            "Now, let's take a look at displaying some information over our model...",
            "If you click on the button in the top left corner, a list with all the patients will pop up.",
            "Here you can select different filters for the patients you want to select.",
            "You also have a 'Select all option'...",
            "For now, let's just select all the patients and load them in...",
            "Great! Now that we have loaded in our patients, if we open the bottom right display, we will see some information about the patient set we have selected.",
            "If we open up the display situated on the right hand of the screen, we can select colours to display over our model.",
            "In this case, let's set the values from 20 to 40 to be displayed as a gradient from green to red...",
            "Fantastic! As you can see, our teeth have been coloured in accordingly!",
            "Now you can explore the model and see how the radiation levels affect different teeth!",
            "If you'd like to compare two different sets of patient records, you can do that from the menu from which you've select your patients in the first place.",
            "Also, if you'd like to graph your data, you can do that through the top menu.",
            "From the top, you can also select to import a new CSV file containing new patient records.",
            "That's it, now you're free to explore and discover! Please feel free to refer back to this tutorial whenever you feel the need to!",
            "Thank you for using our application!"

    };
}
