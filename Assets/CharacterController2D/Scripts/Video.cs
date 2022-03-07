using UnityEngine;

namespace AGGE.CharacterController2D {
    public class Video : MonoBehaviour {
        public GameObject curtain;
        /*
        private MovieTexture movieTexture;

        void Update () 
        {
            if(!movieTexture)
            {
                Renderer renderer = GetComponent<Renderer>();			
                movieTexture = renderer.material.mainTexture as MovieTexture;
                movieTexture.loop = true;
            }

            if(movieTexture.isReadyToPlay && !movieTexture.isPlaying)
            {
                curtain.SetActive(false);
                movieTexture.Play();
            }
        }
        //*/
    }
}