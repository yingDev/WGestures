using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.Core.Commands
{
    /// <summary>
    /// 实现此接口的命令将可实现多步执行（一旦触发，便可“接管”特定的Modifier改变事件）
    /// 此接口可行的原因包括：
    /// *PathTracker允许指定不hook的Modifier
    /// *实现该接口的命令不会被中断（只能被end）
    /// *没有被禁止hook的Modifier通过ModifierTriggered方法告知目标命令
    /// </summary>
    public interface IGestureModifiersAware
    {
        /// <summary>
        /// 当手势被识别，且首次执行
        /// </summary>
        /// <param name="observeModifiers">表明感兴趣的修饰符事件，GestureModifier是一个Flags枚举</param>
        void GestureRecognized(out GestureModifier observeModifiers);

        /// <summary>
        /// 当感兴趣的修饰符发生改变（比如滚轮上滚）
        /// </summary>
        /// <param name="modifier">发生的修饰符事件</param>
        void ModifierTriggered(GestureModifier modifier);
        void GestureEnded();
    }
}
