using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schlechtums.Core.Common
{
	/// <summary>
	/// Class which calculates a moving average
	/// </summary>
	public class MovingAverage : IMovingAverage
	{
		/// <summary>
		/// Instantiates a moving average calculator with a 60 second window and a rounding precision of 1.
		/// </summary>
		/// <param name="totalNumItems">The total number of items to be processed.</param>
		public MovingAverage(int totalNumItems)
			: this(totalNumItems, 1)
		{ }

		/// <summary>
		/// Instantiates a moving average calculator with a 60 second window.
		/// </summary>
		/// <param name="totalItems">The total number of items to be processed.</param>
		/// <param name="roundingPrecision">The decimal precision for the number of records per second.</param>
		public MovingAverage(int totalNumItems, int roundingPrecision)
			: this(totalNumItems, roundingPrecision, 60)
		{ }

		/// <summary>
		/// Instantiates a moving average calculator.
		/// </summary>
		/// /// <param name="totalNumItems">The total number of items to be processed.</param>
		/// /// <param name="roundingPrecision">The decimal precision for the number of records per second.</param>
		/// <param name="windowSizeSeconds">The window size in seconds.</param>
		public MovingAverage(int totalNumItems, int roudingPrecision, int windowSizeSeconds)
		{
			this.ma = new MovingAverage<int>(totalNumItems, roudingPrecision, windowSizeSeconds);
		}

		private MovingAverage<int> ma;

		/// <summary>
		/// Gets the current window average
		/// </summary>
		public Double CurrentAverage => ((IMovingAverage)this.ma).CurrentAverage;

		/// <summary>
		/// The current estimated time remaining based on the current window average.  Calculated with the records per second decimal precision setting.
		/// </summary>
		public TimeSpan CurrentTimeRemaining => ((IMovingAverage)this.ma).CurrentTimeRemaining;

		public int PreviousRunOffset
		{
			get
			{
				return this.ma.PreviousRunOffset;
			}
			set
			{
				this.ma.PreviousRunOffset = value;
			}
		}

		/// <summary>
		/// Gets the current time remaining as a pretty printed string.
		/// </summary>
		public string CurrentTimeRemainingString => ((IMovingAverage)this.ma).CurrentTimeRemainingString;

		/// <summary>
		/// Returns a pretty printed string of the current status showing current speed and time remaining.
		/// </summary>
		public string CurrentStatusString => ((IMovingAverage)this.ma).CurrentStatusString;

		public string GetCurrentStatusStringWithCurrTotal(int curr, int total)
		{
			return string.Format("{0} ({1:n0} of {2:n0})", this.CurrentStatusString, curr, total);
		}

		/// <summary>
		/// Returns the overall, non moving average of the entire operation up until this point.
		/// </summary>
		public Double OverallAverage => ((IMovingAverage)this.ma).OverallAverage;

		/// <summary>
		/// Gets the time stamps in the current window
		/// </summary>
		public List<DateTime> CurrentTimes => ((IMovingAverage)this.ma).CurrentTimes;

		/// <summary>
		/// Sets the start time of the timed operation.  Only needed for computing the overall average at the end.
		/// </summary>
		public void Start()
		{
			((IMovingAverage)this.ma).Start();
		}

		/// <summary>
		/// Pushes a time stamp.
		/// </summary>
		public void Push()
		{
			((IMovingAverage)this.ma).Push();
		}

		/// <summary>
		/// Pushes a time stamp multiple times.
		/// </summary>
		/// <param name="count">The number of times to push.</param>
		public void PushMultiple(int count)
		{
			((IMovingAverage)this.ma).PushMultiple(count);
		}

		/// <summary>
		/// Resets the calculation using the existing configuration.
		/// </summary>
		public void Reset()
		{
			((IMovingAverage)this.ma).Reset();
		}

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		public void Reset(int totalNumItems)
		{
			((IMovingAverage)this.ma).Reset(totalNumItems);
		}

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		/// <param name="decimalPrecision">The new records per second decimal precision.</param>
		/// <param name="windowSizeSeconds">The new window size in seconds.</param>
		public void Reset(int totalNumItems, int decimalPrecision, int windowSizeSeconds)
		{
			((IMovingAverage)this.ma).Reset(totalNumItems, decimalPrecision, windowSizeSeconds);
		}
	}

	public class MovingAverage<T> : IMovingAverage
	{
		/// <summary>
		/// Instantiates a moving average calculator with a 60 second window and a rounding precision of 1.
		/// </summary>
		/// <param name="totalNumItems">The total number of items to be processed.</param>
		public MovingAverage(int totalNumItems)
			: this(totalNumItems, 1)
		{ }

		/// <summary>
		/// Instantiates a moving average calculator with a 60 second window.
		/// </summary>
		/// <param name="totalItems">The total number of items to be processed.</param>
		/// <param name="roundingPrecision">The decimal precision for the number of records per second.</param>
		public MovingAverage(int totalNumItems, int roundingPrecision)
			: this(60, roundingPrecision, totalNumItems)
		{ }

		/// <summary>
		/// Instantiates a moving average calculator.
		/// </summary>
		/// <param name="totalNumItems">The total number of items to be processed.</param>
		/// <param name="roundingPrecision">The decimal precision for the number of records per second.</param>
		/// <param name="windowSizeSeconds">The window size in seconds.</param>
		public MovingAverage(int totalNumItems, int roundingPrecision, int windowSizeSeconds)
		{
			this.windowSizeSeconds = windowSizeSeconds;
			this.roundingPrecision = roundingPrecision;
			this.totalNumItems = totalNumItems;
			this.times = new Queue<Tuple<DateTime, T>>();
			this.defaultT = default(T);
			this.totalQueuedItems = 0;
			this.Start();
		}

		private float windowSizeSeconds;
		private int roundingPrecision;
		private int totalNumItems;
		private Queue<Tuple<DateTime, T>> times;
		private long totalQueuedItems;
		private T defaultT;
		private DateTime? startTime;
		private Object lockObject = new Object();

		/// <summary>
		/// Sets the start time of the timed operation.  Only needed for computing the overall average at the end.
		/// </summary>
		public void Start()
		{
			this.startTime = DateTime.Now;
		}

		/// <summary>
		/// Gets the current window average
		/// </summary>
		public Double CurrentAverage
		{
			get
			{
				this.CullQueue();

				lock (this.lockObject)
				{
					float runningSeconds;
					if (this.times.Count == this.totalQueuedItems) //we haven't culled, we are still within the first window
						runningSeconds = (float)(DateTime.Now - this.times.First().Item1).TotalSeconds;
					else
						runningSeconds = this.windowSizeSeconds;

					if (runningSeconds == 0)
						return 0;
					else
						return Math.Round(this.times.Count / runningSeconds, this.roundingPrecision);
				}
			}
		}

		/// <summary>
		/// The current estimated time remaining based on the current window average.  Calculated with the records per second decimal precision setting.
		/// </summary>
		public TimeSpan CurrentTimeRemaining
		{
			get
			{
				if (this.totalNumItems < 1)
					return TimeSpan.FromSeconds(0);

				var ca = this.CurrentAverage;
				if (ca == 0)
					return TimeSpan.MaxValue;
				else
				{
					return TimeSpan.FromSeconds((this.totalNumItems - this.totalQueuedItems - this.PreviousRunOffset) / ca);
				}
			}
		}

		public int PreviousRunOffset { get; set; }

		/// <summary>
		/// Gets the current time remaining as a pretty printed string.
		/// </summary>
		public string CurrentTimeRemainingString
		{
			get
			{
				var remaining = this.CurrentTimeRemaining;
				if (remaining.TotalDays >= 1)
					return string.Format(@"{0:dd\.hh\:mm\:ss}", this.CurrentTimeRemaining).EnsureDoesNotStartWith("0");
				else if (remaining.TotalHours >= 1)
					return string.Format(@"{0:hh\:mm\:ss}", this.CurrentTimeRemaining).EnsureDoesNotStartWith("0");
				else if (remaining.TotalMinutes >= 1)
					return string.Format(@"{0:mm\:ss}", this.CurrentTimeRemaining).EnsureDoesNotStartWith("0");
				else
					return string.Format(@"{0:ss} {1}", this.CurrentTimeRemaining, ((int)this.CurrentTimeRemaining.TotalSeconds).ToPlural(word: "second")).EnsureDoesNotStartWith("0");
			}
		}

		/// <summary>
		/// Gets the estimated completion time based on the current time remaining
		/// </summary>
		public DateTime? EstimatedCompletionTime
		{
			get
			{
				if (this.CurrentAverage == 0)
					return null;
				else
					return DateTime.Now.AddSeconds(this.CurrentTimeRemaining.TotalSeconds);
			}
		}

		/// <summary>
		/// Returns a pretty printed string of the current status showing current speed and time remaining.
		/// </summary>
		public string CurrentStatusString
		{
			get
			{
				return string.Format("{0:n" + this.roundingPrecision + "}/sec, {1} remaining.  Completion time: {2}", this.CurrentAverage, this.CurrentTimeRemainingString, this.EstimatedCompletionTime);
			}
		}

		/// <summary>
		/// Returns the overall, non moving average of the entire operation up until this point.
		/// </summary>
		public Double OverallAverage
		{
			get
			{
				if (this.startTime == null)
					throw new Exception("Did not set moving average start time.");

				var totalSeconds = (DateTime.Now - this.startTime.Value).TotalSeconds;
				if (totalSeconds == 0)
					return 0;

				return Math.Round(this.totalQueuedItems / totalSeconds, this.roundingPrecision);
			}
		}

		/// <summary>
		/// Gets the time stamps in the current window
		/// </summary>
		public List<DateTime> CurrentTimes
		{
			get
			{
				this.CullQueue();
				lock (this.lockObject)
				{
					return this.times.Select(t => t.Item1).ToList();
				}
			}
		}

		/// <summary>
		/// Gets the items in the current window
		/// </summary>
		public List<T> CurrentItems
		{
			get
			{
				this.CullQueue();
				lock (this.lockObject)
				{
					return this.times.Select(t => t.Item2).ToList();
				}
			}
		}

		/// <summary>
		/// Pushes a time stamp.
		/// </summary>
		public void Push()
		{
			this.Push(this.defaultT);
		}

		/// <summary>
		/// Pushes an item to be time stamped
		/// </summary>
		/// <param name="item">The items to push.</param>
		public void Push(T item)
		{
			this.PushItem(item, DateTime.Now);
		}

		/// <summary>
		/// Pushes a time stamp multiple times.
		/// </summary>
		/// <param name="count">The number of times to push.</param>
		public void PushMultiple(int count)
		{
			this.PushMultiple(this.defaultT, count);
		}

		/// <summary>
		/// Pushes an item multiple times with the same time stamp
		/// </summary>
		/// <param name="item">The item to push.</param>
		/// <param name="count">The number of times to push.</param>
		public void PushMultiple(T item, int count)
		{
			var time = DateTime.Now;
			for (int i = 0; i < count; i++)
			{
				this.PushItem(item, time);
			}
		}

		/// <summary>
		/// Pushes multiple items with the same time stamp.
		/// </summary>
		/// <param name="items">The items to push.</param>
		public void PushMultiple(IEnumerable<T> items)
		{
			var time = DateTime.Now;
			foreach (var i in items)
			{
				this.PushItem(i, time);
			}
		}

		/// <summary>
		/// Resets the calculation using the existing configuration.
		/// </summary>
		public void Reset()
		{
			this.Reset(this.totalNumItems);
		}

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		public void Reset(int totalNumItems)
		{
			this.Reset(totalNumItems, this.roundingPrecision, (int)this.windowSizeSeconds);
		}

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		/// <param name="decimalPrecision">The new records per second decimal precision.</param>
		/// <param name="windowSizeSeconds">The new window size in seconds.</param>
		public void Reset(int totalNumItems, int decimalPrecision, int windowSizeSeconds)
		{
			lock (this.lockObject)
			{
				this.times.Clear();
			}

			this.windowSizeSeconds = windowSizeSeconds;
			this.roundingPrecision = decimalPrecision;
			this.totalNumItems = totalNumItems;
			this.totalQueuedItems = 0;
			this.startTime = DateTime.Now;
		}

		private void PushItem(T item, DateTime time)
		{
			lock (this.lockObject)
			{
				this.times.Enqueue(new Tuple<DateTime, T>(time, item));
				this.totalQueuedItems++;
			}
		}

		private void CullQueue()
		{
			lock (this.lockObject)
			{
				var now = DateTime.Now;
				var cutoff = now.AddSeconds(-this.windowSizeSeconds);
				while (this.times.Count > 0 && this.times.Peek().Item1 < cutoff)
				{
					this.times.Dequeue();
				}
			}
		}
	}

	public interface IMovingAverage
	{
		/// <summary>
		/// Sets the start time of the timed operation.  Only needed for computing the overall average at the end.
		/// </summary>
		void Start();

		/// <summary>
		/// Gets the current window average
		/// </summary>
		Double CurrentAverage { get; }

		/// <summary>
		/// The current estimated time remaining based on the current window average.  Calculated with the records per second decimal precision setting.
		/// </summary>
		TimeSpan CurrentTimeRemaining { get; }

		/// <summary>
		/// Gets the current time remaining as a pretty printed string.
		/// </summary>
		string CurrentTimeRemainingString { get; }

		/// <summary>
		/// Returns a pretty printed string of the current status showing current speed and time remaining.
		/// </summary>
		string CurrentStatusString { get; }

		/// <summary>
		/// Returns the overall, non moving average of the entire operation up until this point.
		/// </summary>
		Double OverallAverage { get; }

		/// <summary>
		/// Gets the time stamps in the current window
		/// </summary>
		List<DateTime> CurrentTimes { get; }

		/// <summary>
		/// Pushes a time stamp.
		/// </summary>
		void Push();

		/// <summary>
		/// Pushes a time stamp multiple times.
		/// </summary>
		/// <param name="count">The number of times to push.</param>
		void PushMultiple(int count);

		/// <summary>
		/// Resets the calculation using the existing configuration.
		/// </summary>
		void Reset();

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		void Reset(int totalNumItems);

		/// <summary>
		/// Resets the calculation.
		/// </summary>
		/// <param name="totalNumItems">The new total number of items.</param>
		/// <param name="decimalPrecision">The new records per second decimal precision.</param>
		/// <param name="windowSizeSeconds">The new window size in seconds.</param>
		void Reset(int totalNumItems, int decimalPrecision, int windowSizeSeconds);
	}
}