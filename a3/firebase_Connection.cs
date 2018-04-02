using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace a3
{
    public class firebase_Connection
    {
        private const String databaseUrl = "https://usep-queue-app.firebaseio.com/";
        private const String databaseSecret = "xMkY2DmLnRefSl34kYlp8PWUzDwNmyJAvxLPygQ1";
        private int run = 0;
        //private const String node = "Queue_Info/";
        private FirebaseClient firebase;

        public firebase_Connection()
        {

            this.firebase = new FirebaseClient(
                databaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(databaseSecret)
                });

        }
        public async Task App_Insert_MainQueueAsync(_Main_Queue mq, bool Guest)
        {
            try
            {
            //Guest is True, Student is False
            if (Guest)
                await Task.Run(() => firebase.Child("Main_Queue/").PostAsync<_Main_Queue>(mq));
            else
                await Task.Run(() => firebase.Child("Main_Queue/").Child(mq.Student_No).PutAsync<_Main_Queue>(mq));
            }
            catch (FirebaseException e) { Console.WriteLine("Error ->" + e.InnerException); }
        }
        public async Task App_Insert_QueueInfoAsync(_Queue_Info qi)
        {
            string ChildRowName = qi.Servicing_Office.ToString();
            try
            {
                await Task.Run(() => firebase.Child("Queue_Info/").Child(ChildRowName).PutAsync<_Queue_Info>(qi));
            }
            catch (FirebaseException e) { Console.WriteLine("Error ->" + e.InnerException); }
            
        }
        public async Task App_Insert_TransactionTypeAsync(_Transaction_Type _tt_t)
        {
            string ChildName = _tt_t.id.ToString();
            try
            {
                await Task.Run(() => firebase.Child("Transaction_Type/").Child(ChildName).PutAsync<_Transaction_Type>(_tt_t));
            }
            catch (FirebaseException e) { Console.WriteLine("Error ->" + e.InnerException); }
        }
        public async Task App_Insert_TransferQueueAsync(_Transfer_Queue _t_q, bool Guest)
        {
            try
            {
            // Guest is True, Student is False
            if (Guest)
                await Task.Run(() => firebase.Child("Transfer_Queue/").PostAsync<_Transfer_Queue>(_t_q));
            else
                await Task.Run(() => firebase.Child("Transfer_Queue/").Child(_t_q.Student_No).PutAsync<_Transfer_Queue >(_t_q));
            }
            catch (FirebaseException e) { Console.WriteLine("Error ->" + e.InnerException); }
        }
        public async Task App_Insert_ServicingTerminalAsync(_Servicing_Terminal _st)
        {
            
            string atServicingOffice = _st.Servicing_Office.ToString();
            string atWindow = _st.Window.ToString();
            try
            {
                await Task.Run(() => firebase.Child("Servicing_Terminal/").Child(atServicingOffice).Child(atWindow).PutAsync<_Servicing_Terminal>(_st));
            }
            catch (FirebaseException e) { Console.WriteLine("Error ->" + e.InnerException); }
            
        }
        public async Task App_Delete_MainQueueAsync()
        {
            string node = "Main_Queue/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }

        }
        public async Task App_Delete_TransactionTypeAsync()
        {
            string node = "Transaction_Type/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }
        }
        public async Task App_Delete_QueueInfoAsync()
        {
            string node = "Queue_Info/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }

        }
        public async Task App_Delete_TransferQueueAsync()
        {
            string node = "Transfer_Queue/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }

        }
        public async Task App_Delete_ServicingTerminalAsync()
        {
            string node = "Servicing_Terminal/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }

        }
        public async Task App_Delete_QueueTransactionAsync()
        {
            string node = "Queue_Transaction/";
            try { await firebase.Child(node).DeleteAsync(); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }
        }
        public async Task App_Update_MainQueue(int where, _Main_Queue mq)
        {
            run++;
            string node = "Main_Queue/";
            Console.WriteLine("patch async App Update MainQueue on -> MainQueue" + run);
            Console.WriteLine("Received values are the following:" + mq.Queue_Number + " " + mq.Servicing_Office + " " + mq.Pattern_Current + " " + where);

            string key = "";
            var cc = await firebase.Child(node).OrderBy("ID").StartAt(where).EndAt((where + 1)).LimitToFirst(1).OnceAsync<_Main_Queue>();
            foreach (var b in cc)
            {
                key = b.Key;
                // Passing variables to the new _Main_Queue updated class
                mq.ID = b.Object.ID;
                mq.Transaction_Type = b.Object.Transaction_Type;
                mq.Student_No = b.Object.Student_No;
                mq.Customer_Queue_Number = b.Object.Customer_Queue_Number;
            }
            Console.WriteLine(run + " Query is > " + node + key + mq.ID);
            try { await firebase.Child(node).Child(key).PatchAsync(mq); }
            catch (Exception e) { Console.Write("error ->" + e); }
            finally { Console.Write("Error happened: Update finished[?]"); }


        }
        public async Task InsertMultiple()
        {
            int x = 1;
            for (x = 1; x <= 10; x++)
            {
                _Queue_Info aa = new _Queue_Info { Servicing_Office = x };
                await firebase.Child("Queue_Info/").PostAsync<_Queue_Info>(aa);
            }

        }

        public async Task Delete(_Queue_Info q_info)
        {
            //>await firebase.Child(node).Child(q_info.Key).DeleteAsync();

        }
        public async void App_Update_QueueInfo(int where, _Queue_Info q_info)
        {
            string node = "Queue_Info/";
            Console.WriteLine("App Update Child running");

            string key = "";
            var cc = await firebase.Child(node).OrderBy("Servicing_Office").StartAt(where).EndAt((where + 1)).LimitToFirst(1).OnceAsync<_Queue_Info>();
            foreach (var b in cc) { key = b.Key; }

            Console.WriteLine("Key returned is " + key);

            try { await firebase.Child(node).Child(key).PatchAsync<_Queue_Info>(q_info); }
            catch (Exception e) { Console.Write("error ->" + e); }
            finally { Console.Write("Update finished."); }

        }
        //public async void Update(string key, _Queue_Info q_info)
        //{
        //    Console.WriteLine("Opening Update...");
        //    //>await firebase.Child(node).Child(key).PatchAsync<_Queue_Info>(q_info);
        //    //>Console.WriteLine(node + key);
        //}
        //public async Task Insert(_Queue_Info q_info)
        //{
        //    Console.Write("running...");
        //    //>await firebase.Child(node).PostAsync<_Queue_Info>(q_info);
        //    Console.Write("done?");
        //}

    }
}
