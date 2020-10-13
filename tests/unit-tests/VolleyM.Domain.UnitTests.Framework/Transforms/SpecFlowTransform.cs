using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace VolleyM.Domain.UnitTests.Framework
{
	/// <summary>
	/// Helper to build objects from SpecFlow Table parameters
	/// </summary>
	public class SpecFlowTransform
	{
		private readonly ISpecFlowTransformFactory _transformFactory;

		public SpecFlowTransform(ISpecFlowTransformFactory transformFactory)
		{
			_transformFactory = transformFactory;
		}

		public T GetInstance<T>(Table table)
		{
			return (T)GetInstance(table, typeof(T));
		}

		public object GetInstance(Table table, Type instanceType)
		{
			var instance = Activator.CreateInstance(instanceType);

			table.FillInstance(instance);

			FillInstanceFromRow(instance, table.Rows[0]);

			return instance;
		}

		public List<T> GetCollection<T>(Table table)
		{
			var result = table.CreateSet<T>().ToList();

			var i = 0;
			foreach (var item in result)
			{
				FillInstanceFromRow(item, table.Rows[i]);
				i++;
			}

			return result;
		}

		private void FillInstanceFromRow(object instance, TableRow row)
		{
			foreach (var propertyInfo in instance.GetType().GetProperties())
			{
				var rawValue = row[propertyInfo.Name];
				var propType = propertyInfo.PropertyType;

				var transform = _transformFactory.GetTransform(propType);

				var value = transform.GetValue(rawValue);

				if (value != null)
				{
					propertyInfo.SetValue(instance, value);
				}
			}
		}
	}
}