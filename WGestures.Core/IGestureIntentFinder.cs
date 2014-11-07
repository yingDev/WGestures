
using WGestures.Core.Persistence;

namespace WGestures.Core
{
    public interface IGestureIntentFinder
    {
        /*bool IntentExists(Gesture gesture, GestureContext context);
        GestureIntent GetIntent();*/

        bool IsGesturingEnabledForContext(GestureContext context);

        GestureIntent Find(Gesture gesture, GestureContext context);

        //IGestureIntentStore IntentStore { get; }
    }
}
