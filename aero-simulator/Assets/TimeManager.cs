using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    void Update(){
        if (Input.GetKey("+")){
        Time.timeScale += (1f/slowdownLength) * Time.unscaledDeltaTime;
        }
    }

    public void BulletTime(){
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
