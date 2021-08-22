using System;
using System.Collections.Generic;
using System.Linq;
using Schlechtums.Core.Common.Extensions;

namespace Schlechtums.Core.BaseClasses
{
	public class ExecuteCommandException : Exception
	{
		public List<String> ErrorMessages { get; set; }

		public ExecuteCommandException(IEnumerable<String> errorMessages)
		{
			var asList = errorMessages as List<String>;
			if (asList != null)
				this.ErrorMessages = asList;
			else
				this.ErrorMessages = errorMessages.ToList();
		}

		public override string ToString()
		{
			return String.Format("Error executing command:{0}{1}", Environment.NewLine, this.ErrorMessages.JoinWithNewline());
		}
	}

    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        public ViewModelBase()
        {
        }

        private Boolean m_IsValid;
        public Boolean IsValid
        {
            get { return this.m_IsValid; }
            set
            {
                if (this.m_IsValid != value)
                {
                    this.m_IsValid = value;
                    this.RaisePropertyChanged("IsValid");
                }
            }
        }
    }
}