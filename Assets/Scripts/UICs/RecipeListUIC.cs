using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RecipeListUIC : MonoBehaviour
{
    #region Singleton
    public static RecipeListUIC instance;

    private void Awake()
    {
        itemPrototype = Resources.Load("Prefabs/RecipeListItem") as GameObject;
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

    public GameObject contentPanel;
    public TMP_InputField searchField;

    private List<GameObject> listItems = new List<GameObject>();

    private GameObject itemPrototype;

    public void FilterBySearchInput()
    {
        string searchInput = searchField.text;
        Debug.Log("searchinput:" + searchInput);
        Debug.Log("Search changed");
        if (searchInput == "")
        {
            Debug.Log("clearing");
            foreach (GameObject item in listItems)
            {
                item.SetActive(true);
            }
            return;
        }

        searchInput = searchInput.ToLower();
        foreach (GameObject item in listItems)
        {
            Debug.Log("checking item");
            string recipeName =
                item.GetComponent<RecipeListItemUIC>().getRecipeName().ToLower();
            item.SetActive(recipeName.Contains(searchInput));
        }
    }

    public void AddRecipeItem(Recipe recipe, int id)
    {
        // create new List Item
        GameObject newItem = Instantiate(itemPrototype);

        // set parent transform to put item into the listview
        newItem.transform.SetParent(contentPanel.transform, false);

        // get content component to set the actual content of the list item
        RecipeListItemUIC itemContent = newItem.GetComponent<RecipeListItemUIC>();

        string lastPrepared = recipe.lastPrepared == AppController.neverMade ?
                "--.--.----" : recipe.GetDate();

        // set content of the item according to passed recipe parameters
        itemContent.SetupListItem(recipe.name, lastPrepared, id);

        // store the item in a list for later access
        listItems.Add(newItem);
    }

    public void DeleteSyncMessage(string messageID)
    {
        GameObject itemToBeRemoved = null;
        foreach (GameObject g in listItems)
        {
            //if (g.GetComponent<SyncMessageListItemUIC>().messageID == messageID)
            //{
            //    itemToBeRemoved = g;
            //}
        }

        if (itemToBeRemoved != null)
        {
            // remove item from interal storage list
            listItems.Remove(itemToBeRemoved);
            Destroy(itemToBeRemoved);
        }
    }

    public void RerenderList(List<Recipe> recipes)
    {
        // clear list
        for (int i = listItems.Count - 1; i >= 0; i--)
        {
            GameObject g = listItems[i];
            listItems.RemoveAt(i);
            Destroy(g);
        }

        // reinstantiate list items
        for (int i = 0; i < recipes.Count; i++)
        {
            AddRecipeItem(recipes[i], i);
        }

        FilterBySearchInput();
    }
}
