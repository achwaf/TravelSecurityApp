using SecurityTravelApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Message : Matchable, INotifyPropertyChanged
    {
        public Guid ID { get; set; }
        public String _text;
        public DateTime _dateSent = DateTime.MaxValue; // maxvalue chosen to be default to get corrected ordered list by dateSent
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
                if (_isSent)
                {
                    return _dateSent.ToShortDateString() + " ";
                }
                else return "";
            }
        }

        public String WaitingString
        {
            get
            {
                if (!_isSent)
                {
                    return I18n.GetText(AppTextID.WAITING_FOR_TRANSFER);
                }
                else return "";
            }
        }

        public String TimeString
        {
            get
            {
                if (_isSent)
                {
                    return _dateSent.ToShortTimeString();
                }
                else return "";
            }
        }
        public Message() { ID = Guid.NewGuid(); }

        public Message(String pText, Boolean pIsSent, String pDateSent) : this()
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
