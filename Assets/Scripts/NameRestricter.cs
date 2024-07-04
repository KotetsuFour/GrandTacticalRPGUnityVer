using UnityEngine;
using TMPro;

public class NameRestricter : MonoBehaviour
{
    private TMP_InputField input;
    [SerializeField] private int minCharacters;
    [SerializeField] private int maxCharacters;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<TMP_InputField>();
    }

    public void validateInput()
    {
        if (input.text.Length > maxCharacters)
        {
            input.text = input.text.Substring(0, maxCharacters);
        }
    }

    public bool isValid()
    {
        return input.text.Length >= minCharacters && input.text.Length <= maxCharacters;
    }
}
