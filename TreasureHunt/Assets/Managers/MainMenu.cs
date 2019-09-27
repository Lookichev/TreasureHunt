using Assets.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMainParams
{
	/// <summary>
	/// Устанавливает кол-во локаторов
	/// </summary>
	public string LocatorAmount { get; set; }

	/// <summary>
	/// Устанавливает кол-во сокровищ
	/// </summary>
	public string TreasureAmount { get; set; }

	/// <summary>
	/// Устанавливает радиус локаторов
	/// </summary>
	public string LocatorRadius { get; set; }

	/// <summary>
	/// Устанавливает кол-во строк поля
	/// </summary>
	public string RowsAmount { get; set; }

	/// <summary>
	/// Устанавливает кол-во колонок поля
	/// </summary>
	public string ColumnsAmount { get; set; }


	/// <summary>
	/// Имя сцены
	/// </summary>
	public string SceneToLoad;

	/// <summary>
	/// Фоновая загрузка сцены
	/// </summary>
	AsyncOperation SceneLoadingOperation;

	public void Start()
	{
		LocatorAmount = "20";
		TreasureAmount = "3";
		LocatorRadius = "9";
		RowsAmount = "15";
		ColumnsAmount = "60"; 

		//Асинхронная загрузка сцены
		SceneLoadingOperation = SceneManager.LoadSceneAsync(SceneToLoad);

		//Но не переключает на эту сцену до готовности
		SceneLoadingOperation.allowSceneActivation = false;
	}

	public void LoadScene()
	{
		UIManager.Params = new[]
		{
			LocatorAmount,
			TreasureAmount,
			LocatorRadius,
			RowsAmount,
			ColumnsAmount
		};
		//Сообщает о переключении сцен по готовности
		SceneLoadingOperation.allowSceneActivation = true;
	}
}
