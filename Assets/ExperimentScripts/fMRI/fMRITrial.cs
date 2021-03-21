[System.Serializable]
public class fMRITrial : Trial
{
    public int trial_id;
    public string trial_type;
    public string trial_code;
    public int SM_velocity;
    public int OM_velocity;
    public string Condition_name;
    public float onset;
    public int duration;
    public float ITI_duration;

    public float StartTime = 0;
    public float ChangeTime = 0;
    public float EndTime;

    // Define the header of the trial list file here
    private static readonly string[] trialListHeader = { "trial_id", "trial_type", "trial_code", "SM_velocity", "OM_velocity", "Condition_name", "onset", "duration", "ITI_duration"};
    override public string[] TrialListHeader => trialListHeader;

    // Define the header of the results file here
    private static readonly string[] resultsFileHeader = { "trial_id", "trial_type", "trial_code", "SM_velocity", "OM_velocity", "Condition_name", "onset", "duration", "ITI_duration", "StartTime", "ChangeTime" };
    override protected string[] TrialSpecificResultsHeader => resultsFileHeader;

    // Read takes an array of strings read from a row of the trial list and
    // parses it into values of trial properties
    override public void Read(string[] values)
    {
        trial_id = int.Parse(values[0]);
        trial_type = values[1];
        trial_code = values[2];
        SM_velocity = int.Parse(values[3]);
        OM_velocity = int.Parse(values[4]);
        Condition_name = values[5];
        onset = float.Parse(values[6]);
        duration = int.Parse(values[7]);
        ITI_duration = float.Parse(values[8]);
    }

    // Save must returns an array of values to be saved to the results file, one
    // for each column of the results file as defined in ResultsFileHeader
    override protected object[] Save()
    {
        ResponseTime = Response != 0 ? (ResponseTime - ChangeTime) * 1000 : 0;
        return new object[] { "trial_id", "trial_type", "trial_code", "SM_velocity", "OM_velocity", "Condition_name", "onset", "duration", "ITI_duration", StartTime, ChangeTime };
    }
}