using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewRecipeDialogUIC : MonoBehaviour
{
    #region Singleton
    public static ViewRecipeDialogUIC instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    #endregion

    public TextMeshProUGUI recipeTextMesh;

    public void OnCookRecipeButtonClicked()
    {
        AppController.instance.OnCookRecipeConfirmed();
    }

    public void OnChangeRecipeButtonClicked()
    {

    }

    public void OnDeleteRecipeButtonClicked()
    {
        AppController.instance.OnDeleteRecipeConfirmed();
    }

    public void OnCloseDialogButtonClicked()
    {
        AppController.instance.OnShowRecipeDialogClosed();
    }

    public void ChangeRecipeName(string newRecipeName)
    {
        recipeTextMesh.text = $"\"{newRecipeName}\"";
    }
}
