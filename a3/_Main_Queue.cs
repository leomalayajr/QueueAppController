﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
    public class _Main_Queue
    {
        public string Key { get; set; }
        public int Queue_Number { get; set; }
        public string Full_Name { get; set; }
        public int Servicing_Office { get; set; }
        public int Transaction_Type { get; set; }
        // Student_No is stored as string, because of dashes and possible value 'Guest'
        public string Student_No { get; set; }
        public int Pattern_Current { get; set; }
        public int Pattern_Max { get; set; }
        public int ID { get; set; }
        public string Customer_Queue_Number { get; set; }
        public string Type { get; set; }
        public string Queue_Status { get; set; }
        public DateTime Time { get; set; }

    }
}
