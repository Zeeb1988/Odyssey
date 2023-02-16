using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    private CinemachineFreeLook followCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RegisterPlayer(CharacterStats player)
    { 
        playerStats = player;
        followCamera =FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(0);
            followCamera.LookAt = playerStats.transform.GetChild(0);
        }
    }

    public void AddObserver(IEndGameObserver observer)
    { 
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    { 
    
        endGameObservers.Remove(observer);
    }

    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers) 
        {
              observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform; 
        }
        return null;
    }
    
}
