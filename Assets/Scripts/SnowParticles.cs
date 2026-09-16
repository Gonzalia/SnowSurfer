using UnityEngine;

// Configure the scene's existing particle renderers and materials.
public static class SnowParticles
{
    public static void Configure(ParticleSystem particles, bool celebration)
    {
        if (particles == null) return;
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = particles.main;
        main.loop = false;
        main.playOnAwake = false;
        main.duration = celebration ? 2.4f : 1.4f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, celebration ? 2.6f : 1.6f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(3f, celebration ? 12f : 8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, celebration ? 0.3f : 0.45f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.gravityModifier = celebration ? 0.45f : 0.7f;
        main.maxParticles = 350;
        main.startColor = celebration
            ? new ParticleSystem.MinMaxGradient(new Color(0.25f, 1f, 0.85f), new Color(1f, 0.7f, 0.2f))
            : new ParticleSystem.MinMaxGradient(new Color(0.5f, 0.85f, 1f), Color.white);
        var emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0f;
        emission.SetBursts(celebration
            ? new[] { new ParticleSystem.Burst(0f, 100), new ParticleSystem.Burst(0.4f, 70), new ParticleSystem.Burst(0.85f, 70) }
            : new[] { new ParticleSystem.Burst(0f, 100), new ParticleSystem.Burst(0.12f, 40) });
        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = celebration ? 0.7f : 0.3f;
        shape.rotation = Vector3.zero;
        var color = particles.colorOverLifetime;
        color.enabled = true;
        var gradient = new Gradient();
        gradient.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.9f, 0.5f), new GradientAlphaKey(0f, 1f) });
        color.color = gradient;
        var size = particles.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.1f));
        var rotation = particles.rotationOverLifetime;
        rotation.enabled = true;
        rotation.z = new ParticleSystem.MinMaxCurve(-4f, 4f);
        particles.GetComponent<ParticleSystemRenderer>().sortingOrder = 5;
    }
}
