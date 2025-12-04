using System;
using TMPro;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int collected = 0;
    public TextMeshProUGUI collectedText;
    public GameObject collectedParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            collected++;
            collectedText.text = collected.ToString();
            var particleObj =  Instantiate(collectedParticle, other.transform.position, Quaternion.identity);
            var particle = particleObj.GetComponent<ParticleSystem>();
            Destroy(particleObj,particle.main.duration);
            Destroy(other.gameObject);
        }
    }
}
