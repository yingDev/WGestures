
using WGestures.Core.Persistence;

namespace WGestures.Core
{
    public interface IGestureIntentFinder
    {
        /*bool IntentExists(Gesture gesture, GestureContext context);
        GestureIntent GetIntent();*/

        bool IsGesturingEnabledForContext(GestureContext context, out ExeApp app);

        GestureIntent Find(Gesture gesture, GestureContext context);
        GestureIntent Find(Gesture gesture, ExeApp inApp);
        
        IGestureIntentStore IntentStore { get; }
    }
}
