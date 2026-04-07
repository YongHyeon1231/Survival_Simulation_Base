using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public static Particle_Handler instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void OnParticle(MeshRenderer meshRenderer)
    {
        transform.position = meshRenderer.transform.position;
        UpdateParticleMesh(meshRenderer);
        particleSystem.Play();
    }

    // 해당 부분은 필요한 것을 찾아서 직접 작업 base 없음
    private void UpdateParticleMesh(MeshRenderer meshRenderer)
    {
        var shape = particleSystem.shape;
        shape.meshRenderer = meshRenderer;
    }
}
