using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public static Particle_Handler instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    ParticleSystem m_Particle;

    private void Start()
    {
        m_Particle = GetComponent<ParticleSystem>();
    }

    public void OnParticle(MeshRenderer meshRenderer)
    {
        transform.position = meshRenderer.transform.position;
        UpdateParticleMesh(meshRenderer);
        m_Particle.Play();
    }

    // 해당 부분은 필요한 것을 찾아서 직접 작업 base 없음
    private void UpdateParticleMesh(MeshRenderer meshRenderer)
    {
        var shape = m_Particle.shape;
        shape.meshRenderer = meshRenderer;
    }
}
