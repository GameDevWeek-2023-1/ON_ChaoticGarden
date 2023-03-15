using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private float loadingCallbackTime;

    private bool _isFirstUpdate = true;
    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            StartCoroutine(LoaderCallBack());
        }
    }
    private IEnumerator LoaderCallBack()
    {
        yield return new WaitForSeconds(loadingCallbackTime);
        SceneLoaderController.LoaderCallBack();
    }
}
