using Siemens.Simatic.Simulation.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CoSimulationPlcSimAdv.Models
{
    /// <summary>
    /// Class for encapsulate the function of an PLCSIM Adv. virtual Controller and the function to read/write the PAE and PAA
    /// </summary>
    public class PLCInstance
    {

        #region Properties
        /// <summary>
        /// instance of PLCSIM Adv. virtual Controller
        /// </summary>
        public IInstance Instance { get; set; }

        /// <summary>
        /// indicates if Instance is configured --> tag list updatet
        /// </summary>
        private bool IsConfigured { get; set; }

        #endregion

        #region Ctor

        public PLCInstance(string instanceName, string remoteIp = "127.0.0.1", string remotePort = "5000")
        {
            Instance = SimulationRuntimeManager.RemoteConnect(string.Concat(remoteIp, ":", remotePort)).CreateInterface(instanceName);
            Instance.IsAlwaysSendOnEndOfCycleEnabled = true;

            Instance.OnEndOfCycle += instance_OnEndOfCycle;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event when PLC reach the End of the main Cycle, this will be called, whenever the Controller reaches the end of cycle.
        /// </summary>
        /// <param name="in_Sender">PLC which fired this event</param>
        /// <param name="in_ErrorCode">ErrorCode of Runtime of the PLC</param>
        /// <param name="in_DateTime">DateTime when the configuration changed</param>
        /// <param name="in_CycleTime_ns">current cycle time in ns of the PLC</param>
        /// <param name="in_CycleCount">current count of Cycles of the PLC</param>

        void instance_OnEndOfCycle(IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, 
            DateTime in_DateTime, long in_CycleTime_ns, uint in_CycleCount)
        {

            try
            {
                Console.WriteLine("HelloPLC");
                Byte[] outputBuffer = new byte[2];
                Byte[] inputBuffer = new byte[2];

                outputBuffer = Instance.OutputArea.ReadBytes(6200, 2);
              
                // Swap.ByteArray(ref outputBuffer);
                //
                // int life = BitConverter.ToInt16(outputBuffer, 0);
                // Console.WriteLine(life);
                // life += 1;
                // inputBuffer = BitConverter.GetBytes(life);
                //
                // Swap.ByteArray(ref inputBuffer);

                inputBuffer = outputBuffer;

                Instance.InputArea.WriteBytes(6200, 2, inputBuffer);
            }
            catch (Exception ex)
            {
            }
            
        }
        
        #endregion //Events
        
    }
}
