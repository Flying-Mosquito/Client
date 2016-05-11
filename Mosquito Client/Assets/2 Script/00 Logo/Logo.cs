using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class Logo : MonoBehaviour {
    public Image Image_Star;
    public Image Image_StarSailor;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        Image_Star.DOFade(0.3f, 0.1f);// DOFade(1, 5.0f);
        yield return new WaitForSeconds(0.1f);
        Image_Star.DOFade(1.0f, 0.1f);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fill_Image(Image_StarSailor, 0.5f));
        yield return new WaitForSeconds(1.5f);

        Image_Star.DOFade(0.0f, 2.5f);
        Image_StarSailor.DOFade(0.0f, 2.5f);

        //  StartCoroutine("ChangeScene");
        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.StartCoroutine("StartLoad", "01 MainMenu");

    }

   /* private IEnumerator ChangeScene()
    { 
        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.GetComponent<Loading>().StartCoroutine("StartLoad", "01 MainMenu");

    }*/

    IEnumerator Fill_Image(Image _image, float _fTime)
    {
        float fnum = 1 / _fTime;

        while (_image.fillAmount < 1)
        {
            _image.fillAmount += (fnum * Time.deltaTime);
            yield return null;
        }

       
       
    }

}
