using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ChangeRecipeDialogUIC : MonoBehaviour
{
    #region Singleton
    public static ChangeRecipeDialogUIC instance;

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

    public TMP_InputField recipeNameInput;
    public TMP_InputField lastPreparedInput;

    public Button saveButton;

    public string GetRecipeName()
    {
        return recipeNameInput.text;
    }

    public DateTime GetLastPrepared()
    {
        return DateTime.ParseExact(
            lastPreparedInput.text,
            new string[] { "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.M.yyyy" },
            null,
            System.Globalization.DateTimeStyles.None);
    }

    public void SetupDialog(Recipe recipe)
    {
        recipeNameInput.text = recipe.name;
        lastPreparedInput.text = recipe.GetDate();
    }

    public void OnDialogSaveButtonClicked()
    {
        AppController.instance.OnChangeRecipeDialogSave();
    }

    public void OnDialogCancelButtonClicked()
    {
        AppController.instance.OnChangeRecipeDialogCancel();
    }

    public void OnDateInputChanged()
    {
        string dateInput = lastPreparedInput.text;
        DateTime newDate;
        if (DateTime.TryParseExact(
            dateInput,
            new string[] {"dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.M.yyyy"},
            null,
            System.Globalization.DateTimeStyles.None,
            out newDate))
        {
            saveButton.interactable = true;
        }
        else
        {
            saveButton.interactable = false;
        }
    }
}
