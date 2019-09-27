using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
	/// <summary>
	/// Статус клетки
	/// </summary>
	public enum StatusCell : int
	{
		/// <summary>
		/// Пустая
		/// </summary>
		Empty = 0,

		/// <summary>
		/// Сокровище
		/// </summary>
		Treasure = 1,

		/// <summary>
		/// Локатор
		/// </summary>
		Locator = 2
	}



	/// <summary>
	/// Размер текста локатора
	/// </summary>
	private const float c_Size = 0.5f;

	/// <summary>
	/// Поле текста
	/// </summary>
	private TextMesh _TestMesh;

	/// <summary>
	/// Адрес родительской клетки
	/// </summary>
	private int _I = -1;
	private int _J = -1;

	/// <summary>
	/// Закрыта-ли клетка для обновления информации датчика
	/// </summary>
	private bool _IsCloseCell;



	/// <summary>
	/// Расстояние действия локатора
	/// </summary>
	public static int LocatorRadius { get; set; }

	/// <summary>
	/// Состояние клетки
	/// </summary>
	public StatusCell Status { get; set; }


	private void Start()
	{
		_TestMesh = GetComponentInChildren<TextMesh>();
	}

	/// <summary>
	/// Установка адреса родительской ячейки
	/// </summary>
	/// <param name="i">Номер ряда</param>
	/// <param name="j">Номер колонки</param>
	public void SetAddress(int i, int j)
	{
		//Если : адрес уже задан - его нельзя изменять
		if (_I != -1) return;

		_I = i;
		_J = j;
	}

	/// <summary>
	/// Пересчет расстояние до ближайшего сокровища
	/// </summary>
	public void UpdateRangeInformation()
	{
		//Если : клетка закрыта для поиска - выход 
		if (_IsCloseCell) return;

		//Периметр с ближайшим сокровищем
		int result = -1;

		try
		{
			//Итераторы циклов
			int lvl = 1, i = -1, j = -1;

			//Проходка по рамкам
			//Каждая итерация обходит по граничным клеткам внутреннюю зону
			//После каждой итерации периметр добавляется во внутреннюю зону
			for (lvl = 1; lvl <= LocatorRadius; lvl++)
			{
				//Верхняя рамка периметра с окаймовкой боковых рамок
				for (i = _I - lvl, j = _J - lvl; j <= _J + lvl; j++)
					if (CheckCell(i, j)) { result = lvl; return; }
				//Левая рамка периметра без окаймлений
				for (i = _I - lvl + 1, j = _J - lvl; i <= _I + lvl - 1; i++)
					if (CheckCell(i, j)) { result = lvl; return; }
				//Нижняя рамка периметра с окаймовкой боковых рамок
				for (i = _I + lvl, j = _J - lvl; j <= _J + lvl; j++)
					if (CheckCell(i, j)) { result = lvl; return; }
				//Правая рамка периметра без окаймлений
				for (i = _I - lvl + 1, j = _J + lvl; i <= _I + lvl - 1; i++)
					if (CheckCell(i, j)) { result = lvl; return; }
			}
		}
		finally
		{
			//Если : найдено ближайшее сокровище
			if (result != -1)
				_TestMesh.text = result.ToString();
			//Сокровищ в радиусе локатора больше нет
			else
			{
				_TestMesh.text = "X";
				_IsCloseCell = false;
			}
		}
	}

	/// <summary>
	/// Локатор установлен на клетку с сокровищем, закрытие для поиска в клетке
	/// </summary>
	public void CloseCell()
	{
		_TestMesh.text = "0";
		_IsCloseCell = true;
	}

	/// <summary>
	/// Проверка клетки на предмет существования и сокровища
	/// </summary>
	private bool CheckCell(int i, int j)
	{
		//Выход за массив по нижней границе
		if (i < 0 || j < 0) return false;
		//Выход за массив по верхней границе
		if (i >= Field.Cells.Length || j >= Field.Cells[0].Length) return false;

		//Клетка с такой координатой существует
		//Истина, если в ней есть сокровище
		return Field.Cells[i][j].Status == StatusCell.Treasure;
	}
}
