#UNTIY PLUGIN
############################
GameMonetize.com
############################

Setup:
 - Drag the prefab "GameMonetize" into your scene
 - Replace the GAME_ID values with your own keys
 - Use GameMonetize.Instance.ShowAd() to show an advertisement
 - Make use of the events GameMonetize.OnResumeGame and GameMonetize.OnPauseGame for resuming/pausing your game in between ads

Example:

public class ExampleClass: MonoBehaviour {
	void Awake()
	{
	  GameMonetize.OnResumeGame += ResumeGame;
	  GameMonetize.OnPauseGame += PauseGame;
	}
	
	void OnDestroy()
	{
	  GameMonetize.OnResumeGame -= ResumeGame;
	  GameMonetize.OnPauseGame -= PauseGame;
	}

	public void ResumeGame()
	{
	  // RESUME MY GAME
	}

	public void PauseGame()
	{
	  // PAUSE MY GAME
	}

	public void ShowAd()
	{
	  GameMonetize.Instance.ShowAd();	
	}
}
