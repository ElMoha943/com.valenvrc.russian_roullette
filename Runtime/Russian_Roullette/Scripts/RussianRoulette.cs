using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RussianRoulette : UdonSharpBehaviour
{
    [SerializeField, Tooltip("1 in x chances of shooting")] int chances = 6;
    [SerializeField] AudioClip gunShot, emptyShot;
    [SerializeField, Tooltip("Should the player be teleported on death?")] bool shouldTeleport = true;
    [SerializeField] Transform TPPoint;

    void Start(){
        if(TPPoint == null){
            TPPoint = transform;
        }
    }

    public override void OnPickupUseDown()
    {
        if (Random.Range(0, chances) == 0){
            AudioSource.PlayClipAtPoint(gunShot, transform.position);
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Shot");
            if (shouldTeleport){
                Networking.LocalPlayer.TeleportTo(TPPoint.position, TPPoint.rotation);
            }
        }
        else{
            AudioSource.PlayClipAtPoint(emptyShot, transform.position);
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Empty");
        }
    }

    public void Shot(){
        AudioSource.PlayClipAtPoint(gunShot, transform.position);
    }

    public void Empty(){
        AudioSource.PlayClipAtPoint(emptyShot, transform.position);
    }


}
