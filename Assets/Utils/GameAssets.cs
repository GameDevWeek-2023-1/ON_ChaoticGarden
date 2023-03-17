using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    //In einem Resource Ordner dieses Script auf einem Prefabe
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if(_i == null)
            {
                _i = (Instantiate(Resources.Load("Game Assets")) as GameObject).GetComponent<GameAssets>();
            }
            return _i;
        }
    }
    //Hier alle Prefabes, welche das Object tragen soll

    public Transform seedGras;
    public Transform seedMushroom;
    public Transform seedBag;
}
