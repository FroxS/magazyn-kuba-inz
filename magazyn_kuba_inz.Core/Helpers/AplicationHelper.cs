using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Diagnostics;
using Warehouse.Core.Interface;
using Warehouse.Models.Page;
using Warehouse.Service.Interface;

namespace Warehouse.Core.Helpers;

public static class AplicationHelper
{
    #region Public static properties

    public static string GetAplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    public static bool IsNeighborPowerTwo(this int one, int second, bool onlyNext = false)
    {
		if(!onlyNext || second > one)
		{
			int log2liczba1 = (int)Math.Log(one, 2);
			int log2liczba2 = (int)Math.Log(second, 2);

			return Math.Abs(log2liczba1 - log2liczba2) == 1;
		}

		return false;
		
	}

	#endregion

}