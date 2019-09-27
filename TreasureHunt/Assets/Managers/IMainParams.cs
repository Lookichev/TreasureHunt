
namespace Assets.Managers
{
	public interface IMainParams
	{
		/// <summary>
		/// Устанавливает кол-во локаторов
		/// </summary>
		string LocatorAmount { get; set; }

		/// <summary>
		/// Устанавливает кол-во сокровищ
		/// </summary>
		string TreasureAmount { get; set; }

		/// <summary>
		/// Устанавливает радиус локаторов
		/// </summary>
		string LocatorRadius { get; set; }

		/// <summary>
		/// Устанавливает кол-во строк поля
		/// </summary>
		string RowsAmount { get; set; }

		/// <summary>
		/// Устанавливает кол-во колонок поля
		/// </summary>
		string ColumnsAmount { get; set; }
	}
}
