using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.RussianRoulette{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RussianRoulette : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("1 in x chances of shooting")] int chances = 6;
        [SerializeField] AudioClip gunShot, emptyShot;
        [SerializeField, Tooltip("Should the player be teleported on death?")] bool shouldTeleport = true;
        [SerializeField] Transform TPPoint;

        AudioSource audioSource;
        VRCPlayerApi localPlayer;

        float maxAudioDistance;

        void Start(){
            audioSource = GetComponent<AudioSource>();
            if(TPPoint == null){
                TPPoint = transform;
            }
            localPlayer = Networking.LocalPlayer;
            maxAudioDistance = audioSource.maxDistance;
        }

        public override void OnPickupUseDown(){
            if (Random.Range(0, chances) == 0){
                Shot();
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Shot");
                if(shouldTeleport){
                    Networking.LocalPlayer.TeleportTo(TPPoint.position, TPPoint.rotation);
                }
            }
            else{
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Empty");
                Empty();
            }
        }

        public void Shot(){
            float playerDistance = Vector3.Distance(localPlayer.GetPosition(), gameObject.transform.position);
            if (playerDistance < maxAudioDistance){
                audioSource.PlayOneShot(gunShot);
            }
        }

        public void Empty(){
            float playerDistance = Vector3.Distance(localPlayer.GetPosition(), gameObject.transform.position);
            if (playerDistance < maxAudioDistance){
                audioSource.PlayOneShot(emptyShot);
            }
        }
    }
}
