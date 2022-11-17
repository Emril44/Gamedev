using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private char separator = '|';
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            Debug.LogWarning("Language not set");
            PlayerPrefs.SetInt("Language", 0);
        }
        textMesh.text = Pick();
    }
    
    private string Pick()
    {
        string[] variations = text.Split(separator);
        int n = PlayerPrefs.GetInt("Language");
        if (n >= variations.Length)
        {
            Debug.LogError("Language index out of range (" + n + "/" + variations.Length + ") for the text type of \"" + variations[0] + "\"");
            return variations[0];
        }
        else
        {
            return variations[n];
        }
    }
}
