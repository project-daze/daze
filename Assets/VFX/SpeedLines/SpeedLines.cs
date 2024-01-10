using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    private ParticleSystem ParticleSystem;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystemRenderer ParticleRenderer;
    private ParticleSystem.MainModule ParticlesMain;
    private ParticleSystem.Particle[] particles;

    public new Transform camera;
    public float minSpeed = 20;

    public float spawnDistance = 10;
    public float margin = 2;
    private float spawnWidth;

    public float PositionUpdateDelay = 0.1f;
    private float PositionUpdateTimeRemaining;
    private bool isInitialized;
    private Vector3 lastpos;

    public bool UpdateAtRuntime;
    public float LinesSize = 0.05f;
    public Color LinesColor1 = new Color(1, 1, 1, 0.7f);
    public Color LinesColor2 = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    public int LinesCount = 500;
    public float LinesStretching = 0.035f;

    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        ParticlesMain = ParticleSystem.main;
        emission = ParticleSystem.emission;
        ParticleRenderer = GetComponent<ParticleSystemRenderer>();

        SetParticleProperties();

        spawnWidth = spawnDistance * 4;
        var shape = ParticleSystem.shape;
        shape.scale = Vector3.one * spawnWidth;

        if(!camera)
        {
            camera = Camera.main.transform;
        }
    }

    private void SetParticleProperties()
    {
        ParticlesMain.startSize = LinesSize;
        ParticlesMain.startColor = new ParticleSystem.MinMaxGradient(LinesColor1, LinesColor2);
        emission.rateOverTime = LinesCount;
        ParticlesMain.maxParticles = LinesCount * 2;
        ParticleRenderer.velocityScale = LinesStretching;
    }

    void LateUpdate()
    {
        transform.position = camera.position;

        if (!isInitialized)
        {
            lastpos = transform.position;
            isInitialized = true;
            PositionUpdateTimeRemaining = PositionUpdateDelay;
        }

        if (UpdateAtRuntime)
        {
            SetParticleProperties();
        }

        PositionUpdateTimeRemaining -= Time.deltaTime;
        if (PositionUpdateTimeRemaining <= 0)
        {
            var speed = (lastpos - transform.position).magnitude / (PositionUpdateDelay- PositionUpdateTimeRemaining);

            if (speed < minSpeed)
            {
                ParticleSystem.Stop();
            }
            else
            {
                ParticleSystem.Play();
            }
            lastpos = transform.position;
            PositionUpdateTimeRemaining = PositionUpdateDelay;
        }

        if (particles == null || particles.Length < ParticleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[ParticleSystem.main.maxParticles];
        int numParticlesAlive = ParticleSystem.GetParticles(particles);

        Vector3 pos;
        Vector3 offset;
        for (int i = 0; i < numParticlesAlive; i++)
        {
            pos = particles[i].position;
            offset = pos - transform.position;
            if (Mathf.Abs(offset.x) > spawnDistance + margin)
            {
                pos.x -= Mathf.Sign(offset.x) * spawnWidth;
            }
            if (Mathf.Abs(offset.y) > spawnDistance + margin)
            {
                pos.y -= Mathf.Sign(offset.y) * spawnWidth;
            }
            if (Mathf.Abs(offset.z) > spawnDistance + margin)
            {
                pos.z -= Mathf.Sign(offset.z) * spawnWidth;
            }
            particles[i].position = pos;
        }
        ParticleSystem.SetParticles(particles, numParticlesAlive);
    }
}
