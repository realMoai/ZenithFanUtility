using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsusSystemAnalysis;

namespace AsusFanControl
{
    public class AsusControl : IDisposable
    {
        private bool disposed = false;

        public AsusControl()
        {
            AsusWinIO64.InitializeWinIo();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                AsusWinIO64.ShutdownWinIo();
                disposed = true;
            }
        }

        public void SetFanSpeed(byte value, byte fanIndex = 0)
        {
            AsusWinIO64.HealthyTable_SetFanIndex(fanIndex);
            AsusWinIO64.HealthyTable_SetFanTestMode((char)(value > 0 ? 0x01 : 0x00));
            AsusWinIO64.HealthyTable_SetFanPwmDuty(value);
        }

        public void SetFanSpeed(int percent, byte fanIndex = 0)
        {
            var value = (byte)(percent / 100.0f * 255);
            SetFanSpeed(value, fanIndex);
        }

        public async Task SetFanSpeedsAsync(byte value)
        {
            var fanCount = AsusWinIO64.HealthyTable_FanCounts();
            for (byte fanIndex = 0; fanIndex < fanCount; fanIndex++)
            {
                SetFanSpeed(value, fanIndex);
                await Task.Delay(10);
            }
        }

        public async Task SetFanSpeedsAsync(int percent)
        {
            var value = (byte)(percent / 100.0f * 255);
            await SetFanSpeedsAsync(value);
        }

        public int GetFanSpeed(byte fanIndex = 0)
        {
            AsusWinIO64.HealthyTable_SetFanIndex(fanIndex);
            var fanSpeed = AsusWinIO64.HealthyTable_FanRPM();
            return fanSpeed;
        }

        public List<int> GetFanSpeeds()
        {
            var fanSpeeds = new List<int>();

            var fanCount = AsusWinIO64.HealthyTable_FanCounts();
            for (byte fanIndex = 0; fanIndex < fanCount; fanIndex++)
            {
                var fanSpeed = GetFanSpeed(fanIndex);
                fanSpeeds.Add(fanSpeed);
            }

            return fanSpeeds;
        }

        public int HealthyTable_FanCounts()
        {
            return AsusWinIO64.HealthyTable_FanCounts();
        }

        public ulong Thermal_Read_Cpu_Temperature()
        {
            return AsusWinIO64.Thermal_Read_Cpu_Temperature();
        }

        public ulong Thermal_Read_GpuTS1L_Temperature()
        {
            return AsusWinIO64.Thermal_Read_GpuTS1L_Temperature();
        }

        public ulong Thermal_Read_GpuTS1R_Temperature()
        {
            return AsusWinIO64.Thermal_Read_GpuTS1R_Temperature();
        }

        public ulong Thermal_Read_Highest_Gpu_Temperature()
        {
            ulong num1 = AsusWinIO64.Thermal_Read_GpuTS1L_Temperature();
            ulong num2 = AsusWinIO64.Thermal_Read_GpuTS1R_Temperature();
            return num1 > num2 ? num1 : num2;
        }
    }
}

