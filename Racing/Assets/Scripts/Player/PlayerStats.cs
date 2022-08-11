using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public float timer = 0;

    [SerializeField] private Text timerText;
    [SerializeField] private Text bestTimeText;
    [SerializeField] private Text progressText;
    [SerializeField] Transform finish;
    
    private float bestTime;
    private float distanceToFinish;

    private void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime");
        if (bestTime == 0)
        {
            bestTimeText.text = "������ ����� ��� �� �����������";
            bestTime = float.MaxValue;
        }
        else
            bestTimeText.text = "������ �����: " + bestTime.ToString("0.0000");

        distanceToFinish = Vector3.Distance(transform.position, finish.position);
    }


    private void Update()
    {
        UpdateTimer();
        UpdateProgress();
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        timerText.text = "�����: " + timer.ToString("0.0000");
    }

    private void UpdateProgress()
    {
        float distancePassed = distanceToFinish - Vector3.Distance(transform.position, finish.position);
        int progress = Mathf.RoundToInt(distancePassed / distanceToFinish * 100);

        if (progress < 0)
            progress = 0;

        progressText.text = "��������: " + (progress).ToString() + "%";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (timer < bestTime)
            {
                bestTime = timer;
                bestTimeText.text = "������ �����: " + timer.ToString("0.0000");
                PlayerPrefs.SetFloat("BestTime", timer);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
