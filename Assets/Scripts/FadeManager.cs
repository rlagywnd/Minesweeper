using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Animator anime;
    public void FadeOut()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void FadeIn()
    {
        transform.parent.gameObject.SetActive(false);
    }
    public void PlayAnime(string name)
    {
        transform.parent.gameObject.SetActive(true);
        anime.SetTrigger(name);
    }
}
