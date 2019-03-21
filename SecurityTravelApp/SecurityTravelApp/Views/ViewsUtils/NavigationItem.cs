using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SecurityTravelApp.Views.ViewsUtils
{
    public class NavigationItem : INotifyPropertyChanged
    {
        public int id;
        private String _text;
        private String _font;
        private String _icon;
        private NavigationItemState _state;
        private Boolean _carriesNotif;
        private int _numberOfNotif;
        private NavigationItemNotifType _notifType;
        private NavigationItemTarget _target;



        public NavigationItemTarget target { get; set; }
        public String text { get; set; }
        public String font { get; set; }
        public String icon { get; set; }
        public NavigationItemNotifType notifType { get; set; }
        public NavigationItemState state
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        public Boolean carriesNotif
        {
            get { return _carriesNotif; }
            set
            {
                _carriesNotif = value;
                OnPropertyChanged();
                OnPropertyChanged("showNumericalNotif");
                OnPropertyChanged("showDotNotif");
            }
        }
        public int numberOfNotif
        {
            get { return _numberOfNotif; }
            set
            {
                _numberOfNotif = value;
                OnPropertyChanged();
            }
        }

        public Boolean showDotNotif
        {
            get
            {
                if (notifType == NavigationItemNotifType.Dot && _carriesNotif)
                {
                    return true;
                }
                else return false;
            }
        }

        public Boolean showNumericalNotif
        {
            get
            {
                if (notifType == NavigationItemNotifType.Numerical && _carriesNotif)
                {
                    return true;
                }
                else return false;
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

    public enum NavigationItemState
    {
        Current,
        Shaded
    }

    public enum NavigationItemTarget
    {
        Login,
        Home,
        Messages,
        Warnings,
        Docs
    }

    public enum NavigationItemNotifType
    {
        Dot,
        Numerical
    }

    public class NavigationItemFont
    {
        static public readonly string FASolid = "FontAwesomeSolid";
        static public readonly string FARegular = "FontAwesomeRegular";
    }
}
