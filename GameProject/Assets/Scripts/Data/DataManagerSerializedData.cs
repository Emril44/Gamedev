[System.Serializable]
public class DataManagerSerializedData
{
    public int timePlayed = 0;
    public int day = 0;
    public int sparksAmount;
    public int unlockedColors;

    public DataManagerSerializedData(int sparksAmount, int unlockedColors, int timePlayed, int day)
    {
        this.sparksAmount = sparksAmount;
        this.unlockedColors = unlockedColors;
        this.timePlayed = timePlayed;
        this.day = day;
    }
}
