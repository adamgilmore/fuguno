namespace Fuguno.UI.ViewModels
{
    using Fuguno.Tfs;
    using System;
    using System.ComponentModel;

    internal class BuildInfoViewModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _isLoading;
        private string _buildNumber;
        private string _status;
        private DateTime? _startTime;
        private DateTime? _finishTime;
        private DateTime? _lastChangedTime;
        private string _requestedFor;
        private long _totalTestCount;
        private long _totalTestPassedCount;

        public string Name 
        {   
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged("IsLoading"); }
        }

        public string BuildNumber
        {
            get { return _buildNumber; }
            set { _buildNumber = value; OnPropertyChanged("BuildNumber"); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public DateTime? StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged("StartTime"); OnPropertyChanged("ElapsedTime");  }
        }

        public DateTime? FinishTime
        {
            get { return _finishTime; }
            set { _finishTime = value; OnPropertyChanged("FinishTime"); }
        }


        public DateTime? LastChangeTime
        {
            get { return _lastChangedTime; }
            set { _lastChangedTime = value; OnPropertyChanged("LastChangedTime"); OnPropertyChanged("ElapsedTime"); }
        }

        public string RequestedFor
        {
            get { return _requestedFor; }
            set { _requestedFor = value; OnPropertyChanged("RequestedFor"); }
        }

        public long TotalTestCount
        {
            get { return _totalTestCount; }
            set { _totalTestCount = value; OnPropertyChanged("TotalTestCount"); OnPropertyChanged("TestPassRate"); }
        }

        public long TotalTestPassedCount
        {
            get { return _totalTestPassedCount; }
            set { _totalTestPassedCount = value; OnPropertyChanged("TotalTestPassedCount"); OnPropertyChanged("TestPassRate"); }
        }

        public TimeSpan? ElapsedTime 
        { 
            get { return StartTime == null ? null : LastChangeTime - StartTime; } 
        }

        public double? TestPassRate
        {
            get
            {
                if (TotalTestCount == 0) 
                    return null;
                else
                    return (double)TotalTestPassedCount / (double)TotalTestCount;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BuildInfoViewModel(string name, bool isLoading)
        {
            _name = name;
            _isLoading = isLoading;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
