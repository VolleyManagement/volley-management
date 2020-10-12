using System;
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
			return (T)GetInsance(table, typeof(T));
		}

		public object GetInsance(Table table, Type instanceType)
		{
			var instance = Activator.CreateInstance(instanceType);

			foreach (var propertyInfo in instanceType.GetProperties())
			{
				var rawValue = table.Rows[0][propertyInfo.Name];
				var propType = propertyInfo.PropertyType;

				var transform = _transformFactory.GetTransform(propType);

				var value = transform.GetValue(rawValue);

				if (value != null)
				{
					propertyInfo.SetValue(instance, value);
				}
			}

			table.FillInstance(instance);

			return instance;
		}
	}
}