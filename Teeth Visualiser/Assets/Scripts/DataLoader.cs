using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Loads in the Patient Data
 */
public static class DataLoader
{
    private static List<Patient> patients;

    // Start is called before the first frame update
    static DataLoader(){
        CSVParser csvParser = new CSVParser();
        csvParser.Startup();
        patients = csvParser.GetPatients();
    }

    public static void LoadNewCSV()
    {
        //patients = new CSVParser(updated file location)
    }

    public static List<Patient> GetAllPatients()
    {
        return patients;
    }

    public static void AppendPatients(List<Patient> newPatients)
    {
        patients.AddRange(newPatients);
    }
}
