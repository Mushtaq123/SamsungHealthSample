using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Samsung.Android.Sdk.Healthdata;
using static Com.Samsung.Android.Sdk.Healthdata.HealthDataResolver;

namespace HealthApi.Droid
{
    public class StepCountReporter : Java.Lang.Object, IHealthResultHolderResultListener
    {
        private HealthDataStore mStore;
        private StepCountObserver mStepCountObserver;
        private CustomHealthDataObserver observer;
        private static long ONE_DAY_IN_MILLIS = 24 * 60 * 60 * 1000L;

        public StepCountReporter(HealthDataStore store)
        {
            mStore = store;
            observer = new CustomHealthDataObserver(null, this);
        }

        public void Start(StepCountObserver listener)
        {
            mStepCountObserver = listener;
            // Register an observer to listen changes of step count and get today step count
            HealthDataObserver.AddObserver(mStore, HealthConstants.StepCount.HealthDataType, observer);
            ReadTodayStepCount();
        }

        // Read the today's step count on demand
        public void ReadTodayStepCount()
        {
            HealthDataResolver resolver = new HealthDataResolver(mStore, null);

            // Set time range from start time of today to the current time
            long startTime = GetStartTimeOfToday();
            long endTime = startTime + ONE_DAY_IN_MILLIS;

            IReadRequest request = new ReadRequestBuilder()
                .SetDataType(HealthConstants.StepCount.HealthDataType)
                .SetProperties(new string[] { HealthConstants.StepCount.Count })
                .SetLocalTimeRange(HealthConstants.StepCount.StartTime, HealthConstants.StepCount.TimeOffset, 0, endTime)
                .Build();

            try
            {
                resolver.Read(request).SetResultListener(this);
            }
            catch (System.Exception e)
            {
            }
        }

        private long GetStartTimeOfToday()
        {
            return DateTime.Today.Ticks;
        }

        public void OnResult(Java.Lang.Object result)
        {
            try
            {
                int count = 0;
                if (result is ReadResult result1)
                {
                    try
                    {
                        var iterator = result1.Iterator();
                        while (iterator.HasNext)
                        {
                            var data = iterator.Next();
                            //count += data.GetInt(HealthConstants.StepCount.Count);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                    finally
                    {
                        result1.Close();
                    }
                }

                if (mStepCountObserver != null)
                {
                    mStepCountObserver.OnChanged(count);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }

    public interface StepCountObserver
    {
        void OnChanged(int count);
    }

    public class CustomHealthDataObserver : HealthDataObserver
    {
        StepCountReporter stepCountReporter;
        public CustomHealthDataObserver(Handler p0, StepCountReporter stepCountReporter) : base(p0)
        {
            this.stepCountReporter = stepCountReporter;
        }

        public override void OnChange(string p0)
        {
            stepCountReporter.ReadTodayStepCount();
        }
    }
}