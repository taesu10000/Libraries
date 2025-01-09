namespace DominoDatabase.InterfaceHistory
{
	public class Where
	{
		public bool? IsDateTime { get; set; }
		public string ColumnName { get; set; }
		public string Value { get; set; }
		public string From { get; set; }
		public string To { get; set; }

		public Where()
		{
			IsDateTime = null;
			ColumnName = null;
			Value = null;
			From = null;
			To = null;
		}
	}
	public class ColumnDefinition
	{
		public string ColumnName { get; set; }
		public string DataType { get; set; }
		public ColumnDefinition()
		{
			ColumnName = null; DataType = null;
		}
	}
}
