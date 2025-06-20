using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private static readonly string LoadScene = "Loding";
    private static string targetScene = "Title";

    public Image Image;

    private IEnumerator Start()
    {
        yield return Waits.GetWait(0.5f);
        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        for(float p=0; p <0.9f; p = op.progress)
        {
            Image.fillAmount = Mathf.Lerp(Image.fillAmount, p, 0.1f);
            yield return null;
        }

        for (float p = 0; p < 11; p +=Time.fixedDeltaTime)
        {
            Image.fillAmount = Mathf.Lerp(Image.fillAmount, 1, 0.1f);
            yield return null;
        }
        yield return Waits.GetWait(1.5f);
        op.allowSceneActivation = true;
    }

    public static void Load(string name)
    {
        targetScene = name;
        SceneManager.LoadScene(LoadScene);
    }


}
