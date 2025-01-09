using System;

namespace Functions.TCP
{
    public interface ICoummunication : IDisposable
    {
        event EventHandler<DataRecievedEventArgs> DataRecieved;
        EnCommuincationStatus ConnectionStatus { get; }
        void Send(string data, bool headAndTail = true, bool debugLog = false);
        string Header { get; set; }
        string Tail { get; set; }
        void Connect();
        void DisConnect();
        string SendAndResponse(string data, int sleeptime, bool headAndTail, bool debugLog);

    }
    public enum EnCommuincationStatus
    {
        NeverConnected,
        Connecting,
        Connected,
        DisConnectedByUser,
        DisconnectedByHostOrClient,
        Error,
        PortNeverOpen,
        PortOpen,
        PortClosed,
    }
}
