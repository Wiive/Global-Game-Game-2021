using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
	TMP_Text scoreText;

	[SerializeField] float scoreTransitionTime = 2f;
	float pointAnimTimer = 0f;

	int currentScore = 0;

	int savedDisplayedScore = 0;

	int displayedScore = 0;


	private void Awake()
	{
		scoreText = GetComponent<TMP_Text>();       
	}

	private void OnEnable()
	{
		EventManager.instance.onScoreUpdate += UpdateScore;
	}

	private void OnDisable()
	{
		EventManager.instance.onScoreUpdate -= UpdateScore;
	}

	private void Update()
	{
		if (pointAnimTimer > 1) return;

		if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.IngameMenu)
			pointAnimTimer = 1;

		pointAnimTimer += Time.deltaTime;
		float prcComplete = pointAnimTimer / scoreTransitionTime;
		displayedScore = (int)Mathf.Lerp(savedDisplayedScore, currentScore, prcComplete);
		scoreText.text = "Score: " + displayedScore.ToString();
	}

	private void UpdateScore(int score)
	{
		savedDisplayedScore = displayedScore;
		currentScore = score;
		pointAnimTimer = 0;
	}
}
