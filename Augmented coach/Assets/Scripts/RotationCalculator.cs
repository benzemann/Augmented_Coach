using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationCalculator {

	public static Quaternion RotateTowardsPoint(Transform from, Vector3 to, Vector3 forward, Quaternion currentRotation, float speed)
    {
        Vector3 targetDir = to - from.position;
        Vector3 localTarget = from.transform.InverseTransformPoint(to);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * speed);
        return currentRotation * deltaRotation;
    }
}
