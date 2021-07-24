/*
 * This class represents a helper class for the
 * DialogueManager class. 
 * 
 */
public class DialogueLogic 
{
    // Determine if Coroutines are running,
    // used to decide if the dialogue can progress
    public static bool IsRunningCoroutines(bool[] cr_status)
    {
        bool res = false;
        foreach (bool b in cr_status)
        {
            // If at least one coroutine running status is 'true'',
            // then res will also be true at the end by the laws of
            // propositional logic
            res = res || b;
        }
        return res;
    }
}
