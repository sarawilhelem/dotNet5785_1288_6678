using BO;
using System;
using System.Windows.Input;

namespace PL.Call
{
    internal class RelayCommand<T> : ICommand
    {
        private Action<VolunteerInList> deleteVolunteer;

        public RelayCommand(Action<CallInList> deleteItem)
        {
            DeleteItem = deleteItem;
        }

        public RelayCommand(Action<VolunteerInList> deleteVolunteer)
        {
            this.deleteVolunteer = deleteVolunteer;
        }

        public Action<CallInList> DeleteItem { get; }
    }
}