using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;


public class UnitsSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpownPoint = null;
    [SerializeField] private Health health = null;

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }
    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(
            unitPrefab,
            unitSpownPoint.position,
            unitSpownPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion



    #region Client 
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (!this.hasAuthority) { return; }
        CmdSpawnUnit();
    }
    #endregion
}
