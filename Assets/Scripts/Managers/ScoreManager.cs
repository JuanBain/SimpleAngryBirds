using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _scoreToAdd;

    [Header("Game Over final Score")]
    [SerializeField] private TextMeshProUGUI _finalScore;

    [Header("Score To Add Game Object")]
    [SerializeField] private GameObject _scoreToAddGO;

    [Header("Time On Screen")]
    [SerializeField] private float _timeOnSCreen;

    private int _totalPoints;
    private int _pointsToAdd;


    private void Start()
    {
        _totalPoints = 0;
    }

    public void AddPoints(int points)
    {
        _pointsToAdd= points;
        _totalPoints += points;

        _score.text = _totalPoints.ToString();
        _scoreToAdd.text = "+ " + _pointsToAdd.ToString();
    }

    public IEnumerator ShowAddingPoints()
    {
        _scoreToAddGO.SetActive(true);
        yield return new WaitForSeconds(_timeOnSCreen);

        if (_scoreToAddGO.activeInHierarchy)
        {
            _scoreToAddGO.SetActive(false);
        }
    }   
    
    public void SetFinalScore()
    {
        _finalScore.text = _score.text;
    }
}
