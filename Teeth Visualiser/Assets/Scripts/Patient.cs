using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The Patient class is used to store the data regarding
* the results of a patients radiation therapy.
*
*/
public class Patient
{
    private string tumour;
    private string nodal;
    private string gender;
    private int ageCancer;
    private string site;
    private string tumourJawSide;
    private string treatment;
    private string totalRT;

    //Each set of 8 teeth have a min, max and mean thus a multi-dimensional array
    private double[,] lowerLeft;
    private double[,] lowerRight;
    private double[,] upperLeft;
    private double[,] upperRight;

    //If this record has been selected for visulisation
    private bool selectedForVisual;

    public Patient(string tumour, string nodal, string gender, int ageCancer, string site, string side, string treatment, string totalRT, double[,] ll, double[,] lr, double[,] ul, double[,] ur)
    {
        this.tumour = tumour;
        this.nodal = nodal;
        this.gender = gender;
        this.ageCancer = ageCancer;
        this.site = SiteRemoveSpace(site);
        this.tumourJawSide = side ;
        this.treatment = treatment;
        this.totalRT = totalRT;
        this.lowerLeft = ll;
        this.lowerRight = lr;
        this.upperLeft = ul;
        this.upperRight = ur;
    }

    public string GetTumourName(){
        return tumour;
    }

    public string GetNodalPosition(){
        return nodal;
    }

    public string GetGender(){
        return gender;
    }

    public int GetCancerAge(){
        return ageCancer;
    }

    public string GetSite(){
        return site;
    }

    public string GetTumourJawSide(){
        return tumourJawSide;
    }

    public string GetTreatment(){
        return treatment;
    }

    public string GetTotalRT(){
        return totalRT;
    }

    public double [,] GetLowerLeft(){

        return lowerLeft;
    }

    public double [,] GetLowerRight(){
        return lowerRight;
    }

    public double [,] GetUpperLeft(){
        return upperLeft;
    }

    public double [,] GetUpperRight(){
        return upperRight;
    }

    public bool GetSelectedForVisual()
    {
        return selectedForVisual;
    }

    public void SetSelectedForVisual(bool selected)
    {
        this.selectedForVisual = selected;
    }

    private string SiteRemoveSpace(string site)
    {
        return site.Replace(" ", "");
    }
    //METHODS FOR CALCULATING VALUES ON THE PATIENT
}
