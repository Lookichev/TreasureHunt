using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// Список клеток с локаторами
	/// </summary>
	private List<Cell> _CellWithLocatorCollection = new List<Cell>();



	/// <summary>
	/// Установка локатора
	/// </summary>
	/// <param name="cell">Клетка, в которую локатор ставится</param>
	public void SetLocator(Cell cell)
	{
		//Клетка не определена
		if (cell == null) return;

		switch(cell.Status)
		{
			//В клетке уже есть локатор
			case Cell.StatusCell.Locator:
				return;
			//В клетке есть сокровище
			case Cell.StatusCell.Treasure:
				Manager.Menu.ChangeItems(true);
				cell.CloseCell();
				break;
			case Cell.StatusCell.Empty:
				Manager.Menu.ChangeItems(false);
				break;
		}

		//Проверка окончания игры, если все сокровища нашли - победа
		//Иначе, если нет больше локаторов - поражение
		if (Manager.Menu.UnfoundTreasureAmount == 0)
			Manager.Menu.GameOver(true);
		else if (Manager.Menu.UnuseLocatorAmount == 0)
			Manager.Menu.GameOver(false);

		//Установка локатора и обновление его датчика
		cell.Status = Cell.StatusCell.Locator;
		_CellWithLocatorCollection.Add(cell);

		//Обновляем показания всех датчиков
		foreach(var item in _CellWithLocatorCollection)	item.UpdateRangeInformation();
	}
}