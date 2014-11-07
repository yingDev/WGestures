
namespace WGestures.Core.Persistence
{
    /// <summary>
    /// 实现查找算法
    /// </summary>
    public abstract class AbstractGestureIntentFinder : IGestureIntentFinder
    {
        public IGestureIntentStore IntentStore { get; private set; }

        protected AbstractGestureIntentFinder(IGestureIntentStore intentStore)
        {
            IntentStore = intentStore;
        }

        /// <summary>
        /// 判断是否在当前的应用程序上禁用的鼠标手势
        /// 如果应用程序没有添加到记录中，则检查全局手是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsGesturingEnabledForContext(GestureContext context)
        {
            var found = GetExeAppByContext(context);
           
            return found != null ?
                found.IsGesturingEnabled : IntentStore.GlobalApp.IsGesturingEnabled;
        }

        /// <summary>
        /// 查找Intent
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public GestureIntent Find(Gesture gesture, GestureContext context)
        {
            var exeApp = GetExeAppByContext(context);
            var globalApp = IntentStore.GlobalApp;

            GestureIntent found;
            
            if (exeApp != null)
            {
                found = exeApp.Find(gesture);
                if (found == null && exeApp.InheritGlobalGestures) //是否继承了全局手势
                {
                    found = globalApp.Find(gesture);
                }
            }else
            {
                found = globalApp.Find(gesture);
            }

            return found;
        }

        /// <summary>
        /// 实现平台相关的查找
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract ExeApp GetExeAppByContext(GestureContext context);
    }
}
