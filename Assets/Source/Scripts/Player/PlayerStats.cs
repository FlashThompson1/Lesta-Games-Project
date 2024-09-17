using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _wonPanel;

    private Animator _characterAnimator;
    


    private void Start()
    {
        _characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_health < 0)
            _health = 0;
        
        _healthText.text = _health.ToString();
        GameOver();
    }

    public void TakeDamage(float damage) {

       
        _health -= damage;
        Debug.Log(_health);
    }



    private void GameOver() {

        if (_health <= 0)
        {
            StartCoroutine(TimerForDeathandWin(_gameOverPanel,"Lose"));
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Last")
            StartCoroutine(TimerForDeathandWin(_wonPanel, "Won"));
    }

    private IEnumerator TimerForDeathandWin(GameObject panel,string AnimationName) {
        _characterAnimator.SetTrigger(AnimationName);
        yield return new WaitForSeconds(1);
        panel.SetActive(true);
        Timer.Instance.StopTimer();
        yield return new WaitForSeconds(1.8f);
        Time.timeScale = 0;

    }

}
