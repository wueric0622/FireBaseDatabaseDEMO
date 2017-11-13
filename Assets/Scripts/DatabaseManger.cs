using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;


public class DatabaseManger: MonoBehaviour {

    [SerializeField] Text LevelInput;
    [SerializeField] Text MessageInput;
    [SerializeField] Text OutputLog;
    [SerializeField] string DatabaseID;

    DatabaseReference reference;

    void Start() {
    // Set this before calling into the realtime database.
    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://helloworld-6b82c.firebaseio.com/");
	reference = FirebaseDatabase.DefaultInstance.RootReference;
  }

	public void SaveData()
	{
		if(LevelInput.text != "" && MessageInput.text !="")
		{
            Data OutputData = new Data();
            OutputData.Level = int.Parse(LevelInput.text);
            OutputData.Message = MessageInput.text;
            string json = JsonUtility.ToJson(OutputData);
            reference.Child("OutputData").SetRawJsonValueAsync(json);
        }
	}

	public void LoadData()
	{
        Data InputData = new Data();
        FirebaseDatabase.DefaultInstance.GetReference("OutputData").GetValueAsync().ContinueWith(
            task =>
            {
                if (task.IsFaulted)
                {
                    OutputLog.text = "Something is wrong";
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
					JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), InputData);
                    OutputLog.text = "Level : " + InputData.Level + "\nMessage : " + InputData.Message + "\nThis data is from Firebase Realtime Database";

                }
            }
            );

    }

}


public class Data{

    public int Level;
    public string Message;

}
