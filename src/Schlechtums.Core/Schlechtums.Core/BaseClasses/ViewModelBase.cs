using System;
using System.Collections.Generic;
using System.Linq;
using Schlechtums.Core.Common.Extensions;

namespace Schlechtums.Core.BaseClasses
{
	public class ExecuteCommandException : Exception
	{
		public List<string> ErrorMessages { get; set; }

		public ExecuteCommandException(IEnumerable<string> errorMessages)
		{
			var asList = errorMessages as List<string>;
			if (asList != null)
				this.ErrorMessages = asList;
			else
				this.ErrorMessages = errorMessages.ToList();
		}

		public override string ToString()
		{
			return string.Format("Error executing command:{0}{1}", Environment.NewLine, this.ErrorMessages.JoinWithNewline());
		}
	}

    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        public ViewModelBase()
        {
        }

        private bool isValid;
        public bool IsValid
        {
            get { return this.isValid; }
            set
            {
                if (this.isValid != value)
                {
                    this.isValid = value;
                    this.RaisePropertyChanged("IsValid");
                }
            }
        }
    }
}