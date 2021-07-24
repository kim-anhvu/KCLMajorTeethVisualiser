using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A Mock dataset to be used in the Testing
 * A list of patients and methods which create specific types of patients
 * Used to Improve the testing process and remove dependecies on loading data from external sources
 */

public class MockDataLoader 
{
    private double defaultRad = 50;
    List<Patient> patients;
    List<Patient> singlePatient;
    List<Patient> multiplePatients;

    public MockDataLoader()
    {
        patients = new List<Patient>();
        CreateMockData();
        singlePatient = new List<Patient>();
        multiplePatients = new List<Patient>();
        AddSinglePatient();
        AddMultiplePatients();

    }

    public List<Patient> GetAllMockData()
    {
        return patients;
    }

    public List<Patient> GetSinglePatientList()
    {
        return singlePatient;
    }

    public List<Patient> GetMultiplePatientsList()
    {
        return multiplePatients;
    }

    private void CreateMockData()
    {
        
        patients.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(32.4391212), CreateEightTeethRadiationValues(32.34829469), CreateEightTeethRadiationValues(42.97899229), CreateEightTeethRadiationValues(39.22992299)));
        patients.Add(CreatePatient("T1", "N0", "F", 50, "TONSIL", "L", "RT", "65/30", CreateEightTeethRadiationValues(28.584928593759), CreateEightTeethRadiationValues(29.5839285920584), CreateEightTeethRadiationValues(29.12477891388), CreateEightTeethRadiationValues(27.9567595976095)));
        patients.Add(CreatePatient("T1", "N1", "F", 73, "BOT", "L", "CRT", "65/30", CreateEightTeethRadiationValues(49.85922883), CreateEightTeethRadiationValues(47.5820493), CreateEightTeethRadiationValues(34.14745622), CreateEightTeethRadiationValues(32.25308088)));
        patients.Add(CreatePatient("T1", "N1", "M", 49, "TONSIL", "R", "RT", "65/30", CreateEightTeethRadiationValues(33.483945839), CreateEightTeethRadiationValues(32.84829547), CreateEightTeethRadiationValues(20.7483949), CreateEightTeethRadiationValues(23.15829354)));
        patients.Add(CreatePatient("T2", "N0", "F", 72, "BOT", "N/A", "RT", "65/30", CreateEightTeethRadiationValues(44.5843902574), CreateEightTeethRadiationValues(45.543802475), CreateEightTeethRadiationValues(35.454380234733), CreateEightTeethRadiationValues(28.73459834214)));
        patients.Add(CreatePatient("T2", "N1", "M", 55, "OP - OTHER", "L", "CRT", "65/30", CreateEightTeethRadiationValues(28.6858295583824), CreateEightTeethRadiationValues(28.4584394206204), CreateEightTeethRadiationValues(31.3054389520582), CreateEightTeethRadiationValues(29.511582058775)));
        patients.Add(CreatePatient("T2", "N2", "M", 51, "BOT", "R", "CRT", "65/30", CreateEightTeethRadiationValues(35.27058212), CreateEightTeethRadiationValues(56.59039853), CreateEightTeethRadiationValues(27.11768613), CreateEightTeethRadiationValues(27.8299627)));
        patients.Add(CreatePatient("T2", "N2", "M", 55, "TONSIL", "L", "RT", "65/30", CreateEightTeethRadiationValues(35.27058212), CreateEightTeethRadiationValues(35.27058212), CreateEightTeethRadiationValues(35.27058212), CreateEightTeethRadiationValues(35.27058212)));
        patients.Add(CreatePatient("T2", "N3", "F", 49, "OP - OTHER", "R", "CRT", "55/20", CreateEightTeethRadiationValues(35.27058212), CreateEightTeethRadiationValues(55.02381147), CreateEightTeethRadiationValues(44.098278), CreateEightTeethRadiationValues(64.89546))); 
        patients.Add(CreatePatient("T3", "N0", "M", 42, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(39.895302389), CreateEightTeethRadiationValues(47.48928105), CreateEightTeethRadiationValues(35.1024978), CreateEightTeethRadiationValues(34.392294))); 
        patients.Add(CreatePatient("T3", "N1", "F", 54, "TONSIL", "L", "CRT", "65/30", CreateEightTeethRadiationValues(23.567519), CreateEightTeethRadiationValues(26.48570275), CreateEightTeethRadiationValues(19.25893481), CreateEightTeethRadiationValues(19.46938523))); 
        patients.Add(CreatePatient("T3", "N1", "F", 86, "BOT", "R", "RT", "65/30", CreateEightTeethRadiationValues(53.468128021), CreateEightTeethRadiationValues(53.16817233), CreateEightTeethRadiationValues(44.94723991), CreateEightTeethRadiationValues(46.46692743))); 
        patients.Add(CreatePatient("T2", "N2", "M", 61, "TONSIL", "L", "CRT", "54/30", CreateEightTeethRadiationValues(50.9995745), CreateEightTeethRadiationValues(47.9952211), CreateEightTeethRadiationValues(34.82382822), CreateEightTeethRadiationValues(32.21504296)));
        patients.Add(CreatePatient("T3", "N2", "M", 62, "BOT", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(43.44389218), CreateEightTeethRadiationValues(43.25428011), CreateEightTeethRadiationValues(47.0002539), CreateEightTeethRadiationValues(45.93840299)));
        patients.Add(CreatePatient("T3", "N3", "M", 81, "OP - OTHER", "L", "RT", "65/30", CreateEightTeethRadiationValues(28.7592036), CreateEightTeethRadiationValues(27.01492014), CreateEightTeethRadiationValues(18.88199898), CreateEightTeethRadiationValues(21.4548925)));
        patients.Add(CreatePatient("T4", "N0", "F", 68, "TONSIL", "R", "CRT", "65/30", CreateEightTeethRadiationValues(48.17922851), CreateEightTeethRadiationValues(50.26806622), CreateEightTeethRadiationValues(49.68372952), CreateEightTeethRadiationValues(49.95830281)));
        patients.Add(CreatePatient("T4", "N1", "M", 58, "BOT", "R", "CRT", "65/30", CreateEightTeethRadiationValues(40.58294728), CreateEightTeethRadiationValues(41.19927885), CreateEightTeethRadiationValues(22.58395811), CreateEightTeethRadiationValues(18.32068202)));
        patients.Add(CreatePatient("T4", "N2", "F", 43, "BOT", "L", "CRT", "65/30", CreateEightTeethRadiationValues(38.95151483), CreateEightTeethRadiationValues(42.3157578), CreateEightTeethRadiationValues(31.32015772), CreateEightTeethRadiationValues(29.12320088)));
        patients.Add(CreatePatient("T4", "N2", "M", 66, "TONSIL", "L", "CRT", "70/35", CreateEightTeethRadiationValues(44.0005294282581), CreateEightTeethRadiationValues(46.5927461046254), CreateEightTeethRadiationValues(35.9423), CreateEightTeethRadiationValues(34.8975795021278)));
        patients.Add(CreatePatient("T4", "N3", "F", 59, "TONSIL", "R", "CRT", "65/30", CreateEightTeethRadiationValues(44.29572685), CreateEightTeethRadiationValues(46.47293423), CreateEightTeethRadiationValues(44.58273937), CreateEightTeethRadiationValues(44.0593048)));
        
    }

    //Creates radiation values for eight teeth which have the same radiation value, for min,max,mean
    private double[,] CreateEightTeethRadiationValues(double value){
        double[,] eightTeeth = new  double[8,3];
        for (int i = 0; i < 8; i++)
        {   
            eightTeeth[i,0] = value;
            eightTeeth[i,1] = value;
            eightTeeth[i,2] = value;
        }
        return eightTeeth;
    }

    /*
        Takes as parameters all the Patients values needed for constructor.
     */
    private Patient CreatePatient(string t, string n, string g, int a, string site, string side, string treatment, string tRT,
                                double[,] ll, double[,] lr, double[,] ul, double[,] ur)
    {
        return new Patient(t,n,g,a,site,side,treatment,tRT,ll,lr,ul,ur);
    }

    //Creates a simple patient list with all teeth having the same radiation.
    public List<Patient> CreateSimpleRadiationMockData(double radiation)
    {
        List<Patient> simplePatients = new List<Patient>();
        simplePatients.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation)));
        simplePatients.Add(CreatePatient("T1", "N0", "F", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation)));
        simplePatients.Add(CreatePatient("T1", "N1", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation)));
        simplePatients.Add(CreatePatient("T1", "N2", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation), CreateEightTeethRadiationValues(radiation)));

        return simplePatients;
    }

    //Creates a simple patient list with all Ages being different having the same radiation.
    public List<Patient> CreateSimpleAgeMockData(List<int> ages)
    {
        List<Patient> simplePatients = new List<Patient>();
        foreach (int age in ages)
        {
            patients.Add(CreatePatient("T1", "N0", "M", age, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(defaultRad), CreateEightTeethRadiationValues(defaultRad), CreateEightTeethRadiationValues(defaultRad), CreateEightTeethRadiationValues(defaultRad)));
   
        }
        return simplePatients;
    }

    private void AddSinglePatient()
    {
        singlePatient.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues()));
    }

    private void AddMultiplePatients()
    {
        multiplePatients.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues()));
        multiplePatients.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues()));
        multiplePatients.Add(CreatePatient("T1", "N0", "M", 87, "OP - OTHER", "N/A", "CRT", "65/30", CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues(), CreateEightTeethRadiationValues()));
    }
    
    //Creates 8 Teeth with numbered Radiational values
    private double[,] CreateEightTeethRadiationValues()
    {
        double[,] eightTeeth = new double[8, 3];
        for (int i = 0; i < 8; i++)
        {
            eightTeeth[i, 0] = i + 1;
            eightTeeth[i, 1] = i + 1;
            eightTeeth[i, 2] = i + 1;
        }
        return eightTeeth;
    }

}
