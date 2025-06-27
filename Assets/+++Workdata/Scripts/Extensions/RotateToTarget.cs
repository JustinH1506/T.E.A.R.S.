using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class RotateToTarget : CinemachineExtension
{
    public bool YRotationOnly = true;
    
    /// <summary>
    /// Rotates the object to the given target around the y axis. 
    /// </summary>
    /// <param name="vcam"></param>
    /// <param name="stage"></param>
    /// <param name="state"></param>
    /// <param name="deltaTime"></param>
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            var follow = VirtualCamera.Follow;
            if (follow != null)
            {
                Vector3 fwd = state.RawOrientation * Vector3.forward;
                if (YRotationOnly)
                    fwd = fwd.ProjectOntoPlane(state.ReferenceUp);
                follow.rotation = Quaternion.LookRotation(fwd, state.ReferenceUp);
            }
        }
    }
}
