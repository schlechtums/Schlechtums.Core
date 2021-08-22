using System;
using System.Data;

namespace Schlechtums.Core.UnitTest.TestTypes
{
	/// <summary>
	/// Specifies the direction to be used when creating a SQL parameter from this property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALParameterDirectionAttribute : Attribute
	{
		public DALParameterDirectionAttribute()
			: this(ParameterDirection.Input)
		{ }

		public DALParameterDirectionAttribute(ParameterDirection direction)
		{
			this.Direction = direction;
		}
		/// <summary>
		/// The ParameterDirection of the property.  Defaults to input.
		/// </summary>
		public ParameterDirection Direction { get; set; }
	}

	/// <summary>
	/// String.Format to be applied when writing a value into this property.  This attribute may only be used on Strings.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALWriteStringFormatAttribute : Attribute
	{
		public DALWriteStringFormatAttribute()
		{ }

		public DALWriteStringFormatAttribute(String format)
		{
			this.Format = format;
		}

		/// <summary>
		/// The String.Format string to apply when reading from the database into this property.
		/// </summary>
		public String Format { get; set; }
	}

	/// <summary>
	/// String.Format to be applied when reading this property into a SQL Parameter.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALReadStringFormatAttribute : Attribute
	{
		public DALReadStringFormatAttribute()
		{ }

		public DALReadStringFormatAttribute(String format)
		{
			this.Format = format;
		}

		/// <summary>
		/// The String.Format string to apply when reading from this property into a SqlParameter.
		/// </summary>
		public String Format { get; set; }
	}

	/// <summary>
	/// Declares that a property should be ignored by the DAL.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALIgnoreAttribute : Attribute
	{ }

	/// <summary>
	/// If the SQL statement is expecting a different name for this property, or if it will return this property under a different column name, set that name in this property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALSQLParameterNameAttribute : Attribute
	{
		public DALSQLParameterNameAttribute()
		{ }

		public DALSQLParameterNameAttribute(String name)
		{
			this.Name = name;
		}

		/// <summary>
		/// When creating Sql parameters to use for the query, the name to use when reading this property.
		/// When populating a model from a query, the column name from which to read when populating this property.
		/// When reading into a table parameter type, the column name in which to write when reading this property.
		/// </summary>
		public String Name { get; set; }
	}

	/// <summary>
	/// Value to be used when populating the model in cases where the SQL statement did not return this column and the populate default values option was used.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class DALDefaultValueAttribute : Attribute
	{
		public DALDefaultValueAttribute()
			: this(null)
		{ }

		public DALDefaultValueAttribute(Object value)
		{
			this.Value = value;
		}

		/// <summary>
		/// The default value to set this property to if the select query does not contain this column.
		/// </summary>
		public Object Value { get; set; }
	}
}