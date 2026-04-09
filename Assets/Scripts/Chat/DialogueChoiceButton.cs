using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public Button button;

    Action onClicked;

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(() => onClicked?.Invoke());
    }

    public void Setup(string label, Action callback)
    {
        labelText.text = label;
        onClicked = callback;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}
