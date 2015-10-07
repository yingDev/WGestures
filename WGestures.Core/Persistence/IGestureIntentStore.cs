using System.Collections.Generic;
using WGestures.Core.Commands;

namespace WGestures.Core.Persistence
{
    public interface IGestureIntentStore : IEnumerable<ExeApp>
    {
        GlobalApp GlobalApp { get; }
        bool TryGetExeApp(string key, out ExeApp found );
        ExeApp GetExeApp(string key);
        AbstractCommand[] HotCornerCommands { get; set; }

        void Remove(string app);
        void Remove(ExeApp app);
        void Add(ExeApp app);
        void Save();
    }
}
