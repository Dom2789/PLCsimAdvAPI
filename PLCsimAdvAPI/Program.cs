// See https://aka.ms/new-console-template for more information

using PLCsimAdvAPI;

Console.WriteLine("Hello, World!");

var plcInstance = new PLCInstance("corrcon");

while (true)
{
    if (plcInstance.newData)
    {
        plcInstance.Inputbuffer = plcInstance.Outputbuffer;
        plcInstance.newData = false;
        plcInstance.firstCycle = false;
    }
}


