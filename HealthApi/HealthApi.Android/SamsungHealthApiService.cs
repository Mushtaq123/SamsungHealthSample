using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.OS;
using Com.Samsung.Android.Sdk.Healthdata;
using HealthApi;
using HealthApi.Droid;
using Xamarin.Forms;
using static Com.Samsung.Android.Sdk.Healthdata.HealthDataResolver;
using static Com.Samsung.Android.Sdk.Healthdata.HealthPermissionManager;

[assembly: Dependency(typeof(SamsungHealthApiService))]
namespace HealthApi.Droid
{
    public class SamsungHealthApiService : Java.Lang.Object,
        ISamsungHealthApiService,
        HealthDataStore.IConnectionListener,
        StepCountObserver
    {
        private HealthDataStore mStore;
        private StepCountReporter mReporter;
        public event EventHandler<SamsungEventArgs> OnSamsungHealthApiValueChanged;

        public void Connect()
        {
            mStore = new HealthDataStore(Forms.Context, this);
            mStore.ConnectService();
        }

        private Java.Lang.Boolean IsPermissionAcquired()
        {
            PermissionKey permKey = new PermissionKey(HealthConstants.StepCount.HealthDataType, PermissionType.Read);
            HealthPermissionManager pmsManager = new HealthPermissionManager(mStore);
            try
            {
                ICollection<PermissionKey> list = new Collection<PermissionKey>() { permKey };
                IDictionary<PermissionKey, Java.Lang.Boolean> resultMap = pmsManager.IsPermissionAcquired(list);
                return resultMap[permKey];
            }
            catch (System.Exception e)
            {

            }
            return new Java.Lang.Boolean(false);
        }
        
        private void RequestPermission()
        {
            PermissionKey permKey = new PermissionKey(HealthConstants.StepCount.HealthDataType, PermissionType.Read);
            ICollection<PermissionKey> list = new Collection<PermissionKey>() { permKey };
            HealthPermissionManager pmsManager = new HealthPermissionManager(mStore);
            IHealthResultHolder healthResultHolder = pmsManager.RequestPermissions(list);
            healthResultHolder.SetResultListener(new CustomSetResultListener(mReporter, this));
        }

        public void OnConnected()
        {
            mReporter = new StepCountReporter(mStore);
            if (IsPermissionAcquired().BooleanValue())
            {
                mReporter.Start(this);
            }
            else
            {
                RequestPermission();
            }
        }

        public void OnConnectionFailed(HealthConnectionErrorResult p0)
        {

        }

        public void OnDisconnected()
        {

        }

        public void OnChanged(int count)
        {
            OnSamsungHealthApiValueChanged?.Invoke(this, new SamsungEventArgs()
            {
                Count = count
            });
        }
    }

    public class CustomSetResultListener : Java.Lang.Object, IHealthResultHolderResultListener
    {
        private StepCountObserver mStepCountObserver;
        private StepCountReporter mReporter;
        public CustomSetResultListener(StepCountReporter reporter, StepCountObserver stepCountObserver)
        {
            mReporter = reporter;
            mStepCountObserver = stepCountObserver;
        }

        public Java.Lang.Object Await()
        {
            return null;
        }

        public void Cancel()
        {

        }

        public void OnResult(Java.Lang.Object result)
        {
            PermissionResult result1 = (PermissionResult)result;
            IDictionary<PermissionKey, Java.Lang.Boolean> resultMap = result1.ResultMap;
            PermissionKey permKey = new PermissionKey(HealthConstants.StepCount.HealthDataType, PermissionType.Read);

            if (resultMap[permKey].BooleanValue().Equals(Java.Lang.Boolean.False))
            {

            }
            else
            {
                mReporter.Start(mStepCountObserver);
            }
        }
    }
}