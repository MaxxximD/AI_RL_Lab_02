using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameEnvController : MonoBehaviour
{
    public int buttonsOnEpisode = 3;
    public int boxesOnEpisode = 3;

    private Agent agent;
    public GridedDistributor buttonsDistributor;
    public GridedDistributor boxDistributor;
    public GridedDistributor agentsDistributor;
    public Door door;
    public MeshCollider goal;
    bool IsOpenedBefore;

    void Start()
    {
        ResetScene();
    }

    void ResetScene()
    {
        var buttons = buttonsDistributor.Respawn(buttonsOnEpisode);
        boxDistributor.Respawn(boxesOnEpisode);

        var activators = new DoorActivator[buttons.Length];
        for (var i = 0; i < buttons.Length; i++)
            activators[i] = buttons[i].GetComponent<Button>();
        door.ResetActivators(activators);
        IsOpenedBefore = false;
        goal.gameObject.SetActive(false);

        agent = agentsDistributor.Respawn(1)[0].GetComponent<Agent>();
    }

    public void OnGoalTriggered()
    {
        agent.AddReward(5f);
        agent.EndEpisode();
        ResetScene();
    }

    public void ActivateDoor()
    {
        if (!IsOpenedBefore)
        {
            agent.AddReward(1f);
            IsOpenedBefore = true;
            goal.gameObject.SetActive(true);
        }
        else
        {
            agent.AddReward(-1f);
        }
        
    }

    void FixedUpdate()
    {
    }
}
