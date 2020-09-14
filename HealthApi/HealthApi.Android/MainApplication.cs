using System;

using Android.App;
using Android.OS;
using Android.Runtime;

namespace HealthApi.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    //[MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyBXeD8uUNn21V5Dt3SNnakhElVJI0u2bOk")]
    [MetaData("com.samsung.android.health.permission.read", Value = "com.samsung.health.step_count")]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {            
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}