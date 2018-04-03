
namespace a3
{
    public class _Queue_Info
    {
        public string Key { get; set; }
        public int ID { get; set; }
        public int Current_Number { get; set; }
        public int Current_Queue { get; set; }
        public int Servicing_Office { get; set; }
        public int Counter { get; set; }
        public int Mode { get; set; }
        public string Status { get; set; }
        public int Window { get; set; }
        public string Customer_Queue_Number { get; set; }
        public int Avg_Serving_Time { get; set; } = 0;
        public string Office_Name { get; set; }
        public bool SameObject(object obj)
        {
            if (!(obj is _Queue_Info))
                return false;

            var other = obj as _Queue_Info;

            if (ID != other.ID)
                return false;

            return true;
        }

    }
}