using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject skipMenu;
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] GameObject cover;
    [SerializeField] List<GameObject> pageList;
    public int indexPage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {

        }
    }

    public void CloseSkip()
    {
        skipMenu.SetActive(false);
    }
    public void OpenTutorial()
    {
        tutorialMenu.SetActive(true);
    }
    public void NextPage()
    {
        pageList[indexPage - 1].SetActive(false);
        if (indexPage < pageList.Count)
        {
            indexPage += 1;
        }
        else
        {
            indexPage = 1;
        }
        pageList[indexPage-1].SetActive(true);
    }
    public void PreviousPage()
    {
        pageList[indexPage - 1].SetActive(false);
        if (indexPage > 1)
        {
            indexPage -= 1;
        }
        else
        {
            indexPage = pageList.Count;
        }
        pageList[indexPage-1].SetActive(true);
    }

    public void LoadNextScene()
    {
        cover.SetActive(true);
        Invoke(nameof(GoNextScene),1f);
    }

    public void GoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
