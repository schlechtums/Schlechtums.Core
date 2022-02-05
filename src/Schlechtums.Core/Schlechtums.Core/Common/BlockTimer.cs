using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Schlechtums.Core.Common
{
    /// <summary>
    /// Disposable class that times a block of code.
    /// </summary>
    public class BlockTimer : IDisposable
    {
        public BlockTimer(string name, ILogger logger)
        {
            this.Name = name;
            this._Logger = logger;
            this._Timer = new Stopwatch();
            this._Timer.Start();
        }

        public string Name { get; private set; }
        public TimeSpan Elapsed { get { return this._Timer.Elapsed; } }

        private Stopwatch _Timer;
        private ILogger _Logger;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this._Logger != null)
                    {
                        try
                        {
                            var msg = $"Timer '{this.Name}' ran in '{this.Elapsed.TotalMilliseconds.ToString("n0")} milliseconds'";
                            this._Logger.Log(msg);
                        }
                        catch { }
                    }
                }
                
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}