using System;
using UnityEngine;

public class Field : MonoBehaviour
{
	/// <summary>
	/// Размер квадратов клеток
	/// </summary>
	private const float _ScaleSquare = 0.5f;

	/// <summary>
	/// Светлая клетка поля
	/// </summary>
	[SerializeField]
	private GameObject SquareLightPrefab;

	/// <summary>
	/// Темная клетка поля
	/// </summary>
	[SerializeField]
	private GameObject SquareDarkPrefab;


	/// <summary>
	/// Плоскость клеток
	/// </summary>
	public static Cell[][] Cells { get; private set; }



	private void Start()
	{
		var rows = Convert.ToInt32(Manager.Menu.RowsAmount);
		var columns = Convert.ToInt32(Manager.Menu.ColumnsAmount);
		//Определение размера поля
		var rangeVert = rows * _ScaleSquare;
		var rangeHor = columns * _ScaleSquare;
		gameObject.transform.localScale = new Vector3(rangeHor, rangeVert, 1);
		//Точка отчета для поля
		var x = - rangeHor / 2 + _ScaleSquare / 2;
		var y = rangeVert / 2 - _ScaleSquare / 2;

		//Заполнение поля клетками
		Cells = new Cell[rows][];
		GameObject cell;
		for (var i = 0; i < rows; i++)
		{
			Cells[i] = new Cell[columns];
			for(var j = 0; j < columns; j++)
			{
				//Заполнение четной строчки четной колонки темной клеткой
				//Нечетной строчки четной колонки светлой клеткой и обратно
				//Четная строчка
				if (i % 2 == 0)
					cell = j % 2 == 0 
						? Instantiate(SquareDarkPrefab) 
						: Instantiate(SquareLightPrefab);
				//Нечетная строчка
				else cell = j % 2 == 0 
						? Instantiate(SquareLightPrefab) 
						: Instantiate(SquareDarkPrefab);

				Cells[i][j] = cell.GetComponent<Cell>();

				//Установка координат и родителя клетки, а также ее адреса
				cell.transform.localPosition = new Vector3(x + j * _ScaleSquare, y - i * _ScaleSquare, -1);
				cell.transform.parent = gameObject.transform;
				Cells[i][j].SetAddress(i, j);
			}
		}

		//Распределение сокровищ по клеткам
		var count = Manager.Menu.UnfoundTreasureAmount;

		int it, jt;

		//Пока : не установлены все сокровища
		while(count > 0)
		{
			//Определение случайных адресов
			it = UnityEngine.Random.Range(0, Cells.Length);
			jt = UnityEngine.Random.Range(0, Cells[0].Length);

			//Если : клетка занята сокровищем - ХХ
			if (Cells[it][jt].Status == Cell.StatusCell.Treasure) continue;

			Cells[it][jt].Status = Cell.StatusCell.Treasure;
			count--;
		}
	}
}
