using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobilePhones.Tests
{
    public abstract class CleanUpAfterTests : IDisposable
    {
        protected string cleanUpDir;
        protected string cleanUpSearchPattern;
        public void Dispose()
        {
            if(cleanUpDir != null && cleanUpSearchPattern != null)
            {
                string[] cleanUpFiles = Directory.GetFiles(cleanUpDir, cleanUpSearchPattern);
                foreach (var ff in cleanUpFiles)
                {
                    File.Delete(ff);
                }
            }
           
        }
    }
}
