using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f;
       GetComponent<Button>().onClick.AddListener(() =>
       {
           gameObject.SetActive(false);
           Time.timeScale = 1.0f;
       });
    }
}
