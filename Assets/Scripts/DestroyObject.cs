using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public Target target;
    [SerializeField] private AudioClip victorySound;
    public Win winScript;


    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target.IsDead())
        {
            SoundFXManager.instance.PlaySoundFXClip(victorySound, transform, 1f);
            winScript.WonGame();
            Destroy(gameObject);
            
        }
    }
}
