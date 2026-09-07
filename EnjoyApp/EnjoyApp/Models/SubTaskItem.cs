using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class SubTaskItem
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool isCompleted;

		public bool IsCompleted
		{
			get { return isCompleted; }
			set { isCompleted = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

        private TimeOnly startTime;

        public TimeOnly StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private TimeOnly endTime;

        public TimeOnly EndTime
        {
            get { return endTime; }
            set { endTime = value; }


        }

		private string description;

		public string Description
        {
			get { return description; }
			set { description = value; }
		}





	}
}
