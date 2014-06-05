using UnityEngine;

using UnityEditor;

 

// Alternate settings found at [url]http://forum.unity3d.com/viewtopic.php?t=26405[/url]

 

 

public class ChangeWheelColliderSettings : ScriptableObject {

   

    [MenuItem ("Custom/Wheel Collider/Change to Alternate 1")]

    static void ChangeWheelColliderSettings_Default1 ()

    {

            SelectedChangeWheelColliderSettings_Default1();

    }

    

    [MenuItem("Custom/Wheel Collider/Change to Alternate 2")]

    static void ChangeWheelColliderSettings_Default2 ()

    {

        SelectedChangeWheelColliderSettings_Default2 ();

    }

    

    [MenuItem("Custom/Wheel Collider/Change to Alternate 3")]

    static void ChangeWheelColliderSettings_Default3 ()

    {

        SelectedChangeWheelColliderSettings_Default3 ();

    }

    

    [MenuItem("Custom/Wheel Collider/Change to Unity Default")]

    static void ChangeWheelColliderSettings_Default ()

    {

        SelectedChangeWheelColliderSettings_Default ();

    }

    

    // ----------------------------------------------------------------------------

   

    static void SelectedChangeWheelColliderSettings_Default1 ()

    {

   

        Object[] colliders = GetSelectedWheelColliders ();

            //Selection.objects = new Object[0];

            foreach (WheelCollider collider in colliders) {

            WheelFrictionCurve curve = new WheelFrictionCurve();

            curve.extremumSlip = 0.01f;

            curve.extremumValue = 1.0f;

            curve.asymptoteSlip = 0.04f;

            curve.asymptoteValue = 0.6f;

            curve.stiffness = 50000.0f;

            collider.forwardFriction = curve;

            collider.sidewaysFriction = curve;

        }

    }

 

    static void SelectedChangeWheelColliderSettings_Default2 ()

    {

   

        Object[] colliders = GetSelectedWheelColliders ();

            //Selection.objects = new Object[0];

            foreach (WheelCollider collider in colliders) {

            WheelFrictionCurve curve = new WheelFrictionCurve();

            curve.extremumSlip = 1.0f;

            curve.extremumValue = 0.02f;

            curve.asymptoteSlip = 2.0f;

            curve.asymptoteValue = 0.01f;

            curve.stiffness = 800000.0f;

            collider.forwardFriction = curve;

            collider.sidewaysFriction = curve;

        }

    }

   

    static void SelectedChangeWheelColliderSettings_Default3 ()

    {

   

        Object[] colliders = GetSelectedWheelColliders ();

            //Selection.objects = new Object[0];

            foreach (WheelCollider collider in colliders) {

            WheelFrictionCurve curve = new WheelFrictionCurve();

            curve.extremumSlip = 1.0f;

            curve.extremumValue = 0.01f;

            curve.asymptoteSlip = 0.6f;

            curve.asymptoteValue = 0.04f;

            curve.stiffness = 6000.0f;

            collider.forwardFriction = curve;

            curve.stiffness = 4000.0f;

            collider.sidewaysFriction = curve;

        }

    }

    

    static void SelectedChangeWheelColliderSettings_Default ()

    {

   

        Object[] colliders = GetSelectedWheelColliders ();

            //Selection.objects = new Object[0];

            foreach (WheelCollider collider in colliders) {

            WheelFrictionCurve curve = new WheelFrictionCurve();

            curve.extremumSlip = 1.0f;

            curve.extremumValue = 20000.0f;

            curve.asymptoteSlip = 2.0f;

            curve.asymptoteValue = 10000.0f;

            curve.stiffness = 1.0f;

            collider.forwardFriction = curve;

            collider.sidewaysFriction = curve;

        }

    }   

    static Object[] GetSelectedWheelColliders()

    {

        return Selection.GetFiltered(typeof(WheelCollider), SelectionMode.DeepAssets);

    }

}