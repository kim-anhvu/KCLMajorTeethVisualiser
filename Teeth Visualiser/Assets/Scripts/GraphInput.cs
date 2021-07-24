using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

/**
 * Static class that stores graph filter data between scene transitions
 * Part of Scene Management
 */
public static class GraphInput
{
    public static int fromAge;
    public static int toAge;
    public static List<string> genderSelected;
    public static List<string> tumourSelected;
    public static List<string> nodalSelected;
    public static List<string> siteSelected;
    public static List<string> jawSideSelected;
    public static List<string> treatmentSelected;
    public static List<string> totalRTSelected;
    public static List<string> toothTypeSelected;
    public static int dataTypeSelected;
    public static string xVariableSelected;
    public static int ageRange = 1;


    public static void SetFromAge(int age) { fromAge = age; }
    public static void SetToAge(int age) { toAge = age; }
    public static void SetGender(List<string> gender) { genderSelected = gender; }
    public static void SetTumour(List<string> tumour) { tumourSelected = tumour; }
    public static void SetNodal(List<string> nodal) { nodalSelected = nodal; }
    public static void SetSite(List<string> site) { siteSelected = site; }
    public static void SetJawSide(List<string> jawSide) { jawSideSelected = jawSide; }
    public static void SetTreatment(List<string> treatment) { treatmentSelected = treatment; }
    public static void SetTotalRT(List<string> totalRT) { totalRTSelected = totalRT; }
    public static void SetToothType(List<string> toothType) { toothTypeSelected = toothType; }
    public static void SetDataType(int dataType) { dataTypeSelected = dataType; }
    public static void SetXVariable(string xVariable) { xVariableSelected = xVariable; }
    public static void SetAgeRange(int age) { ageRange = age; }
    
}
