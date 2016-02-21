using System;
using System.Runtime.InteropServices;

namespace TCD.System.TouchInjection
{
    /// <summary>
    /// Use this Classes static methods to initialize and inject touch input.
    /// </summary>
    public class TouchInjector
    {
        /// <summary>
        /// Call this first to initialize the TouchInjection!
        /// </summary>
        /// <param name="maxCount">The maximum number of touch points to simulate. Must be less than 256!</param>
        /// <param name="feedbackMode">Specifies the visual feedback mode of the generated touch points</param>
        /// <returns>true if success</returns>
        [DllImport("User32.dll")]
        public static extern bool InitializeTouchInjection(uint maxCount = 256, TouchFeedback feedbackMode = TouchFeedback.DEFAULT);

        /// <summary>
        /// Inject an array of POINTER_TUCH_INFO
        /// </summary>
        /// <param name="count">The exact number of entries in the array</param>
        /// <param name="contacts">The POINTER_TOUCH_INFO to inject</param>
        /// <returns>true if success</returns>
        [DllImport("User32.dll")]
        public static extern bool InjectTouchInput(int count, [MarshalAs(UnmanagedType.LPArray), In] PointerTouchInfo[] contacts);
    }

    #region Types
    /// <summary>
    /// Enum of touch visualization options
    /// </summary>
    public enum TouchFeedback
    {
        /// <summary>
        /// Specifies default touch visualizations.
        /// </summary>
        DEFAULT = 0x1,
        /// <summary>
        /// Specifies indirect touch visualizations.
        /// </summary>
        INDIRECT = 0x2,
        /// <summary>
        /// Specifies no touch visualizations.
        /// </summary>
        NONE = 0x3
    }

    /// <summary>
    /// The contact area.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ContactArea
    {
        [FieldOffset(0)]
        public int left;
        [FieldOffset(4)]
        public int top;
        [FieldOffset(8)]
        public int right;
        [FieldOffset(12)]
        public int bottom;
    }
    
    /// <summary>
    /// Values that can appear in the TouchMask field of the PointerTouchInfo structure
    /// </summary>
    public enum TouchFlags
    { 
        /// <summary>
        /// Indicates that no flags are set.
        /// </summary>
        NONE = 0x00000000
    }

    /// <summary>
    /// Values that can appear in the TouchMask field of the PointerTouchInfo structure.
    /// </summary>
    public enum TouchMask
    {
        /// <summary>
        /// Default. None of the optional fields are valid.
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// The ContactArea field is valid
        /// </summary>
        CONTACTAREA = 0x00000001,
        /// <summary>
        /// The orientation field is valid
        /// </summary>
        ORIENTATION = 0x00000002,
        /// <summary>
        /// The pressure field is valid
        /// </summary>
        PRESSURE = 0x00000004
    }

    /// <summary>
    /// Values that can appear in the PointerFlags field of the PointerInfo structure.
    /// </summary>
    public enum PointerFlags
    {
        /// <summary>
        /// Default
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// Indicates the arrival of a new pointer
        /// </summary>
        NEW = 0x00000001,
        /// <summary>
        /// Indicates that this pointer continues to exist. When this flag is not set, it indicates the pointer has left detection range. 
        /// This flag is typically not set only when a hovering pointer leaves detection range (PointerFlag.UPDATE is set) or when a pointer in contact with a window surface leaves detection range (PointerFlag.UP is set). 
        /// </summary>
        INRANGE = 0x00000002,
        /// <summary>
        /// Indicates that this pointer is in contact with the digitizer surface. When this flag is not set, it indicates a hovering pointer.
        /// </summary>
        INCONTACT = 0x00000004,
        /// <summary>
        /// Indicates a primary action, analogous to a mouse left button down.
        ///A touch pointer has this flag set when it is in contact with the digitizer surface.
        ///A pen pointer has this flag set when it is in contact with the digitizer surface with no buttons pressed.
        ///A mouse pointer has this flag set when the mouse left button is down.
        /// </summary>
        FIRSTBUTTON = 0x00000010,
        /// <summary>
        /// Indicates a secondary action, analogous to a mouse right button down.
        /// A touch pointer does not use this flag.
        /// A pen pointer has this flag set when it is in contact with the digitizer surface with the pen barrel button pressed.
        /// A mouse pointer has this flag set when the mouse right button is down.
        /// </summary>
        SECONDBUTTON = 0x00000020,
        /// <summary>
        /// Indicates a secondary action, analogous to a mouse right button down. 
        /// A touch pointer does not use this flag. 
        /// A pen pointer does not use this flag. 
        /// A mouse pointer has this flag set when the mouse middle button is down.
        /// </summary>
        THIRDBUTTON = 0x00000040,
        /// <summary>
        /// Indicates actions of one or more buttons beyond those listed above, dependent on the pointer type. Applications that wish to respond to these actions must retrieve information specific to the pointer type to determine which buttons are pressed. For example, an application can determine the buttons states of a pen by calling GetPointerPenInfo and examining the flags that specify button states.
        /// </summary>
        OTHERBUTTON = 0x00000080,
        /// <summary>
        /// Indicates that this pointer has been designated as primary. A primary pointer may perform actions beyond those available to non-primary pointers. For example, when a primary pointer makes contact with a window’s surface, it may provide the window an opportunity to activate by sending it a WM_POINTERACTIVATE message.
        /// </summary>
        PRIMARY = 0x00000100,
        /// <summary>
        /// Confidence is a suggestion from the source device about whether the pointer represents an intended or accidental interaction, which is especially relevant for PT_TOUCH pointers where an accidental interaction (such as with the palm of the hand) can trigger input. The presence of this flag indicates that the source device has high confidence that this input is part of an intended interaction.
        /// </summary>
        CONFIDENCE = 0x00000200,
        /// <summary>
        /// Indicates that the pointer is departing in an abnormal manner, such as when the system receives invalid input for the pointer or when a device with active pointers departs abruptly. If the application receiving the input is in a position to do so, it should treat the interaction as not completed and reverse any effects of the concerned pointer.
        /// </summary>
        CANCELLED = 0x00000400,
        /// <summary>
        /// Indicates that this pointer just transitioned to a “down” state; that is, it made contact with the window surface.
        /// </summary>
        DOWN = 0x00010000,
        /// <summary>
        /// Indicates that this information provides a simple update that does not include pointer state changes.
        /// </summary>
        UPDATE = 0x00020000,
        /// <summary>
        /// Indicates that this pointer just transitioned to an “up” state; that is, it broke contact with the window surface.
        /// </summary>
        UP = 0x00040000,
        /// <summary>
        /// Indicates input associated with a pointer wheel. For mouse pointers, this is equivalent to the action of the mouse scroll wheel (WM_MOUSEWHEEL).
        /// </summary>
        WHEEL = 0x00080000,
        /// <summary>
        /// Indicates input associated with a pointer h-wheel. For mouse pointers, this is equivalent to the action of the mouse horizontal scroll wheel (WM_MOUSEHWHEEL).
        /// </summary>
        HWHEEL = 0x00100000
    }
    
    /// <summary>
    /// The TouchPoint structure defines the x- and y- coordinates of a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TouchPoint
    {
        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int X;
        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public int Y;
    }

    /// <summary>
    /// Identifies the pointer input types.
    /// </summary>
    public enum PointerInputType
    {
        /// <summary>
        /// Generic pointer type. This type never appears in pointer messages or pointer data. Some data query functions allow the caller to restrict the query to specific pointer type. The PT_POINTER type can be used in these functions to specify that the query is to include pointers of all types
        /// </summary>
        POINTER = 0x00000001,
        /// <summary>
        /// Touch pointer type.
        /// </summary>
        TOUCH = 0x00000002,
        /// <summary>
        /// Pen pointer type.
        /// </summary>
        PEN = 0x00000003,
        /// <summary>
        /// Mouse pointer type
        /// </summary>
        MOUSE = 0x00000004
    };
    
    /// <summary>
    /// Contains basic pointer information common to all pointer types. Applications can retrieve this information using the GetPointerInfo, GetPointerFrameInfo, GetPointerInfoHistory and GetPointerFrameInfoHistory functions. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointerInfo
    {
        /// <summary>
        /// A value from the PointerInputType enumeration that specifies the pointer type.
        /// </summary>
        public PointerInputType pointerType;

        /// <summary>
        /// An identifier that uniquely identifies a pointer during its lifetime. A pointer comes into existence when it is first detected and ends its existence when it goes out of detection range. Note that if a physical entity (finger or pen) goes out of detection range and then returns to be detected again, it is treated as a new pointer and may be assigned a new pointer identifier.
        /// </summary>
        public uint PointerId;

        /// <summary>
        /// An identifier common to multiple pointers for which the source device reported an update in a single input frame. For example, a parallel-mode multi-touch digitizer may report the positions of multiple touch contacts in a single update to the system.
        /// Note that frame identifier is assigned as input is reported to the system for all pointers across all devices. Therefore, this field may not contain strictly sequential values in a single series of messages that a window receives. However, this field will contain the same numerical value for all input updates that were reported in the same input frame by a single device.
        /// </summary>
        public uint FrameId;

        /// <summary>
        /// May be any reasonable combination of flags from the Pointer Flags constants.
        /// </summary>
        public PointerFlags PointerFlags;

        /// <summary>
        /// Handle to the source device that can be used in calls to the raw input device API and the digitizer device API.
        /// </summary>
        public IntPtr SourceDevice;

        /// <summary>
        /// Window to which this message was targeted. If the pointer is captured, either implicitly by virtue of having made contact over this window or explicitly using the pointer capture API, this is the capture window. If the pointer is uncaptured, this is the window over which the pointer was when this message was generated.
        /// </summary>
        public IntPtr WindowTarget;

        /// <summary>
        /// Location in screen coordinates.
        /// </summary>
        public TouchPoint PtPixelLocation;

        /// <summary>
        /// Location in device coordinates.
        /// </summary>
        public TouchPoint PtPixelLocationRaw;
       
        /// <summary>
        /// Location in HIMETRIC units.
        /// </summary>
        public TouchPoint PtHimetricLocation;

        /// <summary>
        /// Location in device coordinates in HIMETRIC units.
        /// </summary>
        public TouchPoint PtHimetricLocationRaw;
        
        /// <summary>
        /// A message time stamp assigned by the system when this input was received.
        /// </summary>
        public uint Time;

        /// <summary>
        /// Count of inputs that were coalesced into this message. This count matches the total count of entries that can be returned by a call to GetPointerInfoHistory. If no coalescing occurred, this count is 1 for the single input represented by the message.
        /// </summary>
        public uint HistoryCount;

        /// <summary>
        /// A value whose meaning depends on the nature of input. 
        /// When flags indicate PointerFlag.WHEEL, this value indicates the distance the wheel is rotated, expressed in multiples or factors of WHEEL_DELTA. A positive value indicates that the wheel was rotated forward and a negative value indicates that the wheel was rotated backward. 
        /// When flags indicate PointerFlag.HWHEEL, this value indicates the distance the wheel is rotated, expressed in multiples or factors of WHEEL_DELTA. A positive value indicates that the wheel was rotated to the right and a negative value indicates that the wheel was rotated to the left. 
        /// </summary>
        public uint InputData;

        /// <summary>
        /// Indicates which keyboard modifier keys were pressed at the time the input was generated. May be zero or a combination of the following values. 
        /// POINTER_MOD_SHIFT – A SHIFT key was pressed. 
        /// POINTER_MOD_CTRL – A CTRL key was pressed. 
        /// </summary>
        public uint KeyStates;

        /// <summary>
        /// TBD
        /// </summary>
        public ulong PerformanceCount;

        /// <summary>
        /// ???
        /// </summary>
        public PointerButtonChangeType ButtonChangeType;
    }

    /// <summary>
    /// Enumeration of PointerButtonChangeTypes
    /// </summary>
    public enum PointerButtonChangeType
    {
        NONE,
        FIRSTBUTTON_DOWN,
        FIRSTBUTTON_UP,
        SECONDBUTTON_DOWN,
        SECONDBUTTON_UP,
        THIRDBUTTON_DOWN,
        THIRDBUTTON_UP,
        FOURTHBUTTON_DOWN,
        FOURTHBUTTON_UP,
        FIFTHBUTTON_DOWN,
        FIFTHBUTTON_UP
    }

    /// <summary>
    /// Contains information about a 'contact' (coordinates, size, pressure...)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointerTouchInfo
    {
        ///<summary>
        /// Contains basic pointer information common to all pointer types.
        ///</summary>
        public PointerInfo PointerInfo;

        ///<summary>
        /// Lists the touch flags.
        ///</summary>
        public TouchFlags TouchFlags;

        /// <summary>
        /// Indicates which of the optional fields contain valid values. The member can be zero or any combination of the values from the Touch Mask constants.
        /// </summary>
        public TouchMask TouchMasks;

        ///<summary>
        /// Pointer contact area in pixel screen coordinates. 
        /// By default, if the device does not report a contact area, 
        /// this field defaults to a 0-by-0 rectangle centered around the pointer location.
        ///</summary>
        public ContactArea ContactArea;

        /// <summary>
        /// A raw pointer contact area.
        /// </summary>
        public ContactArea ContactAreaRaw;

        ///<summary>
        /// A pointer orientation, with a value between 0 and 359, where 0 indicates a touch pointer 
        /// aligned with the x-axis and pointing from left to right; increasing values indicate degrees
        /// of rotation in the clockwise direction.
        /// This field defaults to 0 if the device does not report orientation.
        ///</summary>
        public uint Orientation;

        ///<summary>
        /// Pointer pressure normalized in a range of 0 to 256.
        ///</summary>
        public uint Pressure;

        /// <summary>
        /// Move the touch point, together with its ContactArea
        /// </summary>
        /// <param name="deltaX">the change in the x-value</param>
        /// <param name="deltaY">the change in the y-value</param>
        public void Move(int deltaX, int deltaY)
        {
            PointerInfo.PtPixelLocation.X += deltaX;
            PointerInfo.PtPixelLocation.Y += deltaY;
            ContactArea.left += deltaX;
            ContactArea.right += deltaX;
            ContactArea.top += deltaY;
            ContactArea.bottom += deltaY;
        }
    }
    #endregion
}