using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float currentTime = 0f;
    public TextMeshProUGUI timerText;  // Drag your UI Text component here to display the timer

    [SerializeField]private bool isTimerRunning;



    public static Timer Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isTimerRunning == true)
        {
            currentTime += Time.deltaTime;  // Increment time based on the frame duration
            UpdateTimerDisplay();
        }
    }

    // Starts the timer
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // Stops the timer
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Reset the timer
    public void ResetTimer()
    {
        currentTime = 0f;
        UpdateTimerDisplay();
    }

    // Update the UI to display the current time in minutes:seconds format
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60F);  // Calculate minutes
            int seconds = Mathf.FloorToInt(currentTime % 60F);  // Calculate seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // Format MM:SS
        }
    }
}
