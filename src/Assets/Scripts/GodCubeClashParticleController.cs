using UnityEngine;

public class GodCubeClashParticleController : MonoBehaviour
{
    public GameObject particleSystemObj;

    public void Emit()
    {
        ParticleSystem particleSystemComp = particleSystemObj.GetComponent<ParticleSystem>();
        particleSystemComp.Emit(30);
    }
}
