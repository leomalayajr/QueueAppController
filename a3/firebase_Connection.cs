using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace a3
{
    public class firebase_Connection
    {
        private const String databaseUrl = "https://usep-queue-app.firebaseio.com/";
        private const String databaseSecret = "xMkY2DmLnRefSl34kYlp8PWUzDwNmyJAvxLPygQ1";
        private const String databaseKey = "AIzaSyDL4e41J9HTl-OCktBodhqY6l9yDuNrkaU";
        private int run = 0;
        //private const String node = "Queue_Info/";
        private FirebaseClient firebase;
        private FirebaseAuthProvider ap;
        public firebase_Connection()
        {

            this.firebase = new FirebaseClient(
                databaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(databaseSecret)
                });
            this.ap = new FirebaseAuthProvider(new FirebaseConfig(databaseKey));

        }
        public async Task Controller_RegisterThisUser(_App_User _new_user)
        {
            try
            {
                // sort items first
                string email = _new_user.accountNumber + "@usep.edu.ph"; // username
                string password = _new_user.password;
                string displayName = _new_user.lastName + "," + _new_user.firstName + _new_user.middleName;
                var c = await ap.CreateUserWithEmailAndPasswordAsync(
                email, password, displayName, false
                );
            }
            catch (FirebaseAuthException e)
            {
                throw new FirebaseAuthException(e.RequestUrl,e.ResponseData,e.ResponseData,e.InnerException,e.Reason);
            }
            catch (FirebaseException e)
            {
                Console.WriteLine("Problem -> Method: Controller RegisterThisUser" + e.Message);
                throw;
            }
        }
        public async Task<List<_Queue_Request>> App_Retrieve_QueueRequest(CancellationToken cts)
        {
            string node = "Queue_Request/";
            List<_Queue_Request> list_from_online = new List<_Queue_Request>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Queue_Request_String>();
                foreach (var a in retrieved_objects)
                {
                    int val;
                    _Queue_Request b = new _Queue_Request
                    {
                        Action = a.Object.Action,
                        ID = Int32.Parse(a.Object.ID),
                        Queue_ID = Int32.Parse(a.Object.Queue_ID),
                        Servicing_Office = Int32.Parse(a.Object.Servicing_Office),
                        Value = Int32.TryParse(a.Object.Value, out val) ? val : 0
                    };
                    list_from_online.Add(b);
                }
            }
            catch (FirebaseException e)
            {
                Console.WriteLine("Problem -> Method: Retrieve QueueRequest"+e.Message);
                throw;
            }

            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve QueueRequest"); }
            return list_from_online;

        }
        public async Task<List<_Pre_Queue>> App_Retrieve_PreQueue(CancellationToken cts)
        {
            string node = "Pre_Queue/";
            List<_Pre_Queue> list_from_online = new List<_Pre_Queue>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Pre_Queue>();
                foreach (var a in retrieved_objects)
                {
                    a.Object.Key = a.Key;
                    list_from_online.Add(a.Object);
                }
            }
            catch (FirebaseException e)
            {
                Console.WriteLine("Problem -> Method: Retrieve PreQueue");
                throw;
            }

            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve PreQueue"); }
            return list_from_online;

        }
        public async Task<List<_Main_Queue>> App_Retrieve_MainQueueAsync(CancellationToken cts)
        {
            string node = "Main_Queue/";
            List<_Main_Queue> list_from_online = new List<_Main_Queue>();
            try {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Main_Queue>();
                foreach (var a in retrieved_objects)
                {
                    a.Object.Key = a.Key;
                    list_from_online.Add(a.Object);
                }
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Retrieve MainQueue"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve MainQueue"); }
            return list_from_online;
        }
        public async Task<List<string>> CQN_Retrieve_MainQueueAsync(CancellationToken cts)
        {
            string node = "Main_Queue/";
            List<string> list_from_online = new List<string>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Main_Queue>();
                foreach (var a in retrieved_objects)
                {
                    a.Object.Key = a.Key;
                    list_from_online.Add(a.Object.Customer_Queue_Number);
                }
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Retrieve CQN MainQueue"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve CQN MainQueue"); }
            return list_from_online;
        }
        public async Task<List<_Queue_Info>> App_Retrieve_QueueInfoAsync(CancellationToken cts)
        {
            string node = "Queue_Info/";
            List<_Queue_Info> list_from_online = new List<_Queue_Info>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Queue_Info>();
                foreach (var a in retrieved_objects)
                {
                    list_from_online.Add(a.Object);
                }
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Retrieve QueueInfo"); }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve QueueInfo"); }
            return list_from_online;
        }
        public async Task<List<_Transfer_Queue>> App_Retrieve_TransferQueueAsync(CancellationToken cts)
        {
            string node = "Transfer_Queue/";
            List<_Transfer_Queue> list_from_online = new List<_Transfer_Queue>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Transfer_Queue>();
                foreach (var a in retrieved_objects)
                {
                    list_from_online.Add(a.Object);
                }
            }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Insert MainQueue"); }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert MainQueue"); }
            return list_from_online;
        }
        public async Task<List<_Servicing_Terminal>> App_Retrieve_ServicingTerminalAsync(CancellationToken cts)
        {
            string node = "Servicing_Terminal/";
            List<_Servicing_Terminal> list_from_online = new List<_Servicing_Terminal>();
            try
            {
                cts.ThrowIfCancellationRequested();
                var retrieved_objects = await firebase.Child(node).OnceAsync<_Servicing_Terminal>();
                foreach (var a in retrieved_objects)
                {
                    list_from_online.Add(a.Object);
                }
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Retrieve ServicingTerminal"); }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Retrieve ServicingTerminal"); }

            return list_from_online;
        }
        public async Task App_Insert_MainQueueAsync(_Main_Queue mq, bool Guest, CancellationToken cts)
        {
            try
            {
                cts.ThrowIfCancellationRequested();
                //Guest is True, Student is False
                if (Guest)
                    await Task.Run(() => firebase.Child("Main_Queue/").PostAsync<_Main_Queue>(mq));
                else
                    await Task.Run(() => firebase.Child("Main_Queue/").Child(mq.Student_No).PutAsync<_Main_Queue>(mq));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert MainQueue"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Insert MainQueue"); }
        }
        public async Task App_Insert_QueueInfoAsync(_Queue_Info qi, CancellationToken cts)
        {
            string ChildRowName = qi.Servicing_Office.ToString();
            try
            {
                cts.ThrowIfCancellationRequested();
                await Task.Run(() => firebase.Child("Queue_Info/").Child(ChildRowName).PutAsync<_Queue_Info>(qi));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert QueueInfo"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Insert QueueInfo"); }


        }
        public async Task App_Insert_TransactionTypeAsync(_Transaction_Type _tt_t)
        {
            string ChildName = _tt_t.id.ToString();
            try
            {
                await Task.Run(() => firebase.Child("Transaction_Type/").Child(ChildName).PutAsync<_Transaction_Type>(_tt_t));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert TransactionType"); throw; }

        }
        public async Task App_Insert_TransferQueueAsync(_Transfer_Queue _t_q, bool Guest,CancellationToken cts)
        {
            try
            {
                cts.ThrowIfCancellationRequested();
            // Guest is True, Student is False
            if (Guest)
                await Task.Run(() => firebase.Child("Transfer_Queue/").PostAsync<_Transfer_Queue>(_t_q));
            else
                await Task.Run(() => firebase.Child("Transfer_Queue/").Child(_t_q.Student_No).PutAsync<_Transfer_Queue >(_t_q));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert TransferQueue"); }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Insert TransferQueue"); }

        }
        public async Task App_Insert_ServicingTerminalAsync(_Servicing_Terminal _st,CancellationToken cts)
        {
            
            string atServicingOffice = _st.Servicing_Office.ToString();
            string atWindow = _st.Window.ToString();
            try
            {
                cts.ThrowIfCancellationRequested();
                await Task.Run(() => firebase.Child("Servicing_Terminal/").Child(atServicingOffice).Child(atWindow).PutAsync<_Servicing_Terminal>(_st));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Insert ServicingTerminal"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Insert ServicingTerminal"); }


        }
        public async Task Specific_Delete_MainQueueAsync(string key, CancellationToken cts)
        {
            string node = "Main_Queue/";
            try { cts.ThrowIfCancellationRequested(); await firebase.Child(node).Child(key).DeleteAsync(); }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: (Specific Delete) MainQueue"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: (Specific Delete) MainQueue"); }

        }
        public async Task App_Delete_QueueRequest(CancellationToken cts)
        {
            string node = "Queue_Request/";
            try { cts.ThrowIfCancellationRequested(); await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete QueueRequest"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Delete QueueRequest"); }

        }
        public async Task App_Delete_PreQueueAsync(CancellationToken cts)
        {
            string node = "Pre_Queue/";
            try { cts.ThrowIfCancellationRequested(); await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete PreQueue"); throw; }
            catch (OperationCanceledException e) { Console.WriteLine("Cancelled -> Method: Delete PreQueue"); }

        }
        public async Task App_Delete_MainQueueAsync()
        {
            string node = "Main_Queue/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete MainQueue"); }
            finally { Console.Write("MainQueue is deleted."); }
        }
        public async Task App_Delete_TransactionTypeAsync()
        {
            string node = "Transaction_Type/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete TransactionType"); }
            finally { Console.Write("Delete finished."); }
        }
        public async Task App_Delete_QueueInfoAsync()
        {
            string node = "Queue_Info/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete QueueInfo"); }
            finally { Console.Write("QueueInfo is deleted."); }

        }
        public async Task App_Delete_TransferQueueAsync()
        {
            string node = "Transfer_Queue/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete TransferQueue"); }
            finally { Console.Write("TransferQueue is deleted."); }

        }
        public async Task App_Delete_ServicingTerminalAsync()
        {
            string node = "Servicing_Terminal/";
            try { await Task.Run(() => firebase.Child(node).DeleteAsync()); }
            catch (FirebaseException e) { Console.Write("Problem -> Method: Delete ServicingTerminal"); }
            finally { Console.Write("ServicingTerminal is deleted."); }

        }
        public async Task App_Delete_QueueTransactionAsync()
        {
            string node = "Queue_Transaction/";
            try { await firebase.Child(node).DeleteAsync(); }
            catch (Exception e) { Console.Write("Delete failed ! Error ->" + e); }
            finally { Console.Write("Delete finished."); }
        }
        public async Task App_Update_MainQueue(_Main_Queue mq)
        {
            string node = "Main_Queue/";
            try { await firebase.Child(node).Child(mq.Key).PutAsync<_Main_Queue>(mq); }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Update MainQueue"); throw; }
            
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
        public async Task Controller_ImportUsers(_App_User _au)
        {
            string Student_No = "unknown";
            try
            {
                Student_No = _au.accountNumber;
                await Task.Run(() => firebase.Child("AccountsTest/").Child(Student_No).PutAsync<_App_User>(_au));
            }
            catch (FirebaseException e) { Console.WriteLine("Problem -> Method: Controller ImportUsers"); throw; }
            
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

    }
}
