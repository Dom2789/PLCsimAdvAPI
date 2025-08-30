using Siemens.Simatic.Simulation.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PLCsimAdvAPI
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
        public bool newData { get; set; } = false;
        
        public bool firstCycle { get; set; } = true;
        public Byte[] Outputbuffer { get; private set; } = new byte[200];
        public Byte[] Inputbuffer { private get; set; } = new byte[200];
        
        private Address[] Addresses { get; set; } = new Address[49];
        

        #endregion

        #region Ctor

        public PLCInstance(string instanceName, string remoteIp = "127.0.0.1", string remotePort = "50000")
        {
            Instance = SimulationRuntimeManager.RemoteConnect(string.Concat(remoteIp, ":", remotePort)).CreateInterface(instanceName);
            Instance.IsAlwaysSendOnEndOfCycleEnabled = true;
            // SF1
            Addresses[18] = new Address(offset:3800, length:200);

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
            // stops the cpu-cycle until method is finished
            try
            {
                Console.WriteLine("HelloPLC");
                if (!firstCycle) Instance.InputArea.WriteBytes(Addresses[18].Offset, Addresses[18].Length, Outputbuffer);
                
                Outputbuffer = Instance.OutputArea.ReadBytes(Addresses[18].Offset, Addresses[18].Length );
                
                newData = true;
            }
            catch (Exception ex)
            {
            }
            
        }
        
        #endregion //Events
        
    }
}
