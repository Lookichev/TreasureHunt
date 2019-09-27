using Assets.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IMainParams
{
	/// <summary>
	/// Поле оповещения о кол-ве сокровищ
	/// </summary>
	[SerializeField]
	private Text AmountTreasureTextOnInterface;

	/// <summary>
	/// Поле оповещения о кол-ве локаторов
	/// </summary>
	[SerializeField]
	private Text AmountLocatorTextOnInterface;

	/// <summary>
	/// Меню паузы
	/// </summary>
	[SerializeField]
	private GameObject MenuPause;

	/// <summary>
	/// Меню настройки
	/// </summary>
	[SerializeField]
	private GameObject SettingsMenu;

	/// <summary>
	/// Меню победы
	/// </summary>
	[SerializeField]
	private GameObject WinMenu;

	/// <summary>
	/// Меню поражения
	/// </summary>
	[SerializeField]
	private GameObject LoseMenu;


	/// <summary>
	/// Установлена-ли пауза
	/// </summary>
	public bool Pause { get; private set; }



	/// <summary>
	/// Сообщает о кол-ве оставшихся локаторов
	/// </summary>
	public int UnuseLocatorAmount => Convert.ToInt32(AmountLocatorTextOnInterface.text);

	/// <summary>
	/// Сообщает о кол-ве ненайденных сокровищ
	/// </summary>
	public int UnfoundTreasureAmount => Convert.ToInt32(AmountTreasureTextOnInterface.text);

	#region Настройка игры через меню

	/// <summary>
	/// Параметры ролика для проброса
	/// </summary>
	/// <remarks>
	/// [0] - кол-во локаторов
	/// [1] - кол-во сокровищ
	/// [2] - радиус локаторов
	/// [3] - кол-во строк
	/// [4] - кол-во колонок
	/// </remarks>
	public static string[] Params { get; set; }

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

	#endregion

	void Start()
	{
		//Получение проброшенных параметров
		LocatorAmount = Params[0];
		TreasureAmount = Params[1];
		LocatorRadius = Params[2];
		RowsAmount = Params[3];
		ColumnsAmount = Params[4];

		//Деактивация менюшек в начале ролика
		MenuPause.SetActive(false);
		SettingsMenu.SetActive(false);
		WinMenu.SetActive(false);
		LoseMenu.SetActive(false);

		//Распределение параметров
		AmountLocatorTextOnInterface.text = LocatorAmount;
		AmountTreasureTextOnInterface.text = TreasureAmount;
		Cell.LocatorRadius = Convert.ToInt32(LocatorRadius);
	}

	/// <summary>
	/// Изменение состояние игры
	/// </summary>
	public void GamePause()
	{
		// Пауза действует
		if (Pause)
		{
			Time.timeScale = 0;
			MenuPause.SetActive(Pause = false);
		}
		//Ролик проигрывается
		else
		{
			Time.timeScale = 1;
			MenuPause.SetActive(Pause = true);
		}
	}

	/// <summary>
	/// Перезапуск игры
	/// </summary>
	public void RestartGame()
	{
		//Сохранение параметров
		Params[0] = LocatorAmount;
		Params[1] = TreasureAmount;
		Params[2] = LocatorRadius;
		Params[3] = RowsAmount;
		Params[4] = ColumnsAmount;

		Time.timeScale = 1f;
		SceneManager.LoadScene(1);
	}
	
	/// <summary>
	/// Окончание ролика
	/// </summary>
	/// <param name="isWin">Победа?</param>
	public void GameOver(bool isWin)
	{
		Time.timeScale = 0f;
		if (isWin) WinMenu.SetActive(true);
		else LoseMenu.SetActive(true);

		SettingsMenu.SetActive(true);
	}

	/// <summary>
	/// Выход из игры
	/// </summary>
	public void ExitGame()
	{
		Application.Quit(1);
	}

	/// <summary>
	/// Изменение кол-ва предметов
	/// </summary>
	/// <param name="changeTreasure">Найдено-ли сокровище</param>
	public void ChangeItems(bool findTreasure = false)
	{
		//Уменьшение кол-ва неиспользованных локаторов
		var item = UnuseLocatorAmount;
		AmountLocatorTextOnInterface.text = (item -= 1).ToString();

		//Если : найдено сокровище
		if (findTreasure)
		{
			//Уменьшение кол-ва ненайденных сокровищ
			item = UnfoundTreasureAmount;
			AmountTreasureTextOnInterface.text = (item -=1 ).ToString();
		}
	}
}
