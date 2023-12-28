using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemButton : MonoBehaviour
{
    [SerializeField] private Button button;
    protected void Start()
    {
        this.button.onClick.AddListener(TaskOnClick);
    }
    public abstract void TaskOnClick();
}
