using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;

/**
* The TeethScriptAssigner class is used to assign each tooth object
* a TeethDisplayPanel script and MeshCollider so when hovered over,
* they display a radiation value.
*/
public class TeethScriptAssigner : ScriptableObject
{

    /**
    * Assigns the scripts to a set of teeth
    * @param selectedRecords the set of patients used to calculate the radiation values
    * @param firstSet is used to check which set of teeth, as there can be two (compare), to assign scripts to.
    */
    public void Begin(List<Patient> selectedRecords, bool firstSet)
    {
        double[] bottom = DataModel.CalculateAverageMeanRadiation(DataModel.CombineRightLeftTeethRadiation(selectedRecords, "bottom"));
        double[] top = DataModel.CalculateAverageMeanRadiation(DataModel.CombineRightLeftTeethRadiation(selectedRecords, "top"));

        GameObject bottomSet;
        GameObject topSet;
        Text text;

        if(firstSet) {
          bottomSet = GameObject.Find("BottomTeethSet");
          topSet = GameObject.Find("TopTeethSet");
          text = GameObject.Find("TeethValueA").GetComponent<Text>();
        } else {
          bottomSet = GameObject.Find("teethBottomSetB");
          topSet = GameObject.Find("teethTopSetB");
          text = GameObject.Find("TeethValueB").GetComponent<Text>();
        }

        int botCounter = 0;
        foreach(Transform t in bottomSet.transform)
        {
          t.gameObject.AddComponent<MeshCollider>();
          TeethDisplayPanel tdp = t.gameObject.AddComponent(typeof(TeethDisplayPanel)) as TeethDisplayPanel;
          tdp.Setup(text, bottom[botCounter]);
          botCounter++;
        }

        int topCounter = 0;
        foreach(Transform t in topSet.transform)
        {
          t.gameObject.AddComponent<MeshCollider>();
          TeethDisplayPanel tdp = t.gameObject.AddComponent(typeof(TeethDisplayPanel)) as TeethDisplayPanel;
          tdp.Setup(text, top[topCounter]);
          topCounter++;
        }
    }
}
