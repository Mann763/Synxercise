using System.Collections;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void quitgame()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(quiting());
    }

    IEnumerator quiting()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
