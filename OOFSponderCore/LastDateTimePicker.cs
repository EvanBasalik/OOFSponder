﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOFScheduling
{
    //extend existing class so we can store the old value
    internal class LastDateTimePicker : DateTimePicker
    {
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);

            LastValue = Value;
        }

        public DateTime LastValue { get; private set; }

        public new DateTime Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value;
            }
        }
    }
}
