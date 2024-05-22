using UnityEngine;
using UnityEngine.UI;

public class TogglePasswordVisibility : MonoBehaviour
{
    public InputField passwordInputField;
    public Button toggleButton;
    public Sprite passwordHiddenSprite; //eye represent password hidden
    public Sprite passwordVisibleSprite; //Eye represent password visible 

    private bool isPasswordVisible = false;

    void Start()
    {
        toggleButton.onClick.AddListener(TogglePasswordVisibilityOnClick);
    }

    void TogglePasswordVisibilityOnClick()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            passwordInputField.contentType = InputField.ContentType.Standard;
            //change image when you want to show the password
            toggleButton.image.sprite = passwordVisibleSprite;
        }
        else
        {
            passwordInputField.contentType = InputField.ContentType.Password;
            //change image when you want to hide the password
            toggleButton.image.sprite = passwordHiddenSprite;
        }

        //change the inputvisibilty of the selected label
        passwordInputField.ForceLabelUpdate();
    }
}
