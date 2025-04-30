using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopups : MonoBehaviour
{
    [SerializeField] private Button Next_1;
    [SerializeField] private Button Next_2;
    [SerializeField] private Button Next_3;
    [SerializeField] private Button Next_4;

    [SerializeField] private Button Previous_2;
    [SerializeField] private Button Previous_3;
    [SerializeField] private Button Previous_4;
    [SerializeField] private Button Previous_5;

    [SerializeField] private Transform[] PageArray = new Transform[5];
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button InfoButton;
    [SerializeField] private ToggleUI ToggleScript;
    [SerializeField] private Transform Panel;

    private int CurrentPage;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPage = 1;

        Next_1.onClick.AddListener(() => { GoToPage(2); });
        Next_2.onClick.AddListener(() => { GoToPage(3); });
        Next_3.onClick.AddListener(() => { GoToPage(4); });
        Next_4.onClick.AddListener(() => { GoToPage(5); });

        Previous_2.onClick.AddListener(() => { GoToPage(1); });
        Previous_3.onClick.AddListener(() => { GoToPage(2); });
        Previous_4.onClick.AddListener(() => { GoToPage(3); });
        Previous_5.onClick.AddListener(() => { GoToPage(4); });

        CloseButton.onClick.AddListener(() => {  Close(); });
    }

    void GoToPage(int page)
    {
        Hide(PageArray[CurrentPage - 1]);
        Show(PageArray[page - 1]);
        CurrentPage = page;
    }

    public void InfoButtonPressed()
    {
        ToggleScript.ToggleUIComponent();
        InfoButton.interactable = false;
        Show(Panel);
        Show(PageArray[0]);
        for(int i = 1; i < PageArray.Length; i++)
        {
            Hide(PageArray[i]);
        }
    }

    void Show(Transform page)
    {
        page.gameObject.SetActive(true);
    }

    void Hide(Transform page)
    {
        page.gameObject.SetActive(false);
    }

    void SetInfoButtonInteractive()
    {
        InfoButton.interactable = !InfoButton.interactable;
    }

    public void Close()
    {
        CurrentPage = 1;
        ToggleScript.ToggleUIComponent();
        Hide(Panel);
        Invoke("SetInfoButtonInteractive", 0.3f);
    }
}
