using System;

namespace Functions.TCP
{
    public class DataRecievedEventArgs : EventArgs
    {

        string _msg;
        public DataRecievedEventArgs(string msg)
        {
            _msg = msg;
        }
        public string Message
        {
            get { return _msg; }
            protected set { _msg = value; }
        }
    }
    public class StatusChangedEventArgs : EventArgs
    {
        EnCommuincationStatus status;
        public StatusChangedEventArgs(EnCommuincationStatus _status)
        {
            status = _status;
        }
        public EnCommuincationStatus Status
        {
            get { return status; }
            protected set { status = value; }
        }
    }



}
