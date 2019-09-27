using UnityEngine;

[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(GameManager))]
public class Manager : MonoBehaviour
{
	/// <summary>
	/// Менеджер по интерфейсу
	/// </summary>
	public static UIManager Menu { get; private set; }

	/// <summary>
	/// Менеджер по игровой механике
	/// </summary>
	public static GameManager Game { get; private set; }

    void Start()
    {
		Menu = gameObject.GetComponent<UIManager>();
		Game = gameObject.GetComponent<GameManager>();
	}
}
