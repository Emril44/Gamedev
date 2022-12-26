[System.Serializable]
public class DataManagerSerializedData
{
    public int timePlayed = 0;
    public int day = 0;
    public int sparksAmount;
    public int unlockedColors;
    public int[] touchedLetters;

    public DataManagerSerializedData(int sparksAmount, int unlockedColors, int timePlayed, int day, int[] touchedLetters)
    {
        this.sparksAmount = sparksAmount;
        this.unlockedColors = unlockedColors;
        this.timePlayed = timePlayed;
        this.day = day;
        this.touchedLetters = touchedLetters;
    }
}
