using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookRecipeDialogUIC : MonoBehaviour
{
    #region Singleton
    public static CookRecipeDialogUIC instance;

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

    public TextMeshProUGUI dialogTitle;

    public void OnDialogConfirmedButtonClicked()
    {
        AppController.instance.OnCookRecipeConfirmed();
    }

    public void OnDialogCanceledButtonClicked()
    {
        AppController.instance.OnCookRecipeCanceled();
    }

    public void SetDialogTitle(string newTitle)
    {
        dialogTitle.text = newTitle;
    }
}
