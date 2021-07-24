using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Search query class which applyies the search paramerters set
 */

public class SearchQuery 
{
    //Search query parameters set to defaults
    private int fromAge;
    private int toAge;
    private List<string> genderSelected;
    private List<string> tumourType;
    private List<string> nodal;
    private List<string> site;
    private List<string> jawSide;
    private List<string> treatment;
    private List<string> totalRT;

    public SearchQuery()
    {
        this.fromAge = 0;
        this.toAge = 100;
        this.genderSelected = new List<string>();
        this.tumourType = new List<string>();
        this.nodal = new List<string>();
        this.site = new List<string>();
        this.jawSide = new List<string>();
        this.treatment = new List<string>();
        totalRT = new List<string>();
    }

    /**
     * filter the record patient passed against the filter values set
     * @param patient record - to check if it meets the search criteria.
     * @return true iff record matches search query. 
     */
    public bool filter(Patient record)
    {   
        if( ((record.GetCancerAge() >= fromAge) || (fromAge == -1)) &&
            ((record.GetCancerAge() <= toAge) || (toAge == -1)) &&
            ((CheckRecord(record.GetGender(), genderSelected) || CheckAll(genderSelected))) &&
            ((CheckRecord(record.GetTumourName(), tumourType) || CheckAll(tumourType))) &&
            ((CheckRecord(record.GetNodalPosition(), nodal) || CheckAll(nodal)) &&
            ((CheckRecord(record.GetSite(), site) || CheckAll(site))) &&
            ((CheckRecord(record.GetTumourJawSide(), jawSide) || CheckAll(jawSide))) &&
            ((CheckRecord(record.GetTreatment(), treatment) || CheckAll(treatment))) &&
            ((CheckRecord(record.GetTotalRT(), totalRT) || CheckAll(totalRT))))
        )
        {
            return true;
        }else{
            return false;
        }

    }

    /**
     * takes a filter list as a paramater
     * will return true if "All" or if empty list
     * else false will be returned 
     * @param filter - a list of filters selected
     * @return true if condition met
     */
    public bool CheckAll(List<string> filter)
    {
        if (filter.Count.Equals(0))
        {
            return true;
        }
        else if (filter[0] == "All")
        {
            return true;
        }else{
            return false;
        }
    }

    /**
     * compare if the lower case value of record exists in filter list
     * @param record - a string used for comparision
     * @param filter - a list of strings to compare against
     * @return true iff record exists in filter
     */
    public bool CheckRecord(string record, List<string> filter)
    {

        for (int i = 0; i < filter.Count; i++)
        {
            if (record.ToLower() == filter[i].ToLower())
            {
                return true;
            }
        }
        return false;
    }

    // Methods for setting filter options.
    public void SetGender(List<string> gender)
    {
        this.genderSelected = gender;
    }
    public void SetFromAge(int age)
    {
        this.fromAge = age;
    }
    public void SetToAge(int age)
    {
        this.toAge = age;
    }
    public void SetTumour(List<string> tumourType)
    {
        this.tumourType = tumourType;
    }
    public void SetNodal(List<string> nodal)
    {
        this.nodal = nodal;
    }
    public void SetSite(List<string> site)
    {
        this.site = site;
    }
    public void SetJawSide(List<string> jawSide)
    {
        this.jawSide = jawSide;
    }
    public void SetTreatment(List<string> treatment)
    {
        this.treatment = treatment;
    }
    public void SetTotalRT(List<string> totalRT)
    {
        this.totalRT = totalRT;
    }
}
