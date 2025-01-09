using System.Linq;

namespace DominoDatabase
{
	public class XMLReportItems
	{
		public int flag { get; set; }
		public string parentCode { get; set; }
		public string curCode { get; set; }
		public int packLayer { get; set; }
	}

	public class Relation
	{
		public string productCode { get; set; }
		public string subTypeNo { get; set; }
		public string cascade { get; set; }
		public string packageSpec { get; set; }
		public string packUnit { get; set; }
		public string physicDetailType { get; set; }
		public string comment { get; set; }

		public override string ToString()
		{
			return $"{productCode}|{subTypeNo}|{cascade}|{packageSpec}|{packUnit}|{physicDetailType}|{comment}";
		}

		public void SetFromDataString(string dataString)
		{
			if (string.IsNullOrWhiteSpace(dataString))
				return;

			var splits = dataString.Split('|');

			for (int i = 0; i < splits.Count(); i++)
			{
				switch (i)
				{
					case 0: productCode = splits[i]; break;
					case 1: subTypeNo = splits[i]; break;
					case 2: cascade = splits[i]; break;
					case 3: packageSpec = splits[i]; break;
					case 4: packUnit = splits[i]; break;
					case 5: physicDetailType = splits[i]; break;
					case 6: comment = splits[i]; break;
				}
			}
		}
	}
}
