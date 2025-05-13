using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class DataBaseManager : MonoBehaviour
{
    [SerializeField] private string UserID;
    [SerializeField] private StudentSO studentSO;

    private DatabaseReference reference;

    // Start is called before the first frame update
    private void Awake()
    {
        UserID = SystemInfo.deviceUniqueIdentifier;
    }

    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void UpdloadStudent()
    {
        Student newStudent = studentSO.GetBasicStudentData();

        string json = JsonUtility.ToJson(newStudent);

        reference.Child("Students").Child(UserID).Child(newStudent.nickName).SetRawJsonValueAsync(json);
    }
}

[System.Serializable]
public class Student
{
    public string name;
    public string nickName;
    public int age; 
    public int id;
    public string career;

    public Student(string name, string nickName, int age, int id, string career)
    {
        this.name = name;
        this.nickName = nickName;
        this.age = age;
        this.id = id;
        this.career = career;
    }
}
