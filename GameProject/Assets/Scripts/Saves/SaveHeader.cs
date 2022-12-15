// lightweight save representation for player's easier navigation
public class SaveHeader
{
    // time in seconds
    public int timePlayed;
    public int day;
    public int sparks;

    public SaveHeader(DataManagerSerializedData data)
    {
        this.timePlayed = data.timePlayed;
        this.day = data.day;
        this.sparks = data.sparksAmount;
    }
}
