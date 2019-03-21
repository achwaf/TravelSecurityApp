using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Message : INotifyPropertyChanged
    {
        public String _text;
        public DateTime _dateSent;
        public Boolean _isSent;


        public Boolean isSent
        {
            get
            {
                return _isSent;
            }

            set
            {
                _isSent = value;
                OnPropertyChanged();
            }
        }

        public String text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public DateTime dateSent
        {
            get
            {
                return _dateSent;
            }

            set
            {
                _dateSent = value;
                OnPropertyChanged();
                OnPropertyChanged("DateString");
                OnPropertyChanged("TimeString");
            }
        }

        public String DateString
        {
            get
            {
                if (_dateSent != DateTime.MinValue)
                {
                    return _dateSent.ToShortDateString();
                }
                else return "En attente d'envoi";
            }
        }

        public String TimeString
        {
            get
            {
                if (_dateSent != DateTime.MinValue)
                {
                    return _dateSent.ToShortTimeString();
                }
                else return "";
            }
        }

        public Message(String pText, Boolean pIsSent, String pDateSent)
        {
            _text = pText;
            _isSent = pIsSent;
            if (!String.IsNullOrEmpty(pDateSent))
            {
                _dateSent = DateTime.Parse(pDateSent);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
