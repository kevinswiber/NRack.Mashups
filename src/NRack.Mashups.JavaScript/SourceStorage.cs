using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NRack.Mashups.JavaScript
{
    public class SourceStorage
    {
        private static SourceStorage _instance;
        private static readonly object SyncLock = new object();
        private readonly IDictionary<string, string> _sourceStorage = new ConcurrentDictionary<string, string>();

        public IDictionary<string, string> Sources { get { return _sourceStorage;  }}

        public static SourceStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SourceStorage();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}