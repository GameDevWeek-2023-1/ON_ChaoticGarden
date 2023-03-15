/*
 
    ZUM TRIGGERN
    FunctionTimer.Create(TestingAction, 2f);
    FunctionTimer.Create(TestingAction2, 3f, "Testing2");
    
    ZUM LÖSCHEN
    FunctionTimer.StopTimer("Testing2"); 

 */
/*
 
    DER TIMER UPDATER, WELCHER EINE FUNKTION SOLANGE AUFRUFT, BIS NICHT MEHR TRUE
    private void SpawnEffect(float time, float waitTime, Transform target)
    {
        float backUpTime = waitTime;
        FunctionTimer.Create(() =>
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                Instantiate(effect, target.position, Quaternion.identity);
                waitTime = backUpTime;
            }
            else
            {
                time -= Time.deltaTime;
            }
            return time <= 0f;
        }, "", true);
    }
    
    ALLES IN {} WIRD AUFGERUFEN, SOLANGE time NICHT KLEINER ODER GLEICH NULL IST UND SOMIT TRUE ZURÜCK GIBT.
    SOMIT EINFACH ALLES IN DEN CODEBLOCK ZWISCHEN DEN KLAMMERN

*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer
{
    //EIne Dummy Klasse, damit wir unsere FunctionTimer Script das mit dem Aufruf nach Zeit überlassen können
    public class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            //Macht den Call jeden Frame
            if (onUpdate != null) onUpdate();
        }
    }

    //Hält track über alle Timer die gerade laufen
    public static List<FunctionTimer> activeTimerList;
    //Unser Timer GameObject, damit wir track über alles haben können wenn wir wollen z.B. einen Timer wieder abbrechen, vor der Endzeit
    private static GameObject callIfNeededGameObject;
    private static void CallIfNeeded()
    {
        if(callIfNeededGameObject == null)
        {
            callIfNeededGameObject = new GameObject("FunctionTimer_CallIfNeeded_GameObject");
            activeTimerList = new List<FunctionTimer>();
        }
    }

    //Für Funtkionen welche immer wieder aufgerufen werden sollen
    public static FunctionTimer Create(Func<bool> actionFunc, string functionName, bool active)
    {
        CallIfNeeded();

        GameObject gameObject = new GameObject("FunctionTimer " + functionName, typeof(MonoBehaviourHook));
        FunctionTimer functionUpdater = new FunctionTimer(actionFunc, gameObject, functionName, active);
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionUpdater.Update;

        activeTimerList.Add(functionUpdater);
        return functionUpdater;
    }

    //Called after Time
    public static FunctionTimer Create(Action action, float timer, string timerName = null)
    {
        //Damit wir ein Object haben zum "steuern"
        CallIfNeeded();

        //Dann erstellen wir ein GameObject, dass MonoBehaviourHook zu bekommt
        GameObject gameObject = new GameObject("FuntionTimer", typeof(MonoBehaviourHook));

        //Zuerst erstellen wir unser FunctionTimer als Instance
        FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timerName, false);

        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.UpdateTimer;

        activeTimerList.Add(functionTimer);

        return functionTimer;
    }
    public static bool HasTimer(string name)
    {
        for (int i = 0; i < activeTimerList.Count; i++)
        {
            if (activeTimerList[i].timerName == name)
            {
                return true;
            }
        }
        return false;
    }
    private static void RemoveTimer(FunctionTimer functionTimer)
    {
        CallIfNeeded();
        activeTimerList.Remove(functionTimer);
    }
    public static void StopTimer(string timerName)
    {
        for (int i = 0; i < activeTimerList.Count; i++)
        {
            if(activeTimerList[i].timerName == timerName)
            {
                activeTimerList[i].DestroySelf();
                i--;
            }
        }
    }

    private Action action;
    private Func<bool> actionFunc;
    private float timer;
    private GameObject gameObject;
    //Wir können optional einen Namen als string dazu geben, wenn wir dies tun, können wir so einen Timer wieder löschen, in dem wir StopTimer(mit dem Namen) aufrufen
    private string timerName;
    private bool isDestroyed;
    private bool active;

    private FunctionTimer(Action action, float timer, GameObject gameObject, string timerName, bool active)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        this.timerName = timerName;
        this.active = active;
        isDestroyed = false;
    }
    private FunctionTimer(Func<bool> actionFunc, GameObject gameObject, string timerName, bool active)
    {
        this.actionFunc = actionFunc;
        this.gameObject = gameObject;
        this.timerName = timerName;
        this.active = active;
        isDestroyed = false;
    }

    //Wird über MonoBehaviourHook jeden Frame aufgerufen, bis action getriggert wird
    public void UpdateTimer()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                action();
                DestroySelf();
            }
        }
    }
    private void Update()
    {
        if (!active) return;
        if (actionFunc())
        {
            DestroySelf();
        }
    }
    private void DestroySelf()
    {
        isDestroyed = true;
        //Damit wir unser erstelltes Timer GameObject wirder zerstören können, nach gebrauch
        UnityEngine.Object.Destroy(gameObject);
        RemoveTimer(this);
    }
}
