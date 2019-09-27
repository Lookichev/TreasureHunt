using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	/// <summary>
	/// Производится-ли управление с помощью касаний
	/// </summary>
	private const bool c_MobileControlling = false; //true

	/// <summary>
	/// Сила смещения камеры
	/// </summary>
	[SerializeField]
	private float PowerMove = 6f;

	private Camera _Camera;

	/// <summary>
	/// Первая позиция касания
	/// </summary>
	private Vector3 _FirstPos;

	/// <summary>
	/// Последняя позиция касания
	/// </summary>
	private Vector3 _LastPos;

	/// <summary>
	/// Минимальная дистанция для определения свайпа
	/// </summary>
	private float _Distance;

	/// <summary>
	/// Храним все позиции касания в списке
	/// </summary>
	private List<Vector3> _TouchsPosList = new List<Vector3>();

	void Start()
	{
		_Camera = gameObject.GetComponent<Camera>();

		//20% высоты экрана
		_Distance = Screen.height * 20 / 100;
	}

	void Update()
	{
		//Во время паузы никаких локаторов и движений!
		if (Manager.Menu.Pause) return;

		if (c_MobileControlling) TouchScreenControl();
		else MouseControl();
	}

	/// <summary>
	/// Ролик развернут на комьютере
	/// </summary>
	private void MouseControl()
	{
		//Если : нажата левая кнопка мыши
		if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			//Генерируем луч от камеры в точку нажатия
			var ray = _Camera.ScreenPointToRay(Input.mousePosition);

			//Если : попадание луча зафиксировано
			//Устанавливаем локатор
			if (Physics.Raycast(ray, out RaycastHit hit))
				Manager.Game.SetLocator(hit.transform.gameObject.GetComponent<Cell>());
		}

		//Если : нажата правая кнопка мыши
		//Обрабатывается смещение камеры относительно смещения мыши
		if (Input.GetMouseButton(1))
		{
			var moveX = Input.GetAxis("Mouse X") * PowerMove;
			var moveY = Input.GetAxis("Mouse Y") * PowerMove;

			_Camera.transform.Translate(new Vector3(moveX, moveY, 0) * Time.deltaTime);
		}
	}

	/// <summary>
	/// Ролик развернут на мобильном устройстве
	/// </summary>
	private void TouchScreenControl()
	{
		//Отслеживание всех касаний
		foreach (Touch touch in Input.touches)
		{
			//Если : касание является свайпом - сохраняем его
			if (touch.phase == TouchPhase.Moved) _TouchsPosList.Add(touch.position);

			//Если : касание завершается
			if (touch.phase == TouchPhase.Ended)
			{
				//Положения первого и последнего касаний
				_FirstPos = _TouchsPosList[0];
				_LastPos = _TouchsPosList[_TouchsPosList.Count - 1];

				//Если : дистанция перемещения больше 20% высоты экрана
				if (Mathf.Abs(_LastPos.x - _FirstPos.x) > _Distance || Mathf.Abs(_LastPos.y - _FirstPos.y) > _Distance)
				{
					var moveX = (_LastPos.x - _FirstPos.x) * PowerMove;
					var moveY = (_LastPos.y - _FirstPos.y) * PowerMove;

					_Camera.transform.Translate(new Vector3(moveX, moveY, 0) * Time.deltaTime);
				}
				//Расцениваю короткие перемещения, как простые нажатия
				else
				{
					//Генерируем луч от камеры в точку первого нажатия
					var rayFirst = _Camera.ScreenPointToRay(_FirstPos);
					//Генерируем луч от камеры в точку второго нажатия
					var rayLast = _Camera.ScreenPointToRay(_LastPos);

					//Если : попадание лучей зафиксировано
					if (Physics.Raycast(rayFirst, out RaycastHit hitFirst) && Physics.Raycast(rayLast, out RaycastHit hitLast))
						//Если оба луча дотянулись до одной клетки - Устанавливаем локатор
						if (hitFirst.Equals(hitLast)) Manager.Game.SetLocator(hitLast.transform.gameObject.GetComponent<Cell>());
				}
			}
		}
	}
}
