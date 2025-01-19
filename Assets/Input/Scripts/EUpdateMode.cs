using System;

namespace AGGE.Input {
    [Flags]
    public enum EUpdateMode {
        None = 0,
        FixedUpdate = 1 << 0,
        Update = 1 << 1,
        LateUpdate = 1 << 2,
        WaitForFixedUpdate = 1 << 3,
        WaitForUpdate = 1 << 4,
        WaitForEndOfFrame = 1 << 5,
    }
}